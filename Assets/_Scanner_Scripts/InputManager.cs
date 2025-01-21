using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [HideInInspector]
    public GameObject preGeneratedItem;
    public bool isDragging = false;
    public bool isWallDetected = false;
    public bool isCabinetOnWall = false;

    public Vector3 wallNormal;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (DoubleClickDetector.Instance.isWallOpen)
        {
            if (Input.GetMouseButton(0) && isDragging)
            {
                isCabinetOnWall = false;
                float selectedWallZPositon =  0;
                Camera orthographicCamera;
                orthographicCamera = DoubleClickDetector.Instance.wallCameraObject.GetComponent<Camera>();

                Vector3 mouseWorldPosition = orthographicCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, orthographicCamera.nearClipPlane)
                );

                Vector3 cursorPositon = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y - (preGeneratedItem.transform.localScale.y/2), mouseWorldPosition.z);


                if (preGeneratedItem && !isWallDetected)
                {
                    preGeneratedItem.transform.position = cursorPositon;
                }

                Vector3 direction = orthographicCamera.transform.forward;
                Vector3 origin = preGeneratedItem.GetComponentInChildren<Collider>().bounds.center;

                Ray ray = new Ray(origin, direction);

                Debug.DrawRay(ray.origin, direction * 100, Color.red);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.name.Contains("wall"))
                    {
                        wallNormal = hitInfo.normal;
                        selectedWallZPositon = hitInfo.collider.bounds.center.z;
                        isWallDetected = true;
                        isCabinetOnWall = true;
                    }
                }

                float yPosition = 0;
                if (preGeneratedItem.GetComponent<DraggableItem>().isGroundCabinet)
                    yPosition = RoomModelManager.Instance.floorPositionY;
                else if (preGeneratedItem.GetComponent<DraggableItem>().isWallMount)
                    yPosition = RoomModelManager.Instance.wallmountYPosition;
                else if (preGeneratedItem.GetComponent<DraggableItem>().isOverHead)
                    yPosition = RoomModelManager.Instance.overheadYPosition;


                if (wallNormal == new Vector3(1, 0, 0))
                {
                    Debug.Log("right");
                    preGeneratedItem.transform.forward = Vector3.right;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x - 0.35f, yPosition,cursorPositon.z);
                    //right
                }
                else if (wallNormal == new Vector3(-1, 0, 0))
                {
                    Debug.Log("left");
                    preGeneratedItem.transform.forward = Vector3.left;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x + 0.35f, yPosition, cursorPositon.z);
                    //left
                }
                else if (wallNormal == new Vector3(0, 0, -1))
                {
                    Debug.Log("backward");
                    preGeneratedItem.transform.forward = Vector3.back;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x, yPosition, cursorPositon.z + 0.35f);
                    //backward
                }
                else if (wallNormal == new Vector3(0, 0, 1))
                {
                    Debug.Log("forward");
                    preGeneratedItem.transform.forward = Vector3.forward;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x, yPosition, cursorPositon.z - 0.35f);
                    //forward
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                isWallDetected = false;
                if(!isCabinetOnWall)
                {
                    Destroy(preGeneratedItem.gameObject);
                }
            }
        }
    }
}
