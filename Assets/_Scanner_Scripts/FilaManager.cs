using UnityEngine;
using GLTFast;
using System.IO;

public class FileManager : MonoBehaviour
{
    private string          filePath;
    public GltfAsset       gltfAsset;
    public bool isSetUpWalls = false; 

    public static FileManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void OpenFileBrowser(byte[] file)
    {
        isSetUpWalls = false;
        gltfAsset.Load(file);
    }

    private void Update()
    {
        if(gltfAsset.transform.childCount > 0 && !isSetUpWalls)
        {
            RoomModelManager.Instance.SetupWalls();
            UIManager.Instance.BackTo3DRoom();
            isSetUpWalls = true;
        }
    }
}
