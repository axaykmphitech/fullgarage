using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitSceneHandling : MonoBehaviour
{
    string[] splitArray;
    public string CurrentDealer;
    public Image image;
    public string BaseURL;
    public string dealerLogo;
    public string dealerName;
    public GameObject loadingPanel;

    [DllImport("__Internal")]
    private static extern void OpenURLInSameTab(string url);

    private void Awake()
    {
        Application.targetFrameRate = 60;
        BaseURL = "https://proslatdesigncenter.com/";

        #if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
        #endif
    }

    void Start()
    {
        if (Application.absoluteURL.Contains("?design"))
        {
            string design_id = Regex.Replace(Application.absoluteURL, "[^0-9]", "");
            StartCoroutine(GetdataFromServer(design_id));
        }

        if (Application.absoluteURL.Contains("/fp/"))
        {
            string Data = Application.absoluteURL;
            splitArray = Data.Split(char.Parse("/"));
            CurrentDealer = splitArray[4];
            StartCoroutine(GetDealerData(CurrentDealer));
        }
    }

    IEnumerator Redirect(string url)
    {
        yield return new WaitForSeconds(0.5f);
        OpenLink(url);
    }

    public void OpenLink(string url)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenURLInSameTab(url);
#else
        Application.OpenURL(url);
#endif
    }

    IEnumerator GetdataFromServer(string design_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("design_id", design_id);

        UnityWebRequest www = UnityWebRequest.Post(BaseURL + "proslat/api/get_share_designs", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JsonData jsonvale = JsonMapper.ToObject(www.downloadHandler.text);
        }
    }

    IEnumerator GetDealerData(string DealerName)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DealerName);
        form.AddField("admin_id", 1);

        UnityWebRequest www = UnityWebRequest.Post(BaseURL + "proslat/api/get_dealer_detail", form);
        loadingPanel.SetActive(true);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            StartCoroutine(GetDealerData(CurrentDealer));
            loadingPanel.SetActive(false);
        }
        else
        {
            JsonData jsonvale = JsonMapper.ToObject(www.downloadHandler.text);

            int responceCode = (int)jsonvale["ResponseCode"];
            if (responceCode == 1 && jsonvale["Result"].Count > 0)
            {
                dealerLogo = jsonvale["Result"]["logo"].ToString();
                dealerName = jsonvale["Result"]["design_link"].ToString();

                if (splitArray[2].Contains(".ca"))
                    dealerLogo = dealerLogo.Replace(".com", ".ca");
            }
            else
            {
                dealerLogo = "https://proslatdesigncenter.com//proslat//public//dealer_logo//17043887546596e8926e1b8.png";

                Debug.Log("splitArray 2 " + splitArray[2]);
                if (splitArray[2].Contains(".ca"))
                    dealerLogo = dealerLogo.Replace(".com", ".ca");
            }
            StartCoroutine(ChangeLogo());
        }

        yield return new WaitForSeconds(1f);
        Debug.Log(dealerName);
        if (dealerName != "")
            CheckForRedirect(dealerName);
        else
            loadingPanel.SetActive(false);

    }


    private void CheckForRedirect(string dealerLink)
    {
        loadingPanel.SetActive(true);
        //LocationManager.Instance.CountryCode =  "CA";
        //LocationManager.Instance.ReignCode = "CA-QC";
        if(dealerLink.Equals("Dallas") || dealerLink.Equals("ProslatUS") || dealerLink.Equals("ProslatCA") || dealerLink.Equals("ProslatQC"))
        {
            Debug.Log("proslat url");
            if (LocationManager.Instance.CountryCode == "US" || LocationManager.Instance.CountryCode == "CA")
            {
                Debug.Log("check for redirect  " + dealerLink);

                if (LocationManager.Instance.CountryCode == "US")
                {
                    if (LocationManager.Instance.ReignCode == "US-TX")
                    {
                        if (!dealerLink.Equals("Dallas"))
                        {
                            StartCoroutine(Redirect("https://proslatdesigncenter.com/fp/Dallas/"));
                        }
                        else
                        {
                            loadingPanel.SetActive(false);
                        }
                    }
                    else
                    {
                        if (!dealerLink.Equals("ProslatUS"))
                        {
                            StartCoroutine(Redirect("https://proslatdesigncenter.com/fp/ProslatUS/"));
                        }
                        else
                        {
                            loadingPanel.SetActive(false);
                        }
                    }
                }
                if (LocationManager.Instance.CountryCode == "CA")
                {
                    if (LocationManager.Instance.ReignCode == "CA-QC")
                    {
                        if (!dealerLink.Equals("ProslatQC"))
                        {
                            StartCoroutine(Redirect("https://proslatdesigncenter.com/fp/ProslatQC/"));
                        }
                        else
                        {
                            loadingPanel.SetActive(false);
                        }
                    }
                    else
                    {
                        if (!dealerLink.Equals("ProslatCA"))
                        {
                            StartCoroutine(Redirect("https://proslatdesigncenter.com/fp/ProslatCA/"));
                        }
                        else
                        {
                            loadingPanel.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                loadingPanel.SetActive(false);
            }
        }
        else
        {
            loadingPanel.SetActive(false);
        }
    }

    public IEnumerator ChangeLogo()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(dealerLogo);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.LogError(request.error);
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = null;

            if (tex)
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

            if (sprite)
                image.overrideSprite = sprite;
        }
    }

    public void LuxConfigurator()
    {
        SceneManager.LoadScene("WindowsLux");
    }

    public void FusionConfigurator()
    {
        SceneManager.LoadScene("WindowsProslat");
    }
}