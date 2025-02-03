using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using FrostweepGames.Plugins.WebGLFileBrowser;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button openFileDialogButton;
    public GameObject urlMenu;
    public GameObject _3DRoomMenu;
    public GameObject singleWallMenu;
    public GameObject loadingPanel;
    public GameObject NotificationmessagePanel;
    public GameObject AreYouSureToExitDialog;

    private string _enteredFileExtensions;

    public static UIManager Instance;

    private File[] _loadedFiles;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        WebGLFileBrowser.FilesWereOpenedEvent += FilesWereOpenedEventHandler;
        WebGLFileBrowser.FileOpenFailedEvent +=  FileOpenFailedEventHandler ;

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
        WebGLFileBrowser.FileOpenFailedEvent -= FileOpenFailedEventHandler;
    }

    public void OpenWallUi()
    {
        singleWallMenu.SetActive(true);
        _3DRoomMenu.SetActive(false);
        urlMenu.SetActive(false);
    }

    public void BackToURL()
    {
        AreYouSureToExitDialog.SetActive(true);
    }

    public void YesSureToExit()
    {
        _3DRoomMenu.SetActive(false);
        singleWallMenu.SetActive(false);
        urlMenu.SetActive(true);
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
        DoubleClickDetector.Instance.isWallOpen = false;
        Destroy(DoubleClickDetector.Instance.wallCameraObject.gameObject);
    }

    private void FilesWereOpenedEventHandler(File[] files)
    {
        _loadedFiles = files;

        if (_loadedFiles != null && _loadedFiles.Length > 0)
        {
            var file = _loadedFiles[0];
            FileManager.Instance.OpenFileBrowser(file.data);
            UIManager.Instance.loadingPanel.SetActive(true);
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
}

