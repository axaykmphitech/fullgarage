using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager      Instance;
    public GameObject       preGeneratedItem;
    public bool           isDragging = false;
    public bool isCenterWallDetected = false;

    public Vector3       wallNormal;
    public Vector3    rayCastOffset;
    public Transform  originPositon;
    public Vector3 offsetMultiplier;

    [Space(15)]
    public bool      isCabinetOnWallCenter = false;
    public bool        isCabinetOnWallLeft = false;
    public bool        isCabietOnWallRight = false;
    public bool         isCabinetOnWallTop = false;
    public bool      isCabinetOnWallBottom = false;
    public bool     isCabinetOnWallTopLeft = false;
    public bool    isCabinetOnWallTopRight = false;
    public bool isCabinetOnWallBottomRight = false;
    public bool  isCabinetOnWallBottomLeft = false;
    public bool              isOutsideWall = false;
    public bool       isHighLightDisplayed = false;

    private Vector3           originalSize;
    private Vector3             shrinkSize;
    private BoxCollider preGenItemCollider;
    private Renderer      rendererForBound;
    [SerializeField] private float shrinkFactor = 0.9f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (DoubleClickDetector.Instance.isWallOpen)
        {
            Wall currentSelectedWall = DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>();

            if (Input.GetMouseButtonDown(0))
            {
                if (preGeneratedItem != null)
                {
                    QuikOutline outline = preGeneratedItem.GetComponentInChildren<QuikOutline>();
                    if(outline != null)
                    {
                        rendererForBound = outline.GetComponent<Renderer>();
                    }

                    preGenItemCollider      = preGeneratedItem.GetComponent<BoxCollider>();
                    originalSize            = preGenItemCollider.size;
                    shrinkSize              = originalSize * shrinkFactor;
                    preGenItemCollider.size = shrinkSize;
                }
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                isCabinetOnWallCenter      = false;
                isCabinetOnWallTop         = false;
                isCabietOnWallRight        = false;
                isCabinetOnWallBottom      = false;
                isCabinetOnWallLeft        = false;
                isCabinetOnWallTopLeft     = false;
                isCabinetOnWallTopRight    = false;
                isCabinetOnWallBottomLeft  = false;
                isCabinetOnWallBottomRight = false;

                float selectedWallZPositon = 0;
                Camera orthographicCamera;
                orthographicCamera = DoubleClickDetector.Instance.wallCameraObject.GetComponent<Camera>();

                Vector3 mouseWorldPosition = orthographicCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, orthographicCamera.nearClipPlane)
                );

                Vector3 cursorPositon = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y - (preGeneratedItem.transform.localScale.y / 2), mouseWorldPosition.z);

                if (preGeneratedItem && !isCenterWallDetected)
                {
                    preGeneratedItem.transform.position = cursorPositon;
                }

                QuikOutline outline = preGeneratedItem.GetComponent<QuikOutline>();
                if (outline == null)
                {
                    outline = preGeneratedItem.GetComponentInChildren<QuikOutline>();
                }
                if (outline != null)
                {
                    outline.enabled = true;
                }

                Vector3 direction = orthographicCamera.transform.forward;

                Vector3 centerOrigin      = Vector3.zero;
                Vector3 topOrigin         = Vector3.zero;
                Vector3 bottomOrigin      = Vector3.zero;
                Vector3 leftOrigin        = Vector3.zero;
                Vector3 rightOrigin       = Vector3.zero;
                Vector3 topLeftOrigin     = Vector3.zero;
                Vector3 topRightOrigin    = Vector3.zero;
                Vector3 bottomLeftOrigin  = Vector3.zero;
                Vector3 bottomRightOrigin = Vector3.zero;

                Bounds bounds = preGenItemCollider.bounds;
                centerOrigin  = bounds.center;
                topOrigin     = centerOrigin + new Vector3(0, bounds.extents.y, 0);
                bottomOrigin  = centerOrigin - new Vector3(0, bounds.extents.y, 0);

                topLeftOrigin     = centerOrigin + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
                topRightOrigin    = centerOrigin + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
                bottomLeftOrigin  = centerOrigin + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
                bottomRightOrigin = centerOrigin + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

                float dotRight   = Vector3.Dot(wallNormal.normalized, Vector3.right)  ;
                float dotLeft    = Vector3.Dot(wallNormal.normalized, Vector3.left)   ;
                float dotForward = Vector3.Dot(wallNormal.normalized, Vector3.forward);
                float dotBack    = Vector3.Dot(wallNormal.normalized, Vector3.back)   ;


                if (Mathf.Abs(dotRight - 1f) < 0.01f || Mathf.Abs(dotLeft - 1f) < 0.01f)
                {
                    leftOrigin  = centerOrigin - new Vector3(0, 0, bounds.extents.z);
                    rightOrigin = centerOrigin + new Vector3(0, 0, bounds.extents.z);

                    topLeftOrigin     = leftOrigin  + new Vector3(0, bounds.extents.y, 0);
                    bottomLeftOrigin  = leftOrigin  - new Vector3(0, bounds.extents.y, 0);
                    topRightOrigin    = rightOrigin + new Vector3(0, bounds.extents.y, 0);
                    bottomRightOrigin = rightOrigin - new Vector3(0, bounds.extents.y, 0);
                }

                if (Mathf.Abs(dotForward - 1f) < 0.01f || Mathf.Abs(dotBack - 1f) < 0.01f)
                {
                    leftOrigin  = centerOrigin - new Vector3(bounds.extents.x, 0, 0);
                    rightOrigin = centerOrigin + new Vector3(bounds.extents.x, 0, 0);

                    topLeftOrigin     = leftOrigin  + new Vector3(0, bounds.extents.y, 0);
                    bottomLeftOrigin  = leftOrigin  - new Vector3(0, bounds.extents.y, 0);
                    topRightOrigin    = rightOrigin + new Vector3(0, bounds.extents.y, 0);
                    bottomRightOrigin = rightOrigin - new Vector3(0, bounds.extents.y, 0);
                }

                //Ray to center
                Ray rayCenter   = new Ray(centerOrigin, direction);
                Debug.DrawRay(rayCenter.origin, direction * 100, Color.red);

                //Ray to top
                Ray rayTop      = new Ray(topOrigin, direction);
                Debug.DrawRay(rayTop.origin, direction * 100, Color.blue);

                //Ray to bottom
                Ray rayBottom   = new Ray(bottomOrigin, direction);
                Debug.DrawRay(rayBottom.origin, direction * 100, Color.magenta);

                //Ray to right
                Ray rayRight    = new Ray(rightOrigin, direction);
                Debug.DrawRay(rayRight.origin, direction * 100, Color.cyan);

                //Ray to left
                Ray rayLeft     = new Ray(leftOrigin, direction);
                Debug.DrawRay(rayLeft.origin, direction * 100, Color.green);

                Ray rayTopLeft  = new Ray(topLeftOrigin, direction);
                Debug.DrawRay(rayTopLeft.origin, direction * 100, Color.green);

                Ray rayTopRight = new Ray(topRightOrigin, direction);
                Debug.DrawRay(rayTopRight.origin, direction * 100, Color.green);

                Ray rayBottomLeft = new Ray(bottomLeftOrigin, direction);
                Debug.DrawRay(rayBottomLeft.origin, direction * 100, Color.green);

                Ray rayBottomRight = new Ray(bottomRightOrigin, direction);
                Debug.DrawRay(rayBottomRight.origin, direction * 100, Color.green);

                if (Physics.Raycast(rayCenter, out RaycastHit hitInfoCenter))
                {
                    if(preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoCenter.collider.name.Contains("wall") || hitInfoCenter.collider.name.ToLower().Contains("worksurface") || hitInfoCenter.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallCenter = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoCenter.collider.name.Contains("wall") || hitInfoCenter.collider.name.ToLower().Contains("overhead"))
                        {
                            wallNormal            = hitInfoCenter.normal;
                            selectedWallZPositon  = hitInfoCenter.collider.bounds.center.z;
                            isCenterWallDetected  = true;
                            isCabinetOnWallCenter = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoCenter.collider.name.Contains("wall") || hitInfoCenter.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallCenter = true;
                        }
                    }
                    else if(hitInfoCenter.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallCenter = true;
                    }
                }
                if (Physics.Raycast(rayTop, out RaycastHit hitInfoTop))
                {
                    //Debug.Log(hitInfoTop.collider.name, hitInfoTop.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoTop.collider.name.Contains("wall") || hitInfoTop.collider.name.ToLower().Contains("worksurface") || hitInfoTop.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallTop = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoTop.collider.name.Contains("wall") || hitInfoTop.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallTop = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoTop.collider.name.Contains("wall") || hitInfoTop.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallTop = true;
                        }
                    }
                    else if(hitInfoTop.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallTop = true;
                    }
                }
                if (Physics.Raycast(rayBottom, out RaycastHit hitInfoBottom))
                {
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoBottom.collider.name.Contains("wall") || hitInfoBottom.collider.name.ToLower().Contains("worksurface") || hitInfoBottom.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallBottom = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoBottom.collider.name.Contains("wall") || hitInfoBottom.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallBottom = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoBottom.collider.name.Contains("wall") || hitInfoBottom.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallBottom = true;
                        }
                    }
                    else if(hitInfoBottom.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallBottom = true;
                    }
                }
                if (Physics.Raycast(rayLeft, out RaycastHit hitInfoLeft))
                {
                    //Debug.Log(hitInfoLeft.collider.name, hitInfoLeft.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoLeft.collider.name.Contains("wall") || hitInfoLeft.collider.name.ToLower().Contains("worksurface") || hitInfoLeft.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallLeft = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoLeft.collider.name.Contains("wall") || hitInfoLeft.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallLeft = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoLeft.collider.name.Contains("wall") || hitInfoLeft.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallLeft = true;
                        }
                    }
                    else if(hitInfoLeft.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallLeft = true;
                    }
                }
                if (Physics.Raycast(rayRight, out RaycastHit hitInfoRight))
                {
                    //Debug.Log(hitInfoRight.collider.name, hitInfoRight.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoRight.collider.name.Contains("wall") || hitInfoRight.collider.name.ToLower().Contains("worksurface") || hitInfoRight.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabietOnWallRight = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoRight.collider.name.Contains("wall") || hitInfoRight.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabietOnWallRight = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoRight.collider.name.Contains("wall") || hitInfoRight.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabietOnWallRight = true;
                        }
                    }
                    else if(hitInfoRight.collider.name.Contains("wall"))
                    {
                        isCabietOnWallRight = true;
                    }
                }
                if (Physics.Raycast(rayTopLeft, out RaycastHit hitInfoTopLeft))
                {
                    //Debug.Log(hitInfoTopLeft.collider.name, hitInfoTopLeft.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoTopLeft.collider.name.Contains("wall") || hitInfoTopLeft.collider.name.ToLower().Contains("worksurface") || hitInfoTopLeft.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallTopLeft = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoTopLeft.collider.name.Contains("wall") || hitInfoTopLeft.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallTopLeft = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoTopLeft.collider.name.Contains("wall") || hitInfoTopLeft.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallTopLeft = true;
                        }
                    }
                    else if(hitInfoTopLeft.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallTopLeft = true;
                    }
                }
                if (Physics.Raycast(rayTopRight, out RaycastHit hitInfoTopRight))
                {
                    //Debug.Log(hitInfoTopRight.collider.name, hitInfoTopRight.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoTopRight.collider.name.Contains("wall") || hitInfoTopRight.collider.name.ToLower().Contains("worksurface") || hitInfoTopRight.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallTopRight = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoTopRight.collider.name.Contains("wall") || hitInfoTopRight.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallTopRight = true;
                        }
                    }
                    if(preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if(hitInfoTopRight.collider.name.Contains("wall") || hitInfoTopRight.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallTopRight = true;
                        }
                    }
                    else if(hitInfoTopRight.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallTopRight = true;
                    }
                }
                if (Physics.Raycast(rayBottomLeft, out RaycastHit hitInfoBottomLeft))
                {
                    //Debug.Log(hitInfoBottomLeft.collider.name, hitInfoBottomLeft.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoBottomLeft.collider.name.Contains("wall") || hitInfoBottomLeft.collider.name.ToLower().Contains("worksurface") || hitInfoBottomLeft.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallBottomLeft = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoBottomLeft.collider.name.Contains("wall") || hitInfoBottomLeft.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallBottomLeft = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoBottomLeft.collider.name.Contains("wall") || hitInfoBottomLeft.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallBottomLeft = true;
                        }
                    }
                    else if(hitInfoBottomLeft.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallBottomLeft = true;
                    }
                }
                if (Physics.Raycast(rayBottomRight, out RaycastHit hitInfoBottomRight))
                {
                    //Debug.Log(hitInfoBottomRight.collider.name, hitInfoBottomRight.collider.gameObject);
                    if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                    {
                        if (hitInfoBottomRight.collider.name.Contains("wall") || hitInfoBottomRight.collider.name.ToLower().Contains("worksurface") || hitInfoBottomRight.collider.name.ToLower().Contains("sink40"))
                        {
                            isCabinetOnWallBottomRight = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("overhead"))
                    {
                        if (hitInfoBottomRight.collider.name.Contains("wall") || hitInfoBottomRight.collider.name.ToLower().Contains("overhead"))
                        {
                            isCabinetOnWallBottomRight = true;
                        }
                    }
                    if (preGeneratedItem.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoBottomRight.collider.name.Contains("wall") || hitInfoBottomRight.collider.name.ToLower().Contains("backsplash"))
                        {
                            isCabinetOnWallBottomRight = true;
                        }
                    }
                    else if(hitInfoBottomRight.collider.name.Contains("wall"))
                    {
                        isCabinetOnWallBottomRight = true;
                    }
                }

                if (!isCabietOnWallRight || !isCabinetOnWallLeft || !isCabinetOnWallTop || !isCabinetOnWallBottom || !isCabinetOnWallCenter || !isCabinetOnWallTopLeft || !isCabinetOnWallTopRight || !isCabinetOnWallBottomLeft || !isCabinetOnWallBottomRight)
                {
                    isOutsideWall = true;
                    originPositon = null;
                }
                else
                {
                    isOutsideWall = false;
                }

                float yPosition = 0;
                if (preGeneratedItem.GetComponent<DraggableItem>().isGroundCabinet)
                    yPosition = RoomModelManager.Instance.floorPositionY;
                else if (preGeneratedItem.GetComponent<DraggableItem>().isWallMount)
                    yPosition = RoomModelManager.Instance.wallmountYPosition;
                else if (preGeneratedItem.GetComponent<DraggableItem>().isOverHead)
                    yPosition = RoomModelManager.Instance.overheadYPosition;
                else if (preGeneratedItem.GetComponent<DraggableItem>().isWorksurface)
                    yPosition = RoomModelManager.Instance.worksurfaceYPosition;
                else if (preGeneratedItem.GetComponent<DraggableItem>().isBacksplash)
                    yPosition = RoomModelManager.Instance.backsplashYPosition;

                if (wallNormal == new Vector3(1, 0, 0))
                {
                    preGeneratedItem.transform.forward  = Vector3.right;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x - 0.4f, yPosition, cursorPositon.z);
                    //right
                }
                else if (wallNormal == new Vector3(-1, 0, 0))
                {
                    preGeneratedItem.transform.forward  = Vector3.left;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x + 0.4f, yPosition, cursorPositon.z);
                    //left
                }
                else if (wallNormal == new Vector3(0, 0, -1))
                {
                    preGeneratedItem.transform.forward  = Vector3.back;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x, yPosition, cursorPositon.z + 0.4f);
                    //backward
                }
                else if (wallNormal == new Vector3(0, 0, 1))
                {
                    preGeneratedItem.transform.forward  = Vector3.forward;
                    preGeneratedItem.transform.position = new Vector3(cursorPositon.x, yPosition, cursorPositon.z - 0.4f);
                    //forward
                }
                if (!isHighLightDisplayed)
                    EnableAllAvailableHighlightPart();

                if (!isCabietOnWallRight || !isCabinetOnWallLeft || !isCabinetOnWallTop || !isCabinetOnWallBottom || !isCabinetOnWallCenter || !isCabinetOnWallTopLeft || !isCabinetOnWallTopRight || !isCabinetOnWallBottomLeft || !isCabinetOnWallBottomRight)
                {
                    outline = preGeneratedItem.GetComponent<QuikOutline>();
                    if (outline == null)
                    {
                        outline = preGeneratedItem.GetComponentInChildren<QuikOutline>();
                    }
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                    preGeneratedItem.GetComponentInChildren<QuikOutline>().OutlineColor = Color.red;
                    Debug.Log("2");
                }
                else if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets && originPositon == null)
                {
                    outline = preGeneratedItem.GetComponent<QuikOutline>();
                    if (outline == null)
                    {
                        outline = preGeneratedItem.GetComponentInChildren<QuikOutline>();
                    }
                    outline.OutlineColor = Color.yellow;
                }

                if (preGeneratedItem.name.ToLower().Contains("overheads"))
                {
                    Ray ray = new Ray(centerOrigin, direction);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag.Equals("overheads"))
                        {
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets && !isOutsideWall)
                            {
                                hit.collider.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().GreenHighlightPart;
                                preGeneratedItem.GetComponent<DraggableItem>().GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                                originPositon = hit.collider.transform;
                                preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart = hit.collider.gameObject;
                                UpdateHighlightOverheads(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets && !isOutsideWall)
                            {
                                preGeneratedItem.GetComponent<DraggableItem>().GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
                                UpdateHighlightOverheads(null);
                            }
                        }
                    }
                }

                if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
                {
                    Ray ray = new Ray(centerOrigin, direction);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.black);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag.Equals("worksurface"))
                        {
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                            {
                                hit.collider.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().GreenHighlightPart;
                                preGeneratedItem.GetComponent<DraggableItem>().GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                                originPositon = hit.collider.transform;
                                preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart = hit.collider.gameObject;
                                UpdateHighlightWorksurface(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                            {
                                preGeneratedItem.GetComponent<DraggableItem>().GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
                                UpdateHighlightWorksurface(null);
                            }
                        }
                    }
                }

                if (preGeneratedItem.name.ToLower().Contains("bs"))
                {
                    Ray ray = new Ray(centerOrigin, direction);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.black);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag.Equals("backsplash"))
                        {
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                            {
                                hit.collider.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().GreenHighlightPart;
                                preGeneratedItem.GetComponent<DraggableItem>().GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                                originPositon = hit.collider.transform;
                                preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart = hit.collider.gameObject;
                                UpdateHighlightBacksplashes(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                            {
                                preGeneratedItem.GetComponent<DraggableItem>().GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
                                UpdateHighlightBacksplashes(null);
                            }
                        }
                    }
                }
               
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    isDragging = false;
                    isCenterWallDetected = false;

                    if (preGeneratedItem != null)
                    {
                        preGenItemCollider.size = originalSize;
                    }
                    if (preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                    {
                        if ((preGeneratedItem.gameObject.name.ToLower().Contains("ws") || preGeneratedItem.gameObject.name.ToLower().Contains("sink40") || preGeneratedItem.gameObject.name.Contains("bs")) && preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart != null)
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = false;
                        Destroy(preGeneratedItem.gameObject);
                    }
                    if ((!isCabietOnWallRight || !isCabinetOnWallLeft || !isCabinetOnWallTop || !isCabinetOnWallBottom || !isCabinetOnWallCenter || !isCabinetOnWallTopLeft || !isCabinetOnWallTopRight || !isCabinetOnWallBottomLeft || !isCabinetOnWallBottomRight) && !preGeneratedItem.gameObject.name.ToLower().Contains("ws"))
                    {
                        try
                        {
                            if (preGeneratedItem != null)
                                preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = false;
                        }
                        catch(Exception)
                        {

                        }
                        Destroy(preGeneratedItem.gameObject);
                    }
                    if(originPositon == null && preGeneratedItem != null && (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40") || preGeneratedItem.name.ToLower().Contains("bs")))
                    {
                        try
                        {
                            if(preGeneratedItem != null)
                                preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = false;
                        }
                        catch (Exception)
                        {
                            
                        }
                        Destroy(preGeneratedItem.gameObject);
                    }
                    else if (originPositon != null && preGeneratedItem != null && !preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                    {
                        if (!originPositon.name.ToLower().Contains("wallmount") && !originPositon.name.ToLower().Contains("overhead") && !originPositon.name.ToLower().Contains("worksurface") && !originPositon.name.ToLower().Contains("sink40") && !originPositon.name.ToLower().Contains("backsplash") && !isOutsideWall)
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
                            if (!preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
                                preGeneratedItem.GetComponent<DraggableItem>().UpdateConnectedParts();
                        }
                        if (originPositon.name.ToLower().Contains("wallmount") && !isOutsideWall)
                        {
                            preGeneratedItem.transform.position = originPositon.GetChild(0).position;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = false;
                        }
                        if (originPositon.name.ToLower().Contains("overhead") && !isOutsideWall)
                        {
                            preGeneratedItem.transform.position = originPositon.GetChild(0).position;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = true;
                        }
                        if(originPositon.name.ToLower().Contains("worksurface") || originPositon.name.ToLower().Contains("sink40"))
                        {
                            preGeneratedItem.transform.position = originPositon.GetChild(0).position;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = false;
                        }
                        if (originPositon.name.ToLower().Contains("backsplash") && !isOutsideWall)
                        {
                            preGeneratedItem.transform.position = originPositon.GetChild(0).position;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                            preGeneratedItem.GetComponent<DraggableItem>().currentSelectedPart.GetComponent<ConnectedCheck>().isConnected = true;
                        }
                    }
                    offsetMultiplier = Vector3.zero;
                    DisableAllAvailableHighlightPart();

                    currentSelectedWall = DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>();
                    if(preGeneratedItem != null)
                        currentSelectedWall.wallCabinets.Add(preGeneratedItem.transform);

                    preGeneratedItem.GetComponentInChildren<QuikOutline>().enabled = false;
                    StartCoroutine(DisableOutlineAfterDelay(preGeneratedItem.GetComponentInChildren<QuikOutline>()));
                    DoubleClickDetector.Instance.FindExtreamObjects();

                    DistanceFromWall.Instance.DestroyAllFromCabinetToWallMeasurement();
                    DistanceFromWall.Instance.currentSelectedObject = null;
                    DistanceFromWall.Instance.tempObject = null;
                }
                originPositon = null;
                preGeneratedItem = null;
            }
            if (preGeneratedItem != null)
            {
                if (preGeneratedItem.GetComponentInChildren<QuikOutline>().OutlineColor == Color.yellow)
                {
                    originPositon = null;
                }
            }
            
            currentSelectedWall.wallCabinets.RemoveAll(obj => obj == null || obj.Equals(null));
        }

    }

    IEnumerator DisableOutlineAfterDelay(QuikOutline quikOutline)
    {
        yield return new WaitForSeconds(0.2f);
        if(quikOutline)
            quikOutline.enabled = true;
        yield return new WaitForSeconds(0.2f);
        if(quikOutline)
            quikOutline.enabled = false;
    }

    private void EnableAllAvailableHighlightPart()
    {
        if (!preGeneratedItem.name.ToLower().Contains("wallmount") && !preGeneratedItem.name.ToLower().Contains("overheads") && !preGeneratedItem.name.ToLower().Contains("ws") && !preGeneratedItem.name.ToLower().Contains("sink40") && !preGeneratedItem.name.ToLower().Contains("bs"))
        {
            foreach (Transform child in RoomModelManager.Instance.CabinetDesign)
            {
                DraggableItem draggableItem = child.GetComponent<DraggableItem>();

                if (draggableItem.gameObject != preGeneratedItem)
                {
                    foreach (var item in draggableItem.NonConnectedParts)
                    {
                        item.gameObject.SetActive(true);
                    }
                }
            }
        }
        if (preGeneratedItem.name.ToLower().Contains("wallmount"))
        {
            if (preGeneratedItem.name.Contains("WallMount40"))
            {
                EnableWallmountOverheadHighlight("_40wallmount");
            }
            if (preGeneratedItem.name.Contains("LeftWallMount20"))
            {
                EnableWallmountOverheadHighlight("_20leftwallmount");
            }
            if (preGeneratedItem.name.Contains("RightWallMount20"))
            {
                EnableWallmountOverheadHighlight("_20rightwallmount");
            }
        }

        if (preGeneratedItem.name.ToLower().Contains("overheads"))
        {
            if (preGeneratedItem.name.Contains("OverHeads40"))
            {
                EnableWallmountOverheadHighlight("_40overhead");
            }
            if (preGeneratedItem.name.Contains("OverHeads60"))
            {
                EnableWallmountOverheadHighlight("_60overhead");
            }
            if (preGeneratedItem.name.Contains("OverHeads80"))
            {
                EnableWallmountOverheadHighlight("_80overhead");
            }
        }
        if (preGeneratedItem.name.ToLower().Contains("ws") || preGeneratedItem.name.ToLower().Contains("sink40"))
        {
            if(preGeneratedItem.name.Contains("Stainless20"))
            {
                EnableWorksurfaceHighlight("_20WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Stainless40"))
            {
                EnableWorksurfaceHighlight("_40WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Stainless60"))
            {
                EnableWorksurfaceHighlight("_60WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Stainless80"))
            {
                EnableWorksurfaceHighlight("_80WorkSurface");
            }
            if(preGeneratedItem.name.Contains("Sink"))
            {
                EnableWorksurfaceHighlight("_40Sink");
            }


            if (preGeneratedItem.name.Contains("Maplewood20"))
            {
                EnableWorksurfaceHighlight("_20WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Maplewood40"))
            {
                EnableWorksurfaceHighlight("_40WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Maplewood60"))
            {
                EnableWorksurfaceHighlight("_60WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Maplewood80"))
            {
                EnableWorksurfaceHighlight("_80WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Sink"))
            {
                EnableWorksurfaceHighlight("_40Sink");
            }


            if (preGeneratedItem.name.Contains("PowderCoated20"))
            {
                EnableWorksurfaceHighlight("_20WorkSurface");
            }
            if (preGeneratedItem.name.Contains("PowderCoated40"))
            {
                EnableWorksurfaceHighlight("_40WorkSurface");
            }
            if (preGeneratedItem.name.Contains("PowderCoated60"))
            {
                EnableWorksurfaceHighlight("_60WorkSurface");
            }
            if (preGeneratedItem.name.Contains("PowderCoated80"))
            {
                EnableWorksurfaceHighlight("_80WorkSurface");
            }
            if (preGeneratedItem.name.Contains("Sink"))
            {
                EnableWorksurfaceHighlight("_40Sink");
            }
        }

        if (preGeneratedItem.name.ToLower().Contains("bs"))
        {
            if(preGeneratedItem.name.Contains("Aluminium40"))
            {
                EnableBacksplashHighlight("_40Backsplash");
            }
            if (preGeneratedItem.name.Contains("Aluminium60"))
            {
                EnableBacksplashHighlight("_60Backsplash");
            }
            if (preGeneratedItem.name.Contains("Aluminium80"))
            {
                EnableBacksplashHighlight("_80Backsplash");
            }
            if (preGeneratedItem.name.Contains("Procore40"))
            {
                EnableBacksplashHighlight("_40Backsplash");
            }
            if (preGeneratedItem.name.Contains("Procore60"))
            {
                EnableBacksplashHighlight("_60Backsplash");
            }
            if (preGeneratedItem.name.Contains("Procore80"))
            {
                EnableBacksplashHighlight("_80Backsplash");
            }
        }
        isHighLightDisplayed = true;
    }

    public void EnableWallmountOverheadHighlight(string type)
    {
        switch (type)
        {
            case "_40wallmount":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._40inWallMountHighlight != null)
                            {
                                if (draggableItem._40inWallMountHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._40inWallMountHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_20leftwallmount":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._20inLeftWallMountHighlight != null)
                            {
                                if (draggableItem._20inLeftWallMountHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._20inLeftWallMountHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_20rightwallmount":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._20inRightWallMountHighlight != null)
                            {
                                if (draggableItem._20inRightWallMountHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._20inRightWallMountHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_40overhead":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {   
                            if (draggableItem._40inOverheadHighlight != null)
                            {
                                if (draggableItem._40inOverheadHighlight.Length > 0)
                                {   
                                    foreach (var item in draggableItem._40inOverheadHighlight)
                                    {   
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_60overhead":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._60inOverheadHighlight != null)
                            {
                                if (draggableItem._60inOverheadHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._60inOverheadHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_80overhead":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();
                        if (draggableItem != null)
                        {
                            if (draggableItem._80inOverheadhighlight != null)
                            {
                                if (draggableItem._80inOverheadhighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._80inOverheadhighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;

        }
    }

    public void EnableWorksurfaceHighlight(string type)
    {
        switch (type)
        {
            case "_20WorkSurface":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._20inWorksurfaceHighlight != null)
                            {
                                if (draggableItem._20inWorksurfaceHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._20inWorksurfaceHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case "_40WorkSurface":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._40inWorksurfaceHighlight != null)
                            {
                                if (draggableItem._40inWorksurfaceHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._40inWorksurfaceHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case "_60WorkSurface":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._60inWorksurfaceHighlight != null)
                            {
                                if (draggableItem._60inWorksurfaceHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._60inWorksurfaceHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case "_80WorkSurface":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._80inWorksurfaceHighlight != null)
                            {
                                if (draggableItem._80inWorksurfaceHighlight.Length > 0)
                                {
                                    foreach (var item in draggableItem._80inWorksurfaceHighlight)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case "_40Sink":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._40inSink != null)
                            {
                                if (draggableItem._40inSink.Length > 0)
                                {
                                    foreach (var item in draggableItem._40inSink)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
        }
    }

    public void EnableBacksplashHighlight(string type)
    {
        switch (type)
        {
            case "_40Backsplash":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._40inBacksplash != null)
                            {
                                if (draggableItem._40inBacksplash.Length > 0)
                                {
                                    foreach (var item in draggableItem._40inBacksplash)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_60Backsplash":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)  
                        {
                            if (draggableItem._60inBacksplash != null)
                            {
                                if (draggableItem._60inBacksplash.Length > 0)
                                {
                                    foreach (var item in draggableItem._60inBacksplash)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "_80Backsplash":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if (cabinet.gameObject != preGeneratedItem)
                    {
                        DraggableItem draggableItem = cabinet.GetComponent<DraggableItem>();

                        if (draggableItem != null)
                        {
                            if (draggableItem._80inBacksplash != null)
                            {
                                if (draggableItem._80inBacksplash.Length > 0)
                                {
                                    foreach (var item in draggableItem._80inBacksplash)
                                    {
                                        if (!item.GetComponent<ConnectedCheck>().isConnected)
                                        {
                                            item.SetActive(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
        }
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

            if (draggableItem != null)
            {
                if (draggableItem._40inWallMountHighlight != null)
                {
                    if (draggableItem._40inWallMountHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._40inWallMountHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._20inLeftWallMountHighlight != null)
                {
                    if (draggableItem._20inLeftWallMountHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._20inLeftWallMountHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._20inRightWallMountHighlight != null)
                {
                    if (draggableItem._20inRightWallMountHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._20inRightWallMountHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._40inOverheadHighlight != null)
                {
                    if (draggableItem._40inOverheadHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._40inOverheadHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._60inOverheadHighlight != null)
                {
                    if (draggableItem._60inOverheadHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._60inOverheadHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._80inOverheadhighlight != null)
                {
                    if (draggableItem._80inOverheadhighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._80inOverheadhighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._20inWorksurfaceHighlight != null)
                {
                    if (draggableItem._20inWorksurfaceHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._20inWorksurfaceHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._40inWorksurfaceHighlight != null)
                {
                    if (draggableItem._40inWorksurfaceHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._40inWorksurfaceHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._60inWorksurfaceHighlight != null)
                {
                    if (draggableItem._60inWorksurfaceHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._60inWorksurfaceHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._80inWorksurfaceHighlight != null)
                {
                    if (draggableItem._80inWorksurfaceHighlight.Length > 0)
                    {
                        foreach (var item in draggableItem._80inWorksurfaceHighlight)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._40inSink != null)
                {
                    if (draggableItem._40inSink.Length > 0)
                    {
                        foreach (var item in draggableItem._40inSink)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._40inBacksplash != null)
                {
                    if (draggableItem._40inBacksplash.Length > 0)
                    {
                        foreach (var item in draggableItem._40inBacksplash)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._60inBacksplash != null)
                {
                    if (draggableItem._60inBacksplash.Length > 0)
                    {
                        foreach (var item in draggableItem._60inBacksplash)
                        {
                            item.SetActive(false);
                        }
                    }
                }

                if (draggableItem._80inBacksplash != null)
                {
                    if (draggableItem._80inBacksplash.Length > 0)
                    {
                        foreach (var item in draggableItem._80inBacksplash)
                        {
                            item.SetActive(false);
                        }
                    }
                }
            }
        }
        isHighLightDisplayed = false;
    }

    private void UpdateHighlightOverheads(GameObject currectSelectedPart)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.CompareTag("overheads") && obj.scene.IsValid())
            {
                if (obj != currectSelectedPart)
                {
                    obj.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                    obj.GetComponent<Renderer>().sortingOrder = 5;
                    obj.GetComponent<ConnectedCheck>().isConnected = false;

                    Vector3 newScale = obj.transform.localScale;
                    newScale.y = 0.15f; 
                    obj.transform.localScale = newScale;
                }
                else
                {
                    obj.GetComponent<Renderer>().sortingOrder = 10;
                    obj.GetComponent<ConnectedCheck>().isConnected = true;

                    Vector3 newScale = obj.transform.localScale;
                    newScale.y = 0.175f; 
                    obj.transform.localScale = newScale;
                }
            }
        }
    }

    private void UpdateHighlightWorksurface(GameObject currectSelectedPart)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.CompareTag("worksurface") && obj.scene.IsValid())
            {
                if (obj != currectSelectedPart)
                {
                    obj.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                    obj.GetComponent<Renderer>().sortingOrder = 5;
                    obj.GetComponent<ConnectedCheck>().isConnected = false;
                }
                else
                {
                    obj.GetComponent<Renderer>().sortingOrder = 10;
                    obj.GetComponent<ConnectedCheck>().isConnected = true;
                }
            }
        }
    }

    private void UpdateHighlightBacksplashes(GameObject currectSelectedPart)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.CompareTag("backsplash") && obj.scene.IsValid())
            {
                if (obj != currectSelectedPart)
                {
                    obj.GetComponent<Renderer>().material = preGeneratedItem.GetComponent<DraggableItem>().YellowHighlightPart;
                    obj.GetComponent<Renderer>().sortingOrder = 5;
                    obj.GetComponent<ConnectedCheck>().isConnected = false;
                }
                else
                {
                    obj.GetComponent<Renderer>().sortingOrder = 10;
                    obj.GetComponent<ConnectedCheck>().isConnected = true;
                }
            }
        }
    }
}









