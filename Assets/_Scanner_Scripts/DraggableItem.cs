using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    public bool isGroundCabinet;
    public bool isWallMount;
    public bool isOverHead;
    public bool isCollidingWithOtherCabinets;

    public GameObject currentSelectedPart;

    [Space(15)]
    public List<GameObject> OthersCabinetsPart;
    public List<GameObject> NonConnectedParts;
    public List<GameObject> ConnectedParts;

    public Material YellowHighlightPart;
    public Material GreenHighlightPart;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    //private void OnMouseDown()
    //{
    //    if (DoubleClickDetector.Instance.isWallOpen)
    //    {
    //        InputManager.Instance.preGeneratedItem = gameObject;
    //        InputManager.Instance.isDragging = true;
    //        UpdateConnections();
    //    }
    //}

    private void OnMouseDown()
    {
        if (!DoubleClickDetector.Instance.isWallOpen) return;

        float currentTime = Time.time;

        if (currentTime - lastClickTime < doubleClickThreshold)
        {
            InputManager.Instance.preGeneratedItem = gameObject;
            InputManager.Instance.isDragging = true;
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
                if (InputManager.Instance.isCabietOnWallRight && InputManager.Instance.isCabinetOnWallLeft && InputManager.Instance.isCabinetOnWallBottom && InputManager.Instance.isCabinetOnWallTop && InputManager.Instance.isCabinetOnWallCenter)
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
                if(InputManager.Instance.isCabietOnWallRight && InputManager.Instance.isCabinetOnWallLeft && InputManager.Instance.isCabinetOnWallBottom && InputManager.Instance.isCabinetOnWallTop && InputManager.Instance.isCabinetOnWallCenter)
                {
                    other.gameObject.GetComponent<Renderer>().material = GreenHighlightPart;
                    GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                }
            }
            if (other.gameObject.tag.Equals("wallmount") && !isCollidingWithOtherCabinets)
            {
                Debug.Log("wallmount position detect");
                if (InputManager.Instance.isCabietOnWallRight && InputManager.Instance.isCabinetOnWallLeft && InputManager.Instance.isCabinetOnWallBottom && InputManager.Instance.isCabinetOnWallTop && InputManager.Instance.isCabinetOnWallCenter)
                {
                    other.gameObject.GetComponent<Renderer>().material = GreenHighlightPart;
                    GetComponentInChildren<QuikOutline>().OutlineColor = Color.green;
                }
            }
            if(other.gameObject.GetComponent<DraggableItem>())
            {
                isCollidingWithOtherCabinets = true;
                GetComponentInChildren<QuikOutline>().OutlineColor = Color.red;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("left") || other.gameObject.tag.Equals("right") || other.gameObject.tag.Equals("wallmount"))
        {
            if (InputManager.Instance.preGeneratedItem = gameObject)
            {
                InputManager.Instance.originPositon = null;

                other.gameObject.GetComponent<Renderer>().material = YellowHighlightPart;
                GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
            }
        }
        if(other.gameObject.GetComponent<DraggableItem>())
        {
            isCollidingWithOtherCabinets = false;
            GetComponentInChildren<QuikOutline>().OutlineColor = Color.yellow;
        }
    }

    public void UpdateConnections()
    {
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

            if (currentSelectedPart.tag.Equals("left"))
            {
                RemoveGameObjectByTag("right", currentSelectedPart.transform.parent.GetComponent<DraggableItem>());
            }
            if (currentSelectedPart.tag.Equals("right"))
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
}
