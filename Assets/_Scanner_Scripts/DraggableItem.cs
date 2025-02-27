using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour
{
    public bool              isGroundCabinet;
    public bool                  isWallMount;
    public bool                   isOverHead;
    public bool                isWorksurface;
    public bool                 isBacksplash;
    public bool isCollidingWithOtherCabinets;

    public GameObject currentSelectedPart;

    [Space(15)]
    public List<GameObject> OthersCabinetsPart;
    public List<GameObject>  NonConnectedParts;
    public List<GameObject>     ConnectedParts;

    public GameObject[]      _40inWallMountHighlight;
    public GameObject[]  _20inLeftWallMountHighlight;
    public GameObject[] _20inRightWallMountHighlight;

    [Space(15)]
    public GameObject[] _40inOverheadHighlight;
    public GameObject[] _60inOverheadHighlight;
    public GameObject[] _80inOverheadhighlight;

    [Space(15)]
    public GameObject[] _20inWorksurfaceHighlight;
    public GameObject[] _40inWorksurfaceHighlight;
    public GameObject[] _60inWorksurfaceHighlight;
    public GameObject[] _80inWorksurfaceHighlight;
    public GameObject[] _40inSink;

    [Space(15)]
    public GameObject[] _40inBacksplash;
    public GameObject[] _60inBacksplash;
    public GameObject[] _80inBacksplash;

    [Space(15)]
    public Material YellowHighlightPart;
    public Material GreenHighlightPart;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!DoubleClickDetector.Instance.isWallOpen) return;

        GetComponentInChildren<QuikOutline>().enabled = true;
        GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;

        float currentTime = Time.time;

        if (currentTime - lastClickTime < doubleClickThreshold)
        {
            InputManager.Instance.preGeneratedItem = gameObject;
            InputManager.Instance.isDragging = true;
            InputManager.Instance.originPositon = null;
            UpdateConnections();
        }
        lastClickTime = currentTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(InputManager.Instance.preGeneratedItem == gameObject)
        {
            if (other.gameObject.tag.Equals("left") && !isCollidingWithOtherCabinets)
            {
                currentSelectedPart = other.gameObject;
                InputManager.Instance.originPositon = other.transform.GetChild(0).transform;

                if (InputManager.Instance.wallNormal == new Vector3(0, 0, -1))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.left;
                }
                if (InputManager.Instance.wallNormal == new Vector3(0, 0, 1))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.right;
                }
                if (InputManager.Instance.wallNormal == new Vector3(1, 0, 0))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.back;
                }
                if (InputManager.Instance.wallNormal == new Vector3(-1, 0, 0))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.forward;
                }
                if (InputManager.Instance.isCabietOnWallRight && InputManager.Instance.isCabinetOnWallLeft && InputManager.Instance.isCabinetOnWallBottom && InputManager.Instance.isCabinetOnWallTop && InputManager.Instance.isCabinetOnWallCenter && InputManager.Instance.isCabinetOnWallTopLeft && InputManager.Instance.isCabinetOnWallTopRight && InputManager.Instance.isCabinetOnWallBottomLeft && InputManager.Instance.isCabinetOnWallBottomRight)
                {
                    other.gameObject.GetComponent<Renderer>().material = GreenHighlightPart;
                    GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                }
            }
            if (other.gameObject.tag.Equals("right") && !isCollidingWithOtherCabinets)
            {
                currentSelectedPart = other.gameObject;
                InputManager.Instance.originPositon = other.transform.GetChild(0).transform;
                if (InputManager.Instance.wallNormal == new Vector3(0, 0, -1))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.right;
                }
                if (InputManager.Instance.wallNormal == new Vector3(0, 0, 1))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.left;
                }
                if (InputManager.Instance.wallNormal == new Vector3(1, 0, 0))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.forward;
                }
                if (InputManager.Instance.wallNormal == new Vector3(-1, 0, 0))
                {
                    InputManager.Instance.offsetMultiplier = Vector3.back;
                }
                if(InputManager.Instance.isCabietOnWallRight && InputManager.Instance.isCabinetOnWallLeft && InputManager.Instance.isCabinetOnWallBottom && InputManager.Instance.isCabinetOnWallTop && InputManager.Instance.isCabinetOnWallCenter && InputManager.Instance.isCabinetOnWallTopLeft && InputManager.Instance.isCabinetOnWallTopRight && InputManager.Instance.isCabinetOnWallBottomLeft && InputManager.Instance.isCabinetOnWallBottomRight)
                {
                    other.gameObject.GetComponent<Renderer>().material = GreenHighlightPart;
                    GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                }
            }
            if (other.gameObject.tag.Equals("wallmount") && !isCollidingWithOtherCabinets)
            {
                if (InputManager.Instance.isCabietOnWallRight && InputManager.Instance.isCabinetOnWallLeft && InputManager.Instance.isCabinetOnWallBottom && InputManager.Instance.isCabinetOnWallTop && InputManager.Instance.isCabinetOnWallCenter && InputManager.Instance.isCabinetOnWallTopLeft && InputManager.Instance.isCabinetOnWallTopRight && InputManager.Instance.isCabinetOnWallBottomLeft && InputManager.Instance.isCabinetOnWallBottomRight)
                {
                    currentSelectedPart = other.gameObject;
                    other.gameObject.GetComponent<Renderer>().material = GreenHighlightPart;
                    InputManager.Instance.originPositon = other.transform;
                    GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                }
            }
            if(other.gameObject.GetComponent<DraggableItem>())
            {
                isCollidingWithOtherCabinets = true;
                GetComponentInChildren<QuikOutline>().OutlineColor = Color.red;
            }
            if(other.gameObject.name.ToLower().Equals("window"))
            {
                isCollidingWithOtherCabinets = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("left") || other.gameObject.tag.Equals("right") || other.gameObject.tag.Equals("wallmount") || other.gameObject.tag.Equals("overheads"))
        {
            if (InputManager.Instance.preGeneratedItem == gameObject)
            {
                //Debug.Log("hrer");
                other.gameObject.GetComponent<Renderer>().material = YellowHighlightPart;
                GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
                //Debug.Log("oh yellow");
            }
        }
        if(other.gameObject.GetComponent<DraggableItem>())
        {
            isCollidingWithOtherCabinets = false;
            GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
            //Debug.Log("oh yellow");
        }
    }

    public void UpdateConnections()
    {
        UpdateAllWallMountHighlights();
        var uniqueItems = ConnectedParts.Except(NonConnectedParts).ToList();
        NonConnectedParts.AddRange(uniqueItems);
        ConnectedParts.Clear();

        for (int i = 0; i < OthersCabinetsPart.Count; i++)
        {
            GameObject tempPart = OthersCabinetsPart[i];

            try
            {
                if (OthersCabinetsPart[i].GetComponentInParent<DraggableItem>().ConnectedParts.Contains(OthersCabinetsPart[i]))
                    OthersCabinetsPart[i].GetComponentInParent<DraggableItem>().ConnectedParts.Remove(OthersCabinetsPart[i]);

                if (!OthersCabinetsPart[i].GetComponentInParent<DraggableItem>().NonConnectedParts.Contains(tempPart))
                    OthersCabinetsPart[i].GetComponentInParent<DraggableItem>().NonConnectedParts.Add(tempPart);

                OthersCabinetsPart[i].GetComponentInParent<DraggableItem>().OthersCabinetsPart.RemoveAll(item => NonConnectedParts.Contains(item));
            }
            catch(Exception)
            {

            }  
        }
        OthersCabinetsPart.Clear();
    }

    private void UpdateAllWallMountHighlights()
    {
        if (gameObject.name.Contains("WallMount40"))
        {
            EnableWallmountHighlight("_40wallmount");
        }
        if (gameObject.name.Contains("LeftWallMount20"))
        {
            EnableWallmountHighlight("_20leftwallmount");
        }
        if (gameObject.name.Contains("RightWallMount20"))
        {
            EnableWallmountHighlight("_20rightwallmount");
        }
    }


    public void UpdateConnectedParts()
    {
        UpdateConnections();

        if (currentSelectedPart != null)
        {
            if(currentSelectedPart.transform.parent.GetComponent<DraggableItem>().NonConnectedParts.Contains(currentSelectedPart))
                currentSelectedPart.transform.parent.GetComponent<DraggableItem>().NonConnectedParts.Remove(currentSelectedPart);

            if(!currentSelectedPart.transform.parent.GetComponent<DraggableItem>().ConnectedParts.Contains(currentSelectedPart))
                currentSelectedPart.transform.parent.GetComponent<DraggableItem>().ConnectedParts.Add(currentSelectedPart);

            if(!OthersCabinetsPart.Contains(currentSelectedPart))
                OthersCabinetsPart.Add(currentSelectedPart);

            if(currentSelectedPart.tag.Equals("left"))
            {
                RemoveGameObjectByTag("right", currentSelectedPart.transform.parent.GetComponent<DraggableItem>());
            }
            if(currentSelectedPart.tag.Equals("right"))
            {
                RemoveGameObjectByTag("left", currentSelectedPart.transform.parent.GetComponent<DraggableItem>());
            }
        }
        currentSelectedPart = null;
    }

    void RemoveGameObjectByTag(string tag, DraggableItem draggableItem)
    {
        for (int i = NonConnectedParts.Count - 1; i >= 0; i--)
        {
            if (NonConnectedParts[i].CompareTag(tag))
            {
                if(!ConnectedParts.Contains(NonConnectedParts[i]))
                    ConnectedParts.Add(NonConnectedParts[i]);

                if(!draggableItem.OthersCabinetsPart.Contains(NonConnectedParts[i]))
                    draggableItem.OthersCabinetsPart.Add(NonConnectedParts[i]);

                NonConnectedParts.RemoveAt(i);
            }
        }
    }

    public void EnableWallmountHighlight(string type)
    {
        switch (type)
        {
            case "_40wallmount":
                foreach (Transform cabinet in RoomModelManager.Instance.CabinetDesign)
                {
                    if(cabinet.gameObject != InputManager.Instance.preGeneratedItem)
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
                                        item.SetActive(true);
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
                    if (cabinet.gameObject != InputManager.Instance.preGeneratedItem)
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
                                        item.SetActive(true);
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
                    if (cabinet.gameObject != InputManager.Instance.preGeneratedItem)
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
                                        item.SetActive(true);
                                    }
                                }
                            }
                        }
                    }   
                }
                break;
        }
    }
}
