using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Movement : MonoBehaviour
{
    
    public GameObject cylinder;
    private float minFov = 35f;
    private float maxFov = 100f;
    private float sensitivity = 17f;
    private float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        ZoomCamera();
    }

    // Rotates the camera around the cylinder when the right mouse button is pressed
    private void RotateCamera()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(cylinder.transform.position, transform.up, Input.GetAxis("Mouse X") * speed);
            transform.RotateAround(cylinder.transform.position, transform.right, Input.GetAxis("Mouse Y") * -speed);
        }
    }

    // Zooms the camera in and out based on the mouse scroll wheel
    private void ZoomCamera()
    {
        
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

     
}
