using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    Vector3 mousePosition, targetPosition;
    public Camera Cam;
    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        print("---ecvcdsv----");
        //To get the current mouse position
        mousePosition = Input.mousePosition;

        //Convert the mousePosition according to World position
        targetPosition = Cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));

        //Set the position of targetObject
        transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
                 
    }

}
