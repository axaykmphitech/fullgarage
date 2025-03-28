using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Backsplashes : MonoBehaviour
{
    [Header("Aluminium")]
    public GameObject Aluminium40;
    public GameObject Aluminium60;
    public GameObject Aluminium80;
    public GameObject Aluminium22right;
    public GameObject Aluminium22left ;

    [Header("Procore")]
    public GameObject Procore40;
    public GameObject Procore60;
    public GameObject Procore80;
    public GameObject Procore22right;
    public GameObject Procore22left ;

    private GameObject prefabToInstantiate;

    public Toggle aluminiumToggle;
    public Toggle   procoreToggle;

    public void SelectBacksplash(string name)
    {
        if (DoubleClickDetector.Instance.isWallOpen)
        {
            switch (name)
            {
                case "Aluminium40":
                    prefabToInstantiate = Aluminium40;
                    break;
                case "Aluminium60":
                    prefabToInstantiate = Aluminium60;
                    break;
                case "Aluminium80":
                    prefabToInstantiate = Aluminium80;
                    break;
                case "Aluminium22right":
                    prefabToInstantiate = Aluminium22right;
                    break;
                case "Aluminium22left":
                    prefabToInstantiate = Aluminium22left;
                    break;

                case "Procore40":
                    prefabToInstantiate = Procore40;
                    break;
                case "Procore60":
                    prefabToInstantiate = Procore60;
                    break;
                case "Procore80":
                    prefabToInstantiate = Procore80;
                    break;
                case "Procore22right":
                    prefabToInstantiate = Procore22right;
                    break;
                case "Procore22left":
                    prefabToInstantiate = Procore22left;
                    break;
            }
            
            InputManager.Instance.preGeneratedItem = Instantiate(prefabToInstantiate, Vector3.zero, Quaternion.identity);
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

    public void ApplyAllProcore()
    {
        if (procoreToggle.isOn)
        {
            aluminiumToggle.isOn = false;

            foreach (Transform item in RoomModelManager.Instance.CabinetDesign)
            {
                if (item.gameObject.name.ToLower().Contains("aluminium"))
                {
                    string itemName = item.gameObject.name.Replace("(Clone)", "").Trim();
                    switch (itemName.ToLower())
                    {
                        case "aluminium40bs":
                            Replace(item.gameObject, Procore40);    
                            break;
                        case "aluminium60bs":
                            Replace(item.gameObject, Procore60);
                            break;
                        case "aluminium80bs":
                            Replace(item.gameObject, Procore80);
                            break;
                        case "aluminium22bsright":
                            Replace(item.gameObject, Procore22right);
                            break;
                        case "aluminium22bsleft":
                            Replace(item.gameObject, Procore22left);
                            break;
                    }
                }
            }
        }
    }

    public void ApplyAllAluminium()
    {
        if (aluminiumToggle.isOn)
        {
            procoreToggle.isOn = false;

            foreach (Transform item in RoomModelManager.Instance.CabinetDesign)
            {
                if (item.gameObject.name.ToLower().Contains("procore"))
                {
                    string itemName = item.gameObject.name.Replace("(Clone)", "").Trim();
                    switch (itemName.ToLower())
                    {
                        case "procore40bs":
                            Replace(item.gameObject, Aluminium40);
                            break;
                        case "procore60bs":
                            Replace(item.gameObject, Aluminium60);
                            break;
                        case "procore80bs":
                            Replace(item.gameObject, Aluminium80);
                            break;
                        case "procore22bsright":
                            Replace(item.gameObject, Aluminium22right);
                            break;
                        case "procore22bsleft":
                            Replace(item.gameObject, Aluminium22left);
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

        Vector3 position    = before.transform.position;    
        Quaternion rotation = before.transform.rotation;

        GameObject afterInstance = Instantiate(afterPrefab, position, rotation);

        afterInstance.transform.parent = before.transform.parent;

        Destroy(before);
    }
}
