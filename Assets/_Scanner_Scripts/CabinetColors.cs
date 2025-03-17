using UnityEngine;
using UnityEngine.UI;

public class CabinetColors : MonoBehaviour
{
    public static CabinetColors Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Material   currentSelectedDoorColor;
    public Material currentSelectedHandelColor;

    [Header("Door Color Materials")]
    public Material           BlackMaterial;
    public Material      TrafficRedMaterial;
    public Material     TrafficGreyMaterial;
    public Material       PureWhiteMaterial;
    public Material      WindowGreyMaterial;
    public Material      ZincYellowMaterial;
    public Material      SignalBlueMaterial;
    public Material BrightRedOrangeMaterial;
    public Material         SkyBlueMaterial;
    public Material           GreenMaterial;

    [Header("Handle Color Material")]
    public Material           BlackHandleMat;
    public Material          SilverHandleMat;
    public Material      WindowGreyHandleMat;
    public Material       PureWhiteHandleMat;
    public Material      ZincYellowHandleMat;
    public Material      SignalBlueHandleMat;
    public Material BrightRedOrangeHandleMat;
    public Material         SkyBlueHandleMat;
    public Material           GreenHandleMat;
    public Material             RedHandleMat;
    public Material     TrafficGreyHandleMat;

    //Door and Drawers
    public void DoorBlackColor()
    {
        FindDoorDrawersChangeColor(BlackMaterial);
    }

    public void DoorTrafficRedColor()
    {
        FindDoorDrawersChangeColor(TrafficRedMaterial);
    }

    public void DoorTrafficGreyColor()
    {
        FindDoorDrawersChangeColor(TrafficGreyMaterial);
    }

    public void DoorPureWhiteColor()
    {
        FindDoorDrawersChangeColor(PureWhiteMaterial);
    }

    public void DoorWindowGreyColor()
    {
        FindDoorDrawersChangeColor(WindowGreyMaterial);
    }

    public void DoorZincYellow()
    {
        FindDoorDrawersChangeColor(ZincYellowMaterial);
    }

    public void DoorSignalBlue()
    {
        FindDoorDrawersChangeColor(SignalBlueMaterial);
    }

    public void DoorBrightRedOrange()
    {
        FindDoorDrawersChangeColor(BrightRedOrangeMaterial);
    }

    public void DoorSkyBlue()
    {
        FindDoorDrawersChangeColor(SkyBlueMaterial);
    }

    public void DoorGreen()
    {
        FindDoorDrawersChangeColor(GreenMaterial);
    }

    private void FindDoorDrawersChangeColor(Material color)
    {
        if(DoubleClickDetector.Instance.isWallOpen)
        {
            currentSelectedDoorColor = color;
            bool onlySelected        = false;

            foreach (Transform item in DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>().wallCabinets)
            {
                if (item.GetComponentInChildren<QuikOutline>().enabled)
                {
                    Debug.Log("find door drawers change colors");
                    onlySelected = true;
                    break;
                }
            }
            if(onlySelected)
            {
                foreach (Transform item in DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>().wallCabinets)
                {
                    if (item.GetComponentInChildren<QuikOutline>().enabled)
                    {
                        foreach (Transform child in item.GetComponentsInChildren<Transform>())
                        {
                            if (child.name == "Doors_Drawers")
                            {
                                Renderer renderer = child.GetComponent<Renderer>();
                                if (renderer != null)
                                {
                                    renderer.material = color;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Transform item in DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>().wallCabinets)
                {
                    foreach (Transform child in item.GetComponentsInChildren<Transform>())
                    {
                        if (child.name == "Doors_Drawers")
                        {
                            Renderer renderer = child.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = color;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            UIManager.Instance.NotificationmessagePanel.SetActive(true);
            UIManager.Instance.NotificationmessagePanel.GetComponentInChildren<Text>().text = "Please select a wall first.";
            UIManager.Instance.NotificationmessagePanel.GetComponent<Animator>().Play("NotificationPopINout", -1, 0);
        }
    }

    //Handle
    public void HandleBlackColor()
    {
        FindHandleChangeColor(BlackHandleMat);
    }

    public void HandleSilverColor()
    {
        FindHandleChangeColor(SilverHandleMat);
    }

    public void HandleWindowGreyColor()
    {
        FindHandleChangeColor(WindowGreyHandleMat);
    }

    public void HandlePureWhiteColor()
    {
        FindHandleChangeColor(PureWhiteHandleMat);
    }

    public void HandleZincYellowColor()
    {
        FindHandleChangeColor(ZincYellowHandleMat);
    }

    public void HandleSignalBlueColor()
    {
        FindHandleChangeColor(SignalBlueHandleMat);
    }

    public void HandleBrightRedOrangeColor()
    {
        FindHandleChangeColor(BrightRedOrangeHandleMat);
    }

    public void HandleSkyBlueColor()
    {
        FindHandleChangeColor(SkyBlueHandleMat);
    }

    public void HandleGreenColor()
    {
        FindHandleChangeColor(GreenHandleMat);
    }

    public void HandleRedColor()
    {
        FindHandleChangeColor(RedHandleMat);
    }

    public void HandleTrafficGreyColor()
    {
        FindHandleChangeColor(TrafficGreyHandleMat);
    }

    private void FindHandleChangeColor(Material color)
    {
        if(DoubleClickDetector.Instance.isWallOpen)
        {
            currentSelectedHandelColor = color;
            bool onlySelected = false;

            foreach (Transform item in DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>().wallCabinets)
            {
                if (item.GetComponentInChildren<QuikOutline>().enabled)
                {
                    onlySelected = true;
                    break;
                }
            }

            if(onlySelected)
            {
                foreach (Transform item in DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>().wallCabinets)
                {
                    if (item.GetComponentInChildren<QuikOutline>().enabled)
                    {
                        foreach (Transform child in item.GetComponentsInChildren<Transform>())
                        {
                            if (child.name == "Handles")
                            {
                                Renderer renderer = child.GetComponent<Renderer>();
                                if (renderer != null)
                                {
                                    renderer.material = color;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Transform item in DoubleClickDetector.Instance.selectedWall.GetComponent<Wall>().wallCabinets)
                {
                    foreach (Transform child in item.GetComponentsInChildren<Transform>())
                    {
                        if (child.name == "Handles") 
                        {
                            Renderer renderer = child.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = color;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            UIManager.Instance.NotificationmessagePanel.SetActive(true);
            UIManager.Instance.NotificationmessagePanel.GetComponentInChildren<Text>().text = "Please select a wall first.";
            UIManager.Instance.NotificationmessagePanel.GetComponent<Animator>().Play("NotificationPopINout", -1, 0);
        }
    }
}
