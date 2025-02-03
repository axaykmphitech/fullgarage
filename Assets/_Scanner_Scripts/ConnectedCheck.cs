using UnityEngine;

public class ConnectedCheck : MonoBehaviour
{
    public bool isConnected;
    private void OnTriggerStay(Collider other)
    {
        if(GetComponent<Renderer>().material.name.Contains("Green") && !InputManager.Instance.isOutsideWall)
        {
            isConnected = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        isConnected = false;
    }
}
