using UnityEngine;

public class Empty : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<DraggableItem>())
        {
            other.gameObject.GetComponent<DraggableItem>().isCollidingWithOtherCabinets = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject.GetComponent<DraggableItem>())
        {
            other.gameObject.GetComponent<DraggableItem>().isCollidingWithOtherCabinets = false;
        }
    }
}
