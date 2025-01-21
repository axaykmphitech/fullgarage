using UnityEngine;

public class DoubleClickDetector : MonoBehaviour
{
    public float doubleClickTime = 0.3f;
    private float lastClickTime;
    public float offset;
    [HideInInspector]
    public GameObject wallCameraObject;
    public Material normalWallMaterial;
    public Material selectedWallManterial;
    public static DoubleClickDetector Instance;
    public bool isWallOpen;
    public Transform selectedWall;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(!isWallOpen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name.Contains("wall"))
                {
                    if(selectedWall != null)
                    {
                        selectedWall.GetComponent<Renderer>().material = normalWallMaterial;
                    }

                    selectedWall = hit.collider.transform;
                    hit.collider.GetComponent<Renderer>().material = selectedWallManterial;
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                float timeSinceLastClick = Time.time - lastClickTime;

                if (timeSinceLastClick <= doubleClickTime)
                {
                    OnDoubleClick(hit.collider.gameObject, hit.normal);
                }
                lastClickTime = Time.time;
            }
        }
    }

    private void OnDoubleClick(GameObject hitObject, Vector3 normal)
    {
        MakeCamera(hitObject, normal);
    }

    private void MakeCamera(GameObject targetObject, Vector3 normal)
    {
        DisableAllWallsExceptSelected(targetObject.transform);
        InputManager.Instance.wallNormal = normal;

        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }
        isWallOpen = true;
        selectedWall.GetComponent<Renderer>().material = normalWallMaterial;
        // Create a new GameObject for the camera
        wallCameraObject = new GameObject("OrthographicCamera");

        // Add a Camera component
        Camera camera = wallCameraObject.AddComponent<Camera>();

        wallCameraObject.AddComponent<WallCamera>();

        camera.backgroundColor = Color.gray;
        // Set the camera to orthographic mode
        camera.orthographic = true;

        // Set the orthographic size
        camera.orthographicSize = 5;
        // Make the camera look at the target object

        if (normal == new Vector3(1, 0, 0))
        {
            camera.transform.forward = -targetObject.transform.right;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(-offset,0,0);
            //right
        }
        else if (normal == new Vector3(-1, 0, 0))
        {
            camera.transform.forward = targetObject.transform.right;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(offset,0,0);
            //left
        }
        else if (normal == new Vector3(0, 0, -1))
        {
            camera.transform.forward = targetObject.transform.forward;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(0,0,offset);
            //backward
        }
        else if (normal == new Vector3(0, 0, 1))
        {
            camera.transform.forward = -targetObject.transform.forward;
            camera.transform.position = targetObject.GetComponent<Collider>().bounds.center + new Vector3(0,0, -offset);
            //forward
        }
        UIManager.Instance.OpenWallUi();
    }

    private void DisableAllWallsExceptSelected(Transform selectedWall)
    {
        foreach (Transform item in RoomModelManager.Instance.RoomModelParent.transform.GetChild(0).transform)
        {
            if(item != selectedWall)
            {
                item.gameObject.SetActive(false);/////
            }
        }
    }
}
