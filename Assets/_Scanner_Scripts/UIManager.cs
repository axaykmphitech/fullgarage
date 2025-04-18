using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using FrostweepGames.Plugins.WebGLFileBrowser;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public Button         openFileDialogButton;
    public GameObject                  urlMenu;
    public GameObject              _3DRoomMenu;
    public GameObject           singleWallMenu;
    public GameObject             loadingPanel;
    public GameObject NotificationmessagePanel;
    public GameObject   AreYouSureToExitDialog;

    public GameObject       startMenu;
    public GameObject     cabinetMenu;
    public GameObject       colorMenu;
    public GameObject worksurfaceMenu;
    public GameObject  backsplashMenu;

    private string _enteredFileExtensions;
    public static      UIManager Instance;
    private File[]           _loadedFiles;

    public List<Toggle> showDimentsionsToggle;
    public bool isDimensionsShow = true;

    public List<Button> deleteButtons;

    private void Awake()
    {
        Instance = this;
        isDimensionsShow = true;
    }

    private void Start()
    {
        WebGLFileBrowser.FilesWereOpenedEvent += FilesWereOpenedEventHandler;
        WebGLFileBrowser.FileOpenFailedEvent  +=  FileOpenFailedEventHandler;

        #if !UNITY_EDITOR
            openFileDialogButton.onClick.AddListener(OpenFileDialogButtonOnClickHandler);
        #endif
        #if UNITY_EDITOR
            RoomModelManager.Instance.gltfAsset.LoadOnStartup = true;
        #endif 
    }

    private void OnDestroy()
    {
        WebGLFileBrowser.FilesWereOpenedEvent -= FilesWereOpenedEventHandler;
        WebGLFileBrowser.FileOpenFailedEvent  -=  FileOpenFailedEventHandler;
    }

    public void OpenWallUi()
    {
        singleWallMenu. SetActive(true);
        _3DRoomMenu.   SetActive(false);
        urlMenu.       SetActive(false);
    }

    public void BackToURL()
    {
        AreYouSureToExitDialog.SetActive(true);
    }

    public void YesSureToExit()
    {
        _3DRoomMenu.   SetActive(false);
        singleWallMenu.SetActive(false);
        urlMenu.        SetActive(true);
        Destroy(RoomModelManager.Instance.RoomModelParent.GetChild(0).gameObject);
        Destroy(DoubleClickDetector.Instance.wallCameraObject.gameObject);
        RoomModelManager.Instance.DestroyAllCabinets();
        SceneManager.LoadScene(0);
    }

    public void BackTo3DRoom()
    {
        _3DRoomMenu.SetActive(true);
        singleWallMenu.SetActive(false);
        urlMenu.SetActive(false);
        RoomModelManager.Instance.EnableAllWalls();
        DoubleClickDetector.Instance.isWallOpen =  false;
        DoubleClickDetector.Instance.selectedWall = null;
        WallCamera[] wallCameras = FindObjectsOfType<WallCamera>();
        foreach (WallCamera wallCamera in wallCameras)
        {
            Destroy(wallCamera.gameObject);
        }
        DestroyAllMeasurementLines();
    }
        
    void DestroyAllMeasurementLines()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name == "MeasurementLine" || obj.name == "DimensionText" || obj.name == "CabinetMesurement" || obj.name == "CabinetDimensionText")
            {
                DoubleClickDetector.Instance.AllDimensions.Remove(obj);
                Destroy(obj);
            }
        }
    }

    public void OpenCabinetMenu()
    {
        cabinetMenu.SetActive(true);
        colorMenu.SetActive(false) ;
        worksurfaceMenu.SetActive(false);
        backsplashMenu.SetActive(false) ;
    }

    public void OpenColorMenu()
    {
        cabinetMenu.SetActive(false);
        colorMenu.SetActive(true);
        worksurfaceMenu.SetActive(false);
        backsplashMenu.SetActive(false) ;
    }

    public void OpenWorksurfaceMenu()
    {
        cabinetMenu.SetActive(false);
        colorMenu.SetActive(false);
        worksurfaceMenu.SetActive(true) ;
        backsplashMenu.SetActive (false);
    }

    public void OpenBacksplashMenu()
    {
        cabinetMenu.SetActive(false);
        colorMenu.SetActive(false)  ;
        worksurfaceMenu.SetActive(false) ;
        backsplashMenu.SetActive (true)  ;
    }

    private void FilesWereOpenedEventHandler(File[] files)
    {
        _loadedFiles = files;

        if (_loadedFiles != null && _loadedFiles.Length > 0)
        {
            var file = _loadedFiles[0];
            FileManager.Instance.OpenFileBrowser(file.data);
            loadingPanel.SetActive(true);
        }
    }

    private void FileOpenFailedEventHandler(string error)
    {
        loadingPanel.SetActive(false);
    }

    private void OpenFileDialogButtonOnClickHandler()
    {
        WebGLFileBrowser.SetLocalization(LocalizationKey.DESCRIPTION_TEXT, "Select file to load or use drag & drop");
        WebGLFileBrowser.OpenFilePanelWithFilters(WebGLFileBrowser.GetFilteredFileExtensions(_enteredFileExtensions), false);
    }

    /// <summary>
    ///  New store listing design
    ///  hard level at start
    ///  more vehicles as reward
    ///  controll ads from firebase
    ///
    ///  dancing doll prototype
    /// </summary>
    /// <param name="toggle"></param>
    public void ShowHideDimensions(Toggle toggle)
    {
        foreach (var item in showDimentsionsToggle)
        {
            if(item != toggle)
            {
                item.isOn = toggle.isOn;
            }
        }

        isDimensionsShow = toggle.isOn;
        DoubleClickDetector.Instance.ShowHideMeasurement(toggle.isOn);
    }

    private void Update()
    {
        foreach (Button btn in deleteButtons)
        {
            if (btn != null)
            {
                btn.interactable = DistanceFromWall.Instance.currentSelectedObject;
            }
        }
    }
}

