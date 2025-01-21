using UnityEngine;

public class LookAround : MonoBehaviour
{
    public Transform target; // The point around which the camera should rotate
    public float rotationSpeed = 0.5f;

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    private void Update()
    {
        // Check for mouse button down to start dragging
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        // Check for mouse button up to stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Rotate the camera while dragging
        if (isDragging)
        {
            // Calculate the mouse movement delta
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            // Rotate the camera around the target based on mouse movement
            transform.RotateAround(target.position, Vector3.up, mouseDelta.x * rotationSpeed);
            transform.RotateAround(target.position, transform.right, -mouseDelta.y * rotationSpeed);

            // Update the last mouse position
            lastMousePosition = Input.mousePosition;
        }
    }
}
