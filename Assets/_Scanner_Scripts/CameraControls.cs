using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class CameraZoomAndPan : MonoBehaviour
{
    public Camera              camera; 
    public float       zoomSpeed = 2f;
    public float         minZoom = 2f;
    public float        maxZoom = 10f;
    private Vector3 lastMousePosition;
    public float rotationSpeed = 100f;
    public float        panSpeed = 5f;

    void Update()
    {
        Pan();
        Zoom();
        Rotate();
    }

    private void Zoom()
    {
        if (camera.orthographic)
        {
            // Get the mouse wheel input
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // Adjust the orthographic size based on scroll input
            camera.orthographicSize -= scroll * zoomSpeed;

            // Clamp the orthographic size to the min and max values
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
        }
    }

    void Rotate()
    {
        if (Input.GetMouseButton(1)) // 1 = Right mouse button
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            float rotationY = mouseDelta.x * rotationSpeed * Time.deltaTime;

            transform.parent.transform.Rotate(0, rotationY, 0, Space.World);
        }
        lastMousePosition = Input.mousePosition;
    }

    void Pan()
    {
        if(!InputManager.Instance.isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
                
                float deltaX = Input.GetAxis("Mouse X");
                float deltaY = Input.GetAxis("Mouse Y");

                Vector3 panDirection = new Vector3(-deltaX, -deltaY, 0f) * panSpeed * Time.deltaTime;
                Camera.main.transform.Translate(panDirection, Space.Self);
            }
        }
    }
}
