using UnityEngine;
using UnityEngine.EventSystems;

public class WallCamera : MonoBehaviour
{
    public Camera camera;
    private float zoomSpeed = 5f;
    private float minZoom = 0.5f;
    private float maxZoom =  10f;
    private float panSpeed = 15f;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        Pan();
        Zoom();
    }

    private void Zoom()
    {
        if (camera.orthographic)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            camera.orthographicSize -= scroll * zoomSpeed;
            camera.orthographicSize =  Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
        }
    }

    void Pan()
    {
        if (Input.GetMouseButton(0))
        {
            if (!InputManager.Instance.isDragging)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                float deltaX = Input.GetAxis("Mouse X");
                float deltaY = Input.GetAxis("Mouse Y");

                Vector3 panDirection = new Vector3(-deltaX, -deltaY, 0f) * panSpeed * Time.deltaTime;
                camera.transform.Translate(panDirection, Space.Self);
            }
        }
    }
}
