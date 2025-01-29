using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public GameObject preGeneratedItem;
    public bool isDragging = false;
    public bool isCenterWallDetected = false;

    public Vector3 wallNormal;
    public Vector3 rayCastOffset;
    public Transform originPositon;
    public Vector3 offsetMultiplier;

    [Space(15)]
    public bool isCabinetOnWallCenter = false;
    public bool isCabinetOnWallLeft   = false;
    public bool isCabietOnWallRight   = false;
    public bool isCabinetOnWallTop    = false;
    public bool isCabinetOnWallBottom = false;

    public bool isHighLightDisplayed  = false;

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
                isCabinetOnWallCenter = false;
                isCabinetOnWallTop =    false;
                isCabietOnWallRight =   false;
                isCabinetOnWallBottom = false;
                isCabinetOnWallLeft =   false;

                float selectedWallZPositon =  0;
                Camera orthographicCamera;
                orthographicCamera = DoubleClickDetector.Instance.wallCameraObject.GetComponent<Camera>();

                Vector3 mouseWorldPosition = orthographicCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, orthographicCamera.nearClipPlane)
                );

                Vector3 cursorPositon = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y - (preGeneratedItem.transform.localScale.y/2), mouseWorldPosition.z);

                if (preGeneratedItem && !isCenterWallDetected)
                {
                    preGeneratedItem.transform.position = cursorPositon;
                }

                preGeneratedItem.GetComponentInChildren<QuikOutline>().enabled = true;

                Vector3 direction = orthographicCamera.transform.forward;
                Bounds bounds = preGeneratedItem.GetComponentInChildren<Collider>().bounds;

                Vector3 centerOrigin = Vector3.zero;
                Vector3 topOrigin    = Vector3.zero;
                Vector3 bottomOrigin = Vector3.zero;
                Vector3 leftOrigin   = Vector3.zero;
                Vector3 rightOrigin  = Vector3.zero;

                centerOrigin = bounds.center;
                topOrigin = centerOrigin + new Vector3(0, bounds.extents.y, 0);
                bottomOrigin = centerOrigin - new Vector3(0, bounds.extents.y, 0);

                if (wallNormal.Equals(Vector3.right))
                {
                    leftOrigin = centerOrigin - new Vector3(0, 0, bounds.extents.z);
                    rightOrigin = centerOrigin + new Vector3(0, 0, bounds.extents.z);
                }
                if (wallNormal.Equals(Vector3.left))
                {
                    leftOrigin = centerOrigin - new Vector3(0, 0, bounds.extents.z);
                    rightOrigin = centerOrigin + new Vector3(0, 0, bounds.extents.z);
                }
                if(wallNormal.Equals(Vector3.forward))
                {
                    leftOrigin = centerOrigin - new Vector3(bounds.extents.x, 0, 0);
                    rightOrigin = centerOrigin + new Vector3(bounds.extents.x, 0, 0);
                }
                if(wallNormal.Equals(Vector3.back))
                {
                    leftOrigin = centerOrigin - new Vector3(bounds.extents.x, 0, 0);
                    rightOrigin = centerOrigin + new Vector3(bounds.extents.x, 0, 0);
                }

                //Ray to center
                Ray rayCenter = new Ray(centerOrigin, direction);
                Debug.DrawRay(rayCenter.origin, direction * 100, Color.red);

                //Ray to top
                Ray rayTop = new Ray(topOrigin, direction);
                Debug.DrawRay(rayTop.origin, direction * 100, Color.blue);

                //Ray to bottom
                Ray rayBottom = new Ray(bottomOrigin, direction);
                Debug.DrawRay(rayBottom.origin, direction * 100, Color.magenta);

                //Ray to right
                Ray rayRight = new Ray(rightOrigin, direction);
                Debug.DrawRay(rayRight.origin, direction * 100, Color.cyan);

                //Ray to left
                Ray rayLeft = new Ray(leftOrigin, direction);
                Debug.DrawRay(rayLeft.origin, direction * 100, Color.green);

                if (Physics.Raycast(rayCenter, out RaycastHit hitInfoCenter))
                {
                    if (hitInfoCenter.collider.name.Contains("wall"))
                    {
                        wallNormal = hitInfoCenter.normal;
                        selectedWallZPositon = hitInfoCenter.collider.bounds.center.z;
                        isCenterWallDetected = true;
                        isCabinetOnWallCenter = true;
                    }
                }
                if (Physics.Raycast(rayTop, out RaycastHit hitInfoTop))
                {
                    if (hitInfoTop.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallTop = true;
                    }
                }
                if (Physics.Raycast(rayBottom, out RaycastHit hitInfoBottom))
                {
                    if (hitInfoBottom.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallBottom = true;
                    }
                }
                if (Physics.Raycast(rayLeft, out RaycastHit hitInfoLeft))
                {
                    if (hitInfoLeft.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallLeft = true;
                    }
                }
                if (Physics.Raycast(rayRight, out RaycastHit hitInfoRight))
                {
                    if (hitInfoRight.collider.name.Contains("wall"))
                    {
                        isCabietOnWallRight = true;
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
                    preGeneratedItem.transform.forward = Vector3.right;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x - 0.35f, yPosition,cursorPositon.z);
                    //right
                }
                else if (wallNormal == new Vector3(-1, 0, 0))
                {
                    preGeneratedItem.transform.forward = Vector3.left;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x + 0.35f, yPosition, cursorPositon.z);
                    //left
                }
                else if (wallNormal == new Vector3(0, 0, -1))
                {
                    preGeneratedItem.transform.forward = Vector3.back;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x, yPosition, cursorPositon.z + 0.35f);
                    //backward
                }
                else if (wallNormal == new Vector3(0, 0, 1))
                {
                    preGeneratedItem.transform.forward = Vector3.forward;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x, yPosition, cursorPositon.z - 0.35f);
                    //forward
                }
                if(!isHighLightDisplayed)
                    EnableAllAvailableHighlightPart();

                if (!isCabietOnWallRight || !isCabinetOnWallLeft || !isCabinetOnWallTop || !isCabinetOnWallBottom || !isCabinetOnWallCenter)
                {
                    preGeneratedItem.GetComponentInChildren<QuikOutline>().OutlineColor = Color.red;
                }
                else if(!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                {
                    preGeneratedItem.GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if(isDragging)
                {
                    isDragging = false;
                    isCenterWallDetected = false;
                    if (!isCabietOnWallRight || !isCabinetOnWallLeft || !isCabinetOnWallTop || !isCabinetOnWallBottom || !isCabinetOnWallCenter)
                    {
                        Destroy(preGeneratedItem.gameObject);
                    }
                    else if(originPositon != null)
                    {
                        float halfXSizeDraggingCabinet = 0;
                        Vector3 newPosition = Vector3.zero;

                        if (offsetMultiplier == Vector3.right || offsetMultiplier == Vector3.left)
                        {
                            halfXSizeDraggingCabinet = preGeneratedItem.GetComponent<BoxCollider>().bounds.size.x / 2;
                            newPosition = (new Vector3(originPositon.position.x, preGeneratedItem.transform.position.y, preGeneratedItem.transform.position.z)) + (offsetMultiplier * halfXSizeDraggingCabinet);
                        }
                        if (offsetMultiplier == Vector3.forward || offsetMultiplier == Vector3.back)
                        {
                            halfXSizeDraggingCabinet = preGeneratedItem.GetComponent<BoxCollider>().bounds.size.z / 2;
                            newPosition = (new Vector3(preGeneratedItem.transform.position.x, preGeneratedItem.transform.position.y, originPositon.position.z)) + (offsetMultiplier * halfXSizeDraggingCabinet);
                        }
                        
                        preGeneratedItem.transform.position = newPosition;
                        preGeneratedItem.GetComponent<DraggableItem>().UpdateConnectedParts();
                    }
                    offsetMultiplier = Vector3.zero;
                    DisableAllAvailableHighlightPart();
                    Wall currentSelectedWall = DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>();
                    currentSelectedWall.wallCabinets.RemoveAll(obj => obj == null);
                    currentSelectedWall.wallCabinets.Add(preGeneratedItem);

                    preGeneratedItem.GetComponentInChildren<QuikOutline>().enabled = false;
                    if (preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                        Destroy(preGeneratedItem.gameObject);
                }
                originPositon = null;
            }
        }
    }

    private void EnableAllAvailableHighlightPart()
    {
        foreach (Transform child in RoomModelManager.Instance.CabinetDesign)
        {
            DraggableItem draggableItem = child.GetComponent<DraggableItem>();

            if(draggableItem.gameObject != preGeneratedItem)
            {   
                foreach (var item in draggableItem.NonConnectedParts)
                {
                    item.gameObject.SetActive(true);
                    item.GetComponent<Renderer>().material = item.transform.parent.GetComponent<DraggableItem>().YellowHighlightPart;
                }
            }            
        }
        isHighLightDisplayed = true;
    }

    private void DisableAllAvailableHighlightPart()
    {
        foreach (Transform child in RoomModelManager.Instance.CabinetDesign)
        {
            DraggableItem draggableItem = child.GetComponent<DraggableItem>();

            if (draggableItem)
            {
                foreach (var item in draggableItem.NonConnectedParts)
                {
                    item.gameObject.SetActive(false);
                }
                foreach (var item in draggableItem.ConnectedParts)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
        isHighLightDisplayed = false;
    }
}