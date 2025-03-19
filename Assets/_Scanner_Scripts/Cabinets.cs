using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cabinets : MonoBehaviour
{
    [Header("UIPanel")]
    public GameObject   _40ModualsPanel;
    public GameObject   _20ModualsPanel;
    public GameObject    OverheadsPanel;
    public GameObject ApplicationsPanel;

    [Header("40")]
    public GameObject  _3DrawerBase;
    public GameObject  _5DrawerBase;
    public GameObject _10DrawerBase;
    public GameObject    _2DoorBase;
    public GameObject  Tallcabinets;
    public GameObject      SinkBase;
    public GameObject     Wallmount;

    [Header("20")]
    public GameObject   DoorBaseLeft;
    public GameObject  DoorBaseRight;
    public GameObject     LockerLeft;
    public GameObject    LockerRight;
    public GameObject  WallmountLeft;
    public GameObject WallmountRight;

    [Header("Overheads")]
    public GameObject _40InchOverhead;
    public GameObject _60InchOverhead;
    public GameObject _80InchOverhead;

    [Header("Corner Module")]
    public GameObject CornerBaseLeft;
    public GameObject CornerBaseRight;
    public GameObject CornerWallmountRight;
    public GameObject CornerWallmountLeft;

    [Header("Applications")]
    public GameObject BeverageCenter;
    public GameObject        Freezer;
    public GameObject         Fridge;
    public GameObject         Pantry;

    private GameObject prefabToInstantiate;

    public void Select40Moduals()
    {
        _40ModualsPanel.SetActive(true);
        _20ModualsPanel.SetActive(false);
        OverheadsPanel.SetActive(false);
        ApplicationsPanel.SetActive(false);
    }

    public void Select20Moduals()
    {
        _40ModualsPanel.SetActive(false);
        _20ModualsPanel.SetActive(true);
        OverheadsPanel   .SetActive(false);
        ApplicationsPanel.SetActive(false);
    }

    public void SelectOverheads()
    {
        _40ModualsPanel  .SetActive(false);
        _20ModualsPanel  .SetActive(false);
        OverheadsPanel    .SetActive(true);
        ApplicationsPanel.SetActive(false);
    }

    public void SelectApplications()
    {
        _40ModualsPanel  .SetActive(false);
        _20ModualsPanel  .SetActive(false);
        OverheadsPanel   .SetActive(false);
        ApplicationsPanel .SetActive(true);
    }

    public void SelectCabinet(string name)
    {
        if(DoubleClickDetector.Instance.isWallOpen)
        {
            switch (name)
            {
                case "Drawer3Base40":
                    prefabToInstantiate = _3DrawerBase;
                    break;
                case "Drawer5Base40":
                    prefabToInstantiate = _5DrawerBase;
                    break;
                case "Drawer10Base40":
                    prefabToInstantiate = _10DrawerBase;
                    break;
                case "Door2Base":
                    prefabToInstantiate = _2DoorBase;
                    break;
                case "TallCabinet":
                    prefabToInstantiate = Tallcabinets;
                    break;
                case "SinkBase":
                    prefabToInstantiate = SinkBase;
                    break;
                case "WallMount40":
                    prefabToInstantiate = Wallmount;
                    break;


                case "LeftDoor1Base":
                    prefabToInstantiate = DoorBaseLeft;
                    break;
                case "RightDoor1Base":
                    prefabToInstantiate = DoorBaseRight;
                    break;
                case "LeftLocker":
                    prefabToInstantiate = LockerLeft;
                    break;
                case "RightLocker":
                    prefabToInstantiate = LockerRight;
                    break;
                case "LeftWallMount20":
                    prefabToInstantiate = WallmountLeft;
                    break;
                case "RightWallMount20":
                    prefabToInstantiate = WallmountRight;
                    break;


                case "OverHeads40":
                    prefabToInstantiate = _40InchOverhead;
                    break;
                case "OverHeads60":
                    prefabToInstantiate = _60InchOverhead;
                    break;
                case "OverHeads80":
                    prefabToInstantiate = _80InchOverhead;
                    break;


                case "BeverageCenter":
                    prefabToInstantiate = BeverageCenter;
                    break;
                case "Freezer":
                    prefabToInstantiate = Freezer;
                    break;
                case "Fridge":
                    prefabToInstantiate = Fridge;
                    break;
                case "Pantry":
                    prefabToInstantiate = Pantry;
                    break;


                case "CornerBaseLeft":
                    prefabToInstantiate = CornerBaseLeft;
                    break;
                case "CornerBaseRight":
                    prefabToInstantiate = CornerBaseRight;
                    break;
                case "CornerWallmountRight":
                    prefabToInstantiate = CornerWallmountRight;
                    break;
                case "CornerWallmountLeft":
                    prefabToInstantiate = CornerWallmountLeft;
                    break;
            }


            GameObject cabinet;
            InputManager.Instance.preGeneratedItem = cabinet = Instantiate(prefabToInstantiate, Vector3.zero, Quaternion.identity);

            if(prefabToInstantiate == CornerBaseLeft)
            {
                Debug.Log("left cab");
            }
            if(prefabToInstantiate == CornerBaseRight)
            {
                Debug.Log("right cab");
                InputManager.Instance.preGeneratedItem.transform.eulerAngles = new Vector3(0, 0, 0);
                cabinet.transform.eulerAngles = new Vector3(0, 0, 0);
            }

            Transform doorDrawers   =   cabinet.transform.Find("Doors_Drawers");
            Transform handleDrawers =   cabinet.transform.Find("Handle");

            if (doorDrawers != null)
            {
                if(CabinetColors.Instance != null)
                {
                    if (CabinetColors.Instance.currentSelectedDoorColor != null)
                        doorDrawers.GetComponent<Renderer>().material = CabinetColors.Instance.currentSelectedDoorColor;
                }
            }
            if(handleDrawers != null)
            {
                if(CabinetColors.Instance != null)
                {
                    if (CabinetColors.Instance.currentSelectedHandelColor != null)
                        handleDrawers.GetComponent<Renderer>().material = CabinetColors.Instance.currentSelectedHandelColor;
                }
            }

            InputManager.Instance.preGeneratedItem.transform.SetParent(RoomModelManager.Instance.CabinetDesign);
            InputManager.Instance.isDragging = true;

#if UNITY_EDITOR
            Selection.activeGameObject = InputManager.Instance.preGeneratedItem;
#endif

        }
        else
        {
            UIManager.Instance.NotificationmessagePanel.SetActive(true);
            UIManager.Instance.NotificationmessagePanel.GetComponentInChildren<Text>().text = "Please select a wall first.";
            UIManager.Instance.NotificationmessagePanel.GetComponent<Animator>().Play("NotificationPopINout", -1, 0);
        }
    }
}
