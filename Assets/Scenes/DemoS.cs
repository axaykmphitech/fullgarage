using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemoS : MonoBehaviour
{
    public UnityEvent myEvent;

    private void Start()
    {
        if(myEvent != null)
        {
            myEvent.Invoke();
        }
    }
}
