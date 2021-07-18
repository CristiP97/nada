using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private float distance;
    //private Enemy enemy;
    private bool following;
    private CameraRotationAndMovement cameraRotationScript;
    public float mouseSensitivity = 3f;

    float xAxis;
    float yAxis;

    private Vector2 xMaxRotationAngle = new Vector2(-60, 60);



    private void Start()
    {
        distance = 1.25f;
        cameraRotationScript = Camera.main.GetComponent<CameraRotationAndMovement>();
    }

    private void Update()
    {

        if (following == true)
        {
            // You can go from the enemy view directly to building mode
            // But you'll the player will lose the enemy that it was following
            if(cameraRotationScript.IsBuildingModeActive())
            {
                following = false;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetCameraBack();
                return;
            }

            yAxis += Input.GetAxis("Mouse X") * mouseSensitivity;
            xAxis -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            xAxis = Mathf.Clamp(xAxis, xMaxRotationAngle.x, xMaxRotationAngle.y);

            Vector3 targetRotation = new Vector3(xAxis, yAxis);
            Camera.main.transform.eulerAngles = targetRotation;

            Camera.main.transform.position = transform.position - transform.forward * distance + transform.up * distance * 1.5f;
        }
        
    }

    private void OnDestroy()
    {
        if (IsFollowing())
        {
            GameManager.selectedEnemy = null;
            following = false;
            Camera.main.transform.position = originalCameraPosition;
            Camera.main.transform.rotation = originalCameraRotation;
        }
        
    }

    public void SetCameraBack()
    {
        GameManager.selectedEnemy = null;
        following = false;
        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;
    }

    private void OnMouseDown()
    {
        if (!following)
        {
            // Don't go to enemy view mode if we are in building mode
            // Avoids accidental clicks on enemies while trying to place towers
            if (!cameraRotationScript.IsBuildingModeActive())
            {
                GameManager.selectedEnemy = gameObject;
                following = true;
                originalCameraPosition = Camera.main.transform.position;
                originalCameraRotation = Camera.main.transform.rotation;

                yAxis = gameObject.transform.eulerAngles.y;
                xAxis = gameObject.transform.eulerAngles.x;
            }
        } else
        {
            SetCameraBack();
        }
        

    }

    public bool IsFollowing()
    {
        return following;
    }
}
