using UnityEngine;

public class ConnectedCheck : MonoBehaviour
{
    public bool isConnected;

    Vector3 centerOrigin = Vector3.zero;
    Vector3 leftOrigin = Vector3.zero;
    Vector3 rightOrigin = Vector3.zero;
    Vector3 topLeftOrigin = Vector3.zero;
    Vector3 topRightOrigin = Vector3.zero;

    bool isWsRightTouch;
    bool isWsLeftTouch;

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
                leftOrigin = centerOrigin - new Vector3(0, 0, bounds.extents.z - 0.01f);
                rightOrigin = centerOrigin + new Vector3(0, 0, bounds.extents.z - 0.01f);

                topLeftOrigin = leftOrigin + new Vector3(0, bounds.extents.y - 0.01f, 0);
                topRightOrigin = rightOrigin + new Vector3(0, bounds.extents.y - 0.01f, 0);
            }

            if (Mathf.Abs(dotForward - 1f) < 0.01f || Mathf.Abs(dotBack - 1f) < 0.01f)
            {
                leftOrigin = centerOrigin - new Vector3(bounds.extents.x - 0.01f, 0, 0);
                rightOrigin = centerOrigin + new Vector3(bounds.extents.x - 0.01f, 0, 0);

                topLeftOrigin = leftOrigin + new Vector3(0, bounds.extents.y - 0.01f, 0);
                topRightOrigin = rightOrigin + new Vector3(0, bounds.extents.y - 0.01f, 0);
            }

            Ray rayTopLeft = new Ray(topLeftOrigin, direction);
            Debug.DrawRay(rayTopLeft.origin, direction * 100, Color.green);

            Ray rayTopRight = new Ray(topRightOrigin, direction);
            Debug.DrawRay(rayTopRight.origin, direction * 100, Color.green);

            isWsLeftTouch = false;
            isWsRightTouch = false;

            if (Physics.Raycast(rayTopLeft, out RaycastHit hitInfoTopLeft))
            {
                if(gameObject.name.ToLower().Contains("worksurface"))
                {
                    bool isCurrentObject = false;
                    if (hitInfoTopLeft.collider.gameObject.name.ToLower().Contains("ws"))
                    {
                        if (hitInfoTopLeft.collider.gameObject == InputManager.Instance.preGeneratedItem)
                        {
                            isCurrentObject = true;
                        }
                    }

                    if (hitInfoTopLeft.collider.GetComponent<DraggableItem>() && (!hitInfoTopLeft.collider.gameObject.name.ToLower().Contains("ws") && !hitInfoTopLeft.collider.gameObject.name.ToLower().Contains("sink")) || isCurrentObject)// || hitInfoTopLeft.collider.GetComponent<QuikOutline>().OutlineColor == Color.green
                        isWsLeftTouch = true;
                }
                else
                {
                    if (hitInfoTopLeft.collider.GetComponent<DraggableItem>() && !hitInfoTopLeft.collider.gameObject.name.ToLower().Contains("ws"))
                        isWsLeftTouch = true;
                }
            }

            if (Physics.Raycast(rayTopRight, out RaycastHit hitInfoTopRight))
            {
                if(gameObject.name.ToLower().Contains("worksurface"))
                {
                    bool isCurrentObject = false;
                    if (hitInfoTopRight.collider.gameObject.name.ToLower().Contains("ws"))
                    {
                        if (hitInfoTopRight.collider.gameObject == InputManager.Instance.preGeneratedItem)
                        {
                            isCurrentObject = true;
                        }
                    }
                    if (hitInfoTopRight.collider.GetComponent<DraggableItem>() && (!hitInfoTopRight.collider.gameObject.name.ToLower().Contains("ws") && !hitInfoTopRight.collider.gameObject.name.ToLower().Contains("sink")) || isCurrentObject)// || hitInfoTopRight.collider.GetComponent<QuikOutline>().OutlineColor == Color.green
                        isWsRightTouch = true;
                }
                else
                {
                    if (hitInfoTopRight.collider.GetComponent<DraggableItem>() && !hitInfoTopRight.collider.gameObject.name.ToLower().Contains("ws"))
                        isWsRightTouch = true;
                }
            }
            gameObject.SetActive(isWsRightTouch && isWsLeftTouch);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.tag);
        if((gameObject.tag.Equals("overheads") || gameObject.tag.Equals("wallmount") || gameObject.tag.Equals("worksurface")) && InputManager.Instance.preGeneratedItem != other.gameObject)
        {
            if (other.GetComponent<DraggableItem>())
            {
                gameObject.SetActive(false);
            }
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
