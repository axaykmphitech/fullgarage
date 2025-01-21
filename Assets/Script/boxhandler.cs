using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxhandler : MonoBehaviour
{

    float distnace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDown()
    {

        distnace = transform.GetComponent<MeshRenderer>().bounds.max.y - transform.GetComponent<MeshRenderer>().bounds.min.y;

        gamemanager.instance.distance = distnace;
        gamemanager.instance.posy = gameObject.transform.position.y;



    }
}
