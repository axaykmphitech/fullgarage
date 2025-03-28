using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseFolloScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print("---ecv----");
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    private void OnMouseDown()
    {
        //print("------OnMouseDown--->>>>>");
    }
    private void OnMouseDrag()
    {
        //print("-----OnMouseDrag---->>>>>>");
    }
   
}
