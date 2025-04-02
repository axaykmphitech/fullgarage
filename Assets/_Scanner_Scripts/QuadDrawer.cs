using UnityEngine;

public class QuadDrawer : MonoBehaviour
{
    public GameObject quadPrefab;
    private GameObject currentQuad;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool isDrawing;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = GetMouseWorldPosition();
            currentQuad = Instantiate(quadPrefab, startPoint, Quaternion.identity);
            currentQuad.transform.localScale = Vector3.zero;
            isDrawing = true;
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {   
            endPoint = GetMouseWorldPosition();
            UpdateQuad(currentQuad, startPoint, endPoint);
        }

        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    void UpdateQuad(GameObject quad, Vector3 start, Vector3 end)
    {   
        Vector3 center = (start + end) / 2;
        quad.transform.position =   center;
        
        Vector3 size = new Vector3(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y), 1);
        quad.transform.localScale = new Vector3(size.x, size.y, 1);
    }
}


//using UnityEngine;

//public class QuadDrawer : MonoBehaviour
//{
//    public GameObject quadPrefab;
//    private GameObject currentQuad;
//    private Vector3 startMousePosition;
//    private Vector3 endMousePosition;
//    private bool isDrawing;

//    void Update()
//    {
//        // Start drawing on mouse down
//        if (Input.GetMouseButtonDown(0))
//        {
//            startMousePosition = GetMouseWorldPosition();
//            currentQuad = Instantiate(quadPrefab, startMousePosition, Quaternion.identity);
//            currentQuad.transform.localScale = Vector3.zero;
//            isDrawing = true;
//        }

//        // Update quad size while dragging
//        if (isDrawing && Input.GetMouseButton(0))
//        {
//            endMousePosition = GetMouseWorldPosition();
//            Vector3 center = (startMousePosition + endMousePosition) / 2;
//            Vector3 size = endMousePosition - startMousePosition;

//            currentQuad.transform.position = center;
//            currentQuad.transform.localScale = new Vector3(Mathf.Abs(size.x), 1, Mathf.Abs(size.z));

//            // Draw the corners
//            DrawCorners(center, size);
//        }

//        // Stop drawing on mouse release
//        if (Input.GetMouseButtonUp(0))
//        {
//            isDrawing = false;
//        }
//    }

//    // Get mouse position in world space
//    private Vector3 GetMouseWorldPosition()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out RaycastHit hit))
//        {
//            return hit.point;
//        }
//        return Vector3.zero;
//    }

//    // Draw corners as spheres
//    private void DrawCorners(Vector3 center, Vector3 size)
//    {
//        Vector3 halfSize = new Vector3(size.x / 2, 0, size.z / 2);

//        Vector3[] corners = new Vector3[4];
//        corners[0] = center + new Vector3(-halfSize.x, 0, -halfSize.z); // Bottom-left
//        corners[1] = center + new Vector3(halfSize.x, 0, -halfSize.z);  // Bottom-right
//        corners[2] = center + new Vector3(-halfSize.x, 0, halfSize.z);  // Top-left
//        corners[3] = center + new Vector3(halfSize.x, 0, halfSize.z);   // Top-right

//        foreach (Vector3 corner in corners)
//        {
//            Debug.DrawLine(corner, corner + Vector3.up * 0.5f, Color.red);
//        }
//    }
//}
