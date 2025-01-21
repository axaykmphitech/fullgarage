using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadVideo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        print("============");
        if (GetComponent<VideoPlayer>().url == "")
        {
            print("============sdasdasd");
            GetComponent<VideoPlayer>().url = Application.streamingAssetsPath + "/TutorialVideo.mp4";
            GetComponent<VideoPlayer>().Play();
        }

    }    
}
