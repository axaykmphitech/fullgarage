using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWidthAndHeight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 posStart = Camera.main.WorldToScreenPoint(new Vector3(GetComponent<MeshRenderer>().bounds.min.x, GetComponent<MeshRenderer>().bounds.min.y, GetComponent<MeshRenderer>().bounds.min.z));
        Vector3 posEnd = Camera.main.WorldToScreenPoint(new Vector3(GetComponent<MeshRenderer>().bounds.max.x, GetComponent<MeshRenderer>().bounds.max.y, GetComponent<MeshRenderer>().bounds.min.z));

        print("---bounds.min.x----" + posStart + "---bounds.max.x----" + posEnd + "------" + transform.parent.gameObject.name);
        int widthX = (int)(posEnd.x - posStart.x);
        int widthY = (int)(posEnd.y - posStart.y);

        print("---widthX----" + widthX + "--widthY--" + widthY + "------" + transform.parent.gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
