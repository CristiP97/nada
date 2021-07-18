using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationAndMovement : MonoBehaviour
{
    float xAxis;
    float yAxis;

    public float mouseSensitivity = 12f;
    public float panSensitivity = 10f;
    public float scrollSpeed = 2000f;
    public GameObject ground;
    public float offsetMultiplier;

    private Vector2 xMaxRotationAngle = new Vector2(-45, 45);
    private bool buildingMode;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private Mesh groundMesh;
    private Transform groundTransform;

    private Vector2 xAxisMinMax;
    private Vector2 zAxisMinMax;


    private void Start()
    {
        groundMesh = ground.GetComponent<MeshFilter>().mesh;
        groundTransform = ground.GetComponent<Transform>();
        float xDistance = groundMesh.bounds.size.x * groundTransform.localScale.x;
        float zDistance = groundMesh.bounds.size.z * groundTransform.localScale.z;

        xAxisMinMax = new Vector2(-xDistance, xDistance);
        zAxisMinMax = new Vector2(-zDistance, zDistance);

        buildingMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If the game has ended, the player ca no longer move around 
        if (GameManager.gameEnded)
        {
            this.enabled = false;
            return;
        }

        // Move to the standard building position when the player presses "F"
        // When pressing "F" again move the player back to where the camera was prior to this moment
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (buildingMode)
            {
                buildingMode = false;
                
                // Keep track of the original camera position
                originalCameraPosition = transform.position;
                originalCameraRotation = transform.rotation;

                //Set the camera to the new building view
                /*float dimensionsSum = groundMesh.bounds.size.x * groundTransform.localScale.x + groundMesh.bounds.size.z * groundTransform.localScale.z;

                transform.position = groundTransform.position + new Vector3(0, dimensionsSum * offsetMultiplier, 0);
                transform.eulerAngles = Vector3.right * 90;*/

            } else
            {
                buildingMode = true;
                transform.position = originalCameraPosition;
                transform.rotation = originalCameraRotation;
            }
        }

        // If the player is in building mode, don't let him accidentaly modify the angle of the camera
        // Since it might make the building process a lot harder
        // Also let him zoom in with the mouse wheel
        if (buildingMode)
        {
            // Camera controls for moving forward, backward, left and right
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * panSensitivity * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(-Vector3.forward * panSensitivity * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-Vector3.right * panSensitivity * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * panSensitivity * Time.deltaTime, Space.World);
            }

            // Zooming in with the mouse wheel since all other buttons are occupied
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Vector3 pos = transform.position;

            pos.y -= scroll * scrollSpeed * Time.deltaTime;

            transform.position = pos;

            //TODO: Maybe clamp the values here as well

            return;
        }

        // Camera controls for moving forward, backward, left and right
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * panSensitivity * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-transform.forward * panSensitivity * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-transform.right * panSensitivity * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * panSensitivity * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(transform.up * panSensitivity * Time.deltaTime, Space.World);

        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(-transform.up * panSensitivity * Time.deltaTime, Space.World);

        }

        // Rotate the camera only when the player keeps the right button mouse pressed
        if (Input.GetKey(KeyCode.Mouse1))
        {
            yAxis += Input.GetAxis("Mouse X") * mouseSensitivity;
            xAxis -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            xAxis = Mathf.Clamp(xAxis, xMaxRotationAngle.x, xMaxRotationAngle.y);

            Vector3 targetRotation = new Vector3(xAxis, yAxis);
            transform.eulerAngles = targetRotation;
        }

        // Clamp the values to make sure that the players doesn't go to infinity and beyond
        // TODO: Maybe clamp on y axis as well
        float clampedX = Mathf.Clamp(transform.position.x, xAxisMinMax.x, xAxisMinMax.y);
        float clampedZ = Mathf.Clamp(transform.position.z, zAxisMinMax.x, zAxisMinMax.y);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    public bool IsBuildingModeActive()
    {
        return buildingMode;
    }
}
