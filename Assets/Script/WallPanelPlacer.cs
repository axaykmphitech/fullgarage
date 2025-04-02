using UnityEngine;

public class WallPanelPlacer : MonoBehaviour
{
    public GameObject panelPrefab;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isDragging = false;
    private GameObject currentPanel;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = GetMouseWorldPosition();
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            endPos = GetMouseWorldPosition();
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            PlacePanel(startPos, endPos);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void PlacePanel(Vector3 start, Vector3 end)
    {
        if (currentPanel != null) Destroy(currentPanel);

        Vector3 center = (start + end) / 2;
        Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y), 0.1f);

        currentPanel = Instantiate(panelPrefab, center, Quaternion.identity);
        currentPanel.transform.localScale = size;

        Renderer renderer = currentPanel.GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector2 textureScale = new Vector2(size.x, size.y);
            renderer.material.mainTextureScale = textureScale ;
        }
    }
}
