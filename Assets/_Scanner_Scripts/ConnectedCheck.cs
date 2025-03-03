using UnityEngine;

public class ConnectedCheck : MonoBehaviour
{
    public bool isConnected;

    Vector3 centerOrigin =    Vector3.zero;
    Vector3 leftOrigin =      Vector3.zero;
    Vector3 rightOrigin =     Vector3.zero;
    Vector3 topLeftOrigin =   Vector3.zero;
    Vector3 topRightOrigin =  Vector3.zero;

    bool isWsRightTouchWs;
    bool isWsLeftTouchWs;

    bool isBsLeftTouchBs;
    bool isBsRightTouchBs;

    private void OnEnable()
    {
        if (gameObject.tag.Equals("worksurface") && DoubleClickDetector.Instance.isWallOpen)
        {
            Bounds bounds = GetComponent<MeshRenderer>().bounds;

            Vector3 direction = Vector3.down;
            centerOrigin = bounds.center;

            topLeftOrigin = centerOrigin + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
            topRightOrigin = centerOrigin + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);

            float dotRight = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.right);
            float dotLeft = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.left);
            float dotForward = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.forward);
            float dotBack = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.back);

            if (Mathf.Abs(dotRight - 1f) < 0.01f || Mathf.Abs(dotLeft - 1f) < 0.01f)
            {
                leftOrigin =  centerOrigin - new Vector3(0, 0, bounds.extents.z - 0.01f);
                rightOrigin = centerOrigin + new Vector3(0, 0, bounds.extents.z - 0.01f);

                topLeftOrigin =  leftOrigin + new  Vector3(0,  bounds.extents.y + 0.05f, 0);
                topRightOrigin = rightOrigin + new Vector3(0, bounds.extents.y + 0.05f, 0);
            }

            if (Mathf.Abs(dotForward - 1f) < 0.01f || Mathf.Abs(dotBack - 1f) < 0.01f)
            {
                leftOrigin =  centerOrigin - new Vector3(bounds.extents.x - 0.01f, 0, 0);
                rightOrigin = centerOrigin + new Vector3(bounds.extents.x - 0.01f, 0, 0);

                topLeftOrigin =  leftOrigin +  new Vector3(0, bounds.extents.y + 0.05f, 0);
                topRightOrigin = rightOrigin + new Vector3(0, bounds.extents.y + 0.05f, 0);
            }

            Ray rayTopLeftWs = new Ray(topLeftOrigin, direction);
            Debug.DrawRay(rayTopLeftWs.origin, direction * 100, Color.green);

            Ray rayTopRightWs = new Ray(topRightOrigin, direction);
            Debug.DrawRay(rayTopRightWs.origin, direction * 100, Color.green);

            isWsLeftTouchWs = false;
            isWsRightTouchWs = false;

            if (Physics.Raycast(rayTopLeftWs, out RaycastHit hitInfoTopLeftWs))
            {
                if (gameObject.name.ToLower().Contains("worksurface"))
                {
                    bool isCurrentObject = false;
                    if (hitInfoTopLeftWs.collider.gameObject.name.ToLower().Contains("ws"))
                    {
                        if (hitInfoTopLeftWs.collider.gameObject == InputManager.Instance.preGeneratedItem)
                        {
                            isCurrentObject = true;
                        }
                    }
                    if (hitInfoTopLeftWs.collider.GetComponent<DraggableItem>() && (!hitInfoTopLeftWs.collider.gameObject.name.ToLower().Contains("ws") && !hitInfoTopLeftWs.collider.gameObject.name.ToLower().Contains("sink")) || isCurrentObject)
                    {
                        isWsLeftTouchWs = true;
                    }
                }
                else
                {
                    if (hitInfoTopLeftWs.collider.GetComponent<DraggableItem>() && !hitInfoTopLeftWs.collider.gameObject.name.ToLower().Contains("ws"))
                    {
                        isWsLeftTouchWs = true;
                    }
                }
            }

            if (Physics.Raycast(rayTopRightWs, out RaycastHit hitInfoTopRightWs))
            {
                if (gameObject.name.ToLower().Contains("worksurface"))
                {
                    bool isCurrentObjectws = false;
                    if (hitInfoTopRightWs.collider.gameObject.name.ToLower().Contains("ws"))
                    {
                        if (hitInfoTopRightWs.collider.gameObject == InputManager.Instance.preGeneratedItem)
                        {
                            isCurrentObjectws = true;
                        }
                    }
                    if (hitInfoTopRightWs.collider.GetComponent<DraggableItem>() && (!hitInfoTopRightWs.collider.gameObject.name.ToLower().Contains("ws") && !hitInfoTopRightWs.collider.gameObject.name.ToLower().Contains("sink")) || isCurrentObjectws)
                    {
                        isWsRightTouchWs = true;
                    }
                }
                else
                {
                    if (hitInfoTopRightWs.collider.GetComponent<DraggableItem>() && !hitInfoTopRightWs.collider.gameObject.name.ToLower().Contains("ws"))
                    {
                        isWsRightTouchWs = true;
                    }
                }
            }
            gameObject.SetActive(isWsRightTouchWs && isWsLeftTouchWs);
        }

        if (gameObject.tag.Equals("backsplash") && DoubleClickDetector.Instance.isWallOpen)
        {
            Bounds bounds = GetComponent<MeshRenderer>().bounds;

            Vector3 direction = Vector3.down;
            centerOrigin = bounds.center;

            topLeftOrigin = centerOrigin + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
            topRightOrigin = centerOrigin + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);

            float dotRight = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.right);
            float dotLeft = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.left);
            float dotForward = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.forward);
            float dotBack = Vector3.Dot(InputManager.Instance.wallNormal.normalized, Vector3.back);

            float offset = 0;
            if(InputManager.Instance.wallNormal == Vector3.back || InputManager.Instance.wallNormal == Vector3.left)
            {
                offset = -0.03f;
            }
            if(InputManager.Instance.wallNormal == Vector3.forward || InputManager.Instance.wallNormal == Vector3.right)
            {
                offset = 0.03f;
            }

            if (Mathf.Abs(dotRight - 1f) < 0.01f || Mathf.Abs(dotLeft - 1f) < 0.01f)
            {
                leftOrigin = centerOrigin -  new Vector3(offset, 0, bounds.extents.z -  0.03f);
                rightOrigin = centerOrigin + new Vector3(-offset, 0, bounds.extents.z - 0.03f);

                topLeftOrigin =  leftOrigin +  new Vector3(0, bounds.extents.y - 0.05f, 0);
                topRightOrigin = rightOrigin + new Vector3(0, bounds.extents.y - 0.05f, 0);
            }

            if (Mathf.Abs(dotForward - 1f) < 0.01f || Mathf.Abs(dotBack - 1f) < 0.01f)
            {
                leftOrigin =  centerOrigin - new Vector3(bounds.extents.x - 0.03f, 0, offset);
                rightOrigin = centerOrigin + new Vector3(bounds.extents.x - 0.03f, 0, -offset);

                topLeftOrigin =   leftOrigin + new Vector3(0, bounds.extents.y - 0.05f, 0);
                topRightOrigin = rightOrigin + new Vector3(0, bounds.extents.y - 0.05f, 0);
            }

            Ray rayTopLeftBs = new Ray(topLeftOrigin, direction);
            Debug.DrawRay(rayTopLeftBs.origin, direction * 100, Color.green);

            Ray rayTopRightBs = new Ray(topRightOrigin, direction);
            Debug.DrawRay(rayTopRightBs.origin, direction * 100, Color.green);

            isBsLeftTouchBs  = false;
            isBsRightTouchBs = false;

            if (Physics.Raycast(rayTopLeftBs, out RaycastHit hitInfoTopLeftBs))
            {
                if (gameObject.name.ToLower().Contains("backsplash"))
                {
                    bool isCurrentObject = false;
                    if (hitInfoTopLeftBs.collider.gameObject.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoTopLeftBs.collider.gameObject == InputManager.Instance.preGeneratedItem)
                        {
                            isCurrentObject = true;
                        }
                    }

                    //Debug.Log(gameObject.name, gameObject);
                    //Debug.Log(hitInfoTopLeftBs.collider.gameObject, hitInfoTopLeftBs.collider.gameObject);
                    //Debug.Log(hitInfoTopLeftBs.collider.GetComponent<DraggableItem>() + " " + !hitInfoTopLeftBs.collider.gameObject.name.ToLower().Contains("bs") + " " + isCurrentObject);
                    if (hitInfoTopLeftBs.collider.GetComponent<DraggableItem>() && !hitInfoTopLeftBs.collider.gameObject.name.ToLower().Contains("bs") || isCurrentObject)
                    {
                        isBsLeftTouchBs = true;
                    }
                }
                else
                {
                    //Debug.Log(gameObject.name, gameObject);
                    //Debug.Log(hitInfoTopLeftBs.collider.gameObject, hitInfoTopLeftBs.collider.gameObject);
                    //Debug.Log(hitInfoTopLeftBs.collider.GetComponent<DraggableItem>() + " " + !hitInfoTopLeftBs.collider.gameObject.name.ToLower().Contains("bs"));
                    if (hitInfoTopLeftBs.collider.GetComponent<DraggableItem>() && !hitInfoTopLeftBs.collider.gameObject.name.ToLower().Contains("bs"))
                    {
                        isBsLeftTouchBs = true;
                    }
                }
            }

            if (Physics.Raycast(rayTopRightBs, out RaycastHit hitInfoTopRightBs))
            {
                if (gameObject.name.ToLower().Contains("backsplash"))
                {
                    bool isCurrentObject = false;
                    if (hitInfoTopRightBs.collider.gameObject.name.ToLower().Contains("bs"))
                    {
                        if (hitInfoTopRightBs.collider.gameObject == InputManager.Instance.preGeneratedItem)
                        {
                            isCurrentObject = true;
                        }
                    }
                    //Debug.Log(gameObject.name, gameObject);
                    //Debug.Log(hitInfoTopRightBs.collider.gameObject, hitInfoTopRightBs.collider.gameObject);
                    //Debug.Log(hitInfoTopRightBs.collider.GetComponent<DraggableItem>() + " " + !hitInfoTopRightBs.collider.gameObject.name.ToLower().Contains("bs") + " " + isCurrentObject);
                    if (hitInfoTopRightBs.collider.GetComponent<DraggableItem>() && !hitInfoTopRightBs.collider.gameObject.name.ToLower().Contains("bs") || isCurrentObject)
                    {
                        isBsRightTouchBs = true;
                    }
                }
                else
                {
                    //Debug.Log(gameObject.name, gameObject);
                    //Debug.Log(hitInfoTopRightBs.collider.gameObject, hitInfoTopRightBs.collider.gameObject);
                    //Debug.Log(hitInfoTopRightBs.collider.GetComponent<DraggableItem>() + " " + !hitInfoTopRightBs.collider.gameObject.name.ToLower().Contains("bs"));
                    if (hitInfoTopRightBs.collider.GetComponent<DraggableItem>() && !hitInfoTopRightBs.collider.gameObject.name.ToLower().Contains("bs"))
                    {
                        isBsRightTouchBs = true;
                    }
                }
            }
            //Debug.Log("right " + isBsLeftTouchBs + " left " + isBsRightTouchBs, gameObject);
            //gameObject.SetActive(isBsLeftTouchBs && isBsRightTouchBs);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DraggableItem>() && InputManager.Instance.preGeneratedItem != other.gameObject)
            gameObject.SetActive(false);

        if (gameObject.tag.Equals("overheads") && InputManager.Instance.preGeneratedItem != other.gameObject)
        {
            if(other.GetComponent<DraggableItem>() && other.gameObject.name.ToLower().Contains("overheads"))
            {
                gameObject.SetActive(false);
            }
        }

        if (gameObject.tag.Equals("wallmount") && InputManager.Instance.preGeneratedItem != other.gameObject)
        {
            if (other.GetComponent<DraggableItem>() && other.gameObject.name.ToLower().Contains("wallmount"))
            {
                gameObject.SetActive(false);
            }
        }

        //if (gameObject.tag.Equals("worksurface") && InputManager.Instance.preGeneratedItem != other.gameObject)
        //{
        //    if (other.GetComponent<DraggableItem>() && other.gameObject.name.ToLower().Contains("ws"))
        //    {
        //        gameObject.SetActive(false);
        //    }
        //}

        if (gameObject.tag.Equals("backsplash"))
        {
            //Debug.Log(other.gameObject.name.ToLower(), other.gameObject);
            //if (other.GetComponent<DraggableItem>() && (other.gameObject.name.ToLower().Contains("bs") || other.gameObject.tag.Equals("backsplash")))
            //{
                Debug.Log("make false", other.gameObject);
                gameObject.SetActive(false);
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GetComponent<Renderer>().material.name.Contains("Green") && !InputManager.Instance.isOutsideWall && !InputManager.Instance.preGeneratedItem.GetComponent<DraggableItem>().isCollidingWithOtherCabinets)
        {
            isConnected = true;
        }
        else
        {
            isConnected = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        isConnected = false;
        GetComponent<Renderer>().material = GetComponentInParent<DraggableItem>().YellowHighlightPart;
    }
}
