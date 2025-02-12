using UnityEngine;
using UnityEngine.UI;

public class Worksurface : MonoBehaviour
{
    public GameObject stainlessSteelWorksurface;
    public GameObject powderCoatedWorksurface;
    public GameObject maplewoodWorksurface;

    public Toggle stainlessSteelToggle;
    public Toggle mapleToggle;
    public Toggle powderToggle;

    [Header("Stainless")]
    public GameObject stainlessSteel20;
    public GameObject stainlessSteel40;
    public GameObject stainlessSteel60;
    public GameObject stainlessSteel80;
    public GameObject stainlessSteelSink;

    [Header("Wood")]
    public GameObject mapleWood20;
    public GameObject mapleWood40;
    public GameObject mapleWood60;
    public GameObject mapleWood80;
    public GameObject mapleWoodSink;

    [Header("PowderCoated")]
    public GameObject powderCoated20;
    public GameObject powderCoated40;
    public GameObject powderCoated60;
    public GameObject powderCoated80;
    public GameObject powderCoatedSink;



    public void OpenStainlessSteelWS()
    {
        stainlessSteelWorksurface.SetActive(true);
        powderCoatedWorksurface.SetActive(false);
        maplewoodWorksurface.SetActive(false);
    }

    public void OpenPowderCoatedWS()
    {
        stainlessSteelWorksurface.SetActive(false);
        powderCoatedWorksurface.SetActive(true);
        maplewoodWorksurface.SetActive(false);
    }

    public void OpenMapleWoodWS()
    {
        stainlessSteelWorksurface.SetActive(false);
        powderCoatedWorksurface.SetActive(false);
        maplewoodWorksurface.SetActive(true);
    }

    private GameObject prefabToInstantiate;

    public void SelectWorksurface(string name)
    {
        if (DoubleClickDetector.Instance.isWallOpen)
        {
            switch (name)
            {
                case "Stainless20":
                    prefabToInstantiate = stainlessSteel20;
                    break;
                case "Stainless40":
                    prefabToInstantiate = stainlessSteel40;
                    break;
                case "Stainless60":
                    prefabToInstantiate = stainlessSteel60;
                    break;
                case "Stainless80":
                    prefabToInstantiate = stainlessSteel80;
                    break;
                case "StainlessSink":
                    prefabToInstantiate = stainlessSteelSink;
                    break;


                case "Maplewood20":
                    prefabToInstantiate = mapleWood20;
                    break;
                case "Maplewood40":
                    prefabToInstantiate = mapleWood40;
                    break;
                case "Maplewood60":
                    prefabToInstantiate = mapleWood60;
                    break;
                case "Maplewood80":
                    prefabToInstantiate = mapleWood80;
                    break;
                case "MaplewoodSink":
                    prefabToInstantiate = mapleWoodSink;
                    break;


                case "PowderCoated20":
                    prefabToInstantiate = powderCoated20;
                    break;
                case "PowderCoated40":
                    prefabToInstantiate = powderCoated40;
                    break;
                case "PowderCoated60":
                    prefabToInstantiate = powderCoated60;
                    break;
                case "PowderCoated80":
                    prefabToInstantiate = powderCoated80;
                    break;
                case "PowderCoatedSink":
                    prefabToInstantiate = powderCoatedSink;
                    break;
            }

            InputManager.Instance.preGeneratedItem = Instantiate(prefabToInstantiate, Vector3.zero, Quaternion.identity);
            InputManager.Instance.preGeneratedItem.transform.SetParent(RoomModelManager.Instance.CabinetDesign);
            InputManager.Instance.isDragging = true;

            //#if UNITY_EDITOR
            //            Selection.activeGameObject = InputManager.Instance.preGeneratedItem;
            //#endif
        }
        else
        {
            UIManager.Instance.NotificationmessagePanel.SetActive(true);
            UIManager.Instance.NotificationmessagePanel.GetComponentInChildren<Text>().text = "Please select a wall first.";
            UIManager.Instance.NotificationmessagePanel.GetComponent<Animator>().Play("NotificationPopINout", -1, 0);
        }
    }

