using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraController : MonoBehaviour
{

    public static CameraController Instance;

    private float
        zoomMax = 85,
        zoomMin = 30;

    public float zoomStep = 10f;
       

    [SerializeField]
    private Camera cam; 

    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();           
    }  
   
    public void MouseZoomOut()
    {
        if (cam.fieldOfView <= zoomMax)
        {
            cam.fieldOfView += zoomStep;
        }
    }

    public void MouseZoomIn()
    {
        if (cam.fieldOfView >= zoomMin)
        {
            cam.fieldOfView -= zoomStep;
        }
    }

}