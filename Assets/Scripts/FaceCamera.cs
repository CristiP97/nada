using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    private Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        
        camera = Camera.main.transform;
        InvokeRepeating("LookAtCamera", 0, 0.2f);
    }

    private void LookAtCamera()
    {

        transform.rotation = Quaternion.LookRotation(transform.position - camera.position);
    }
}
