using System.Collections;
using System.Collections.Generic;
using GLTFast;
using UnityEngine;

public class RoomModelManager : MonoBehaviour
{
    private List<Transform> allWalls;
    public List<Transform> walls;
    public Transform floor;
    public Transform RoomModelParent;
    [HideInInspector]
    public GltfAsset gltfAsset;
    public Transform CabinetDesign;

    public float floorPositionY;
    public float wallmountYPosition;
    public float overheadYPosition;

    public static RoomModelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gltfAsset = RoomModelParent.GetComponent<GltfAsset>();
    }

    private IEnumerator DelaySetupWalls()
    {
        yield return new WaitForSeconds(1f);

        allWalls = new List<Transform>();
        List<Transform> allFloor  = new List<Transform>();
        foreach (Transform item in RoomModelParent.transform.GetChild(0).transform)
        {
            if(item.name.Contains("wall"))
            {
                allWalls.Add(item);
            }
            if(item.name.Contains("floor"))
            {
                allFloor.Add(item);
            }
            if (item.name.Contains("ceiling"))
            {
                item.gameObject.SetActive(false);
            }
        }
        walls.Clear();
        for (int i = 0; i < allWalls.Count; i++)
        {
            if (i % 5 == 0)
            {
                walls.Add(allWalls[i]);
                allWalls[i].GetComponent<Renderer>().material = DoubleClickDetector.Instance.normalWallMaterial;
            }
        }
        foreach (Transform item in walls)
        {
            item.gameObject.AddComponent<BoxCollider>();
        }

        floorPositionY = allFloor[0].gameObject.AddComponent<BoxCollider>().bounds.center.y;
        wallmountYPosition = floorPositionY + 1.4973f;
        overheadYPosition = floorPositionY + 2.080293f;
        UIManager.Instance.loadingPanel.SetActive(false);
    }

    public void SetupWalls()
    {
        StartCoroutine(DelaySetupWalls());
    }

    public void EnableAllWalls()
    {
        foreach (Transform item in RoomModelParent.transform.GetChild(0).transform)
        {
            if(!item.gameObject.name.Contains("ceiling"))
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    public void DestroyAllCabinets()
    {
        foreach (Transform item in CabinetDesign)
        {
            Destroy(item.gameObject);
        }
    }
}
