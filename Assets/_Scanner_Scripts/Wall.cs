using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour
{
    public List<Transform> wallCabinets = new List<Transform>();

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        DistanceFromWall.Instance.DestroyAllFromCabinetToWallMeasurement();
        DistanceFromWall.Instance.currentSelectedObject = null;
        DistanceFromWall.Instance.tempObject = null;
        foreach (Transform item in RoomModelManager.Instance.CabinetDesign)
        {
            item.GetComponentInChildren<QuikOutline>().enabled = false;
        }
    }
}