    public void ApplyAllStainlessWs()
    {
        if(stainlessSteelToggle.isOn)
        {
            mapleToggle.isOn = false;
            powderToggle.isOn = false;
            Debug.Log("all stainlesssteel");

            foreach (Transform item in RoomModelManager.Instance.CabinetDesign)
            {
                if(item.gameObject.name.ToLower().Contains("maple") || item.gameObject.name.ToLower().Contains("powder"))
                {
                    string itemName = item.gameObject.name.Replace("(Clone)", "").Trim();
                    switch (itemName.ToLower())
                    {
                        case "maplewood20ws":
                        case "powdercoated20ws":
                            Replace(item.gameObject, stainlessSteel20);
                            break;
                        case "maplewood40ws":
                        case "powdercoated40ws":
                            Replace(item.gameObject, stainlessSteel40);
                            break;
                        case "maplewood60ws":
                        case "powdercoated60ws":
                            Replace(item.gameObject, stainlessSteel60);
                            break;
                        case "maplewood80ws":
                        case "powdercoated80ws":
                            Replace(item.gameObject, stainlessSteel80);
                            break;
                        case "maple_sink40":
                        case "powder_sink40":
                            Replace(item.gameObject, stainlessSteelSink);
                            break;
                    }
                }
            }
        }
    }

    public void ApplyAllMapleWs()
    {
        if(mapleToggle.isOn)
        {
            stainlessSteelToggle.isOn = false;
            powderToggle.isOn = false;
            Debug.Log("all maple");

            foreach (Transform item in RoomModelManager.Instance.CabinetDesign)
            {
                if (item.gameObject.name.ToLower().Contains("stainless") || item.gameObject.name.ToLower().Contains("powder"))
                {
                    string itemName = item.gameObject.name.Replace("(Clone)", "").Trim();
                    switch (itemName.ToLower())
                    {
                        case "stainless20ws":
                        case "powdercoated20ws":
                            Replace(item.gameObject, mapleWood20);
                            break;
                        case "stainless40ws":
                        case "powdercoated40ws":
                            Replace(item.gameObject, mapleWood40);
                            break;
                        case "stainless60ws":
                        case "powdercoated60ws":
                            Replace(item.gameObject, mapleWood60);
                            break;
                        case "stainless80ws":
                        case "powdercoated80ws":
                            Replace(item.gameObject, mapleWood80);
                            break;
                        case "stainless_sink40":
                        case "powder_sink40":
                            Replace(item.gameObject, mapleWoodSink);
                            break;
                    }
                }
            }
        }
    }

    public void ApplyAllPowderWs()
    {
        if(powderToggle.isOn)
        {
            stainlessSteelToggle.isOn = false;
            mapleToggle.isOn = false;
            Debug.Log("all powder");

            foreach (Transform item in RoomModelManager.Instance.CabinetDesign)
            {
                if (item.gameObject.name.ToLower().Contains("maple") || item.gameObject.name.ToLower().Contains("stainless"))
                {
                    string itemName = item.gameObject.name.Replace("(Clone)", "").Trim();
                    switch (itemName.ToLower())
                    {
                        case "maplewood20ws":
                        case "stainless20ws":
                            Replace(item.gameObject, powderCoated20);
                            break;
                        case "maplewood40ws":
                        case "stainless40ws":
                            Replace(item.gameObject, powderCoated40);
                            break;
                        case "maplewood60ws":
                        case "stainless60ws":
                            Replace(item.gameObject, powderCoated60);
                            break;
                        case "maplewood80ws":
                        case "stainless80ws":
                            Replace(item.gameObject, powderCoated80);
                            break;
                        case "maple_sink40":
                        case "stainless_sink40":
                            Replace(item.gameObject, powderCoatedSink);
                            break;
                    }
                }
            }
        }
    }

    private void Replace(GameObject before, GameObject afterPrefab)
    {
        if (before == null || afterPrefab == null)
        {
            Debug.LogError("Replace failed: One or both objects are null.");
            return;
        }

        Vector3 position = before.transform.position;
        Quaternion rotation = before.transform.rotation;

        GameObject afterInstance = Instantiate(afterPrefab, position, rotation);

        afterInstance.transform.parent = before.transform.parent;

        Destroy(before);
    }
}