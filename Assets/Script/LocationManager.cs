using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class LocationManager : MonoBehaviour
{
    private const string API_URL = "https://api.ipregistry.co/?key=m4vb7xfqyrr6v1yn";

    public static LocationManager Instance;

    public string Country;
    public string CountryCode;
    public string ReignCode;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        StartCoroutine(GetCountryName());
    }


    IEnumerator GetCountryName()
    {
        UnityWebRequest www = UnityWebRequest.Get(API_URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogWarning("Failed to get country name: " + www.error);

            if (PlayerPrefs.HasKey("Country"))
                Country = PlayerPrefs.GetString("Country", "");

            if (PlayerPrefs.HasKey("CountryCode"))
                CountryCode = PlayerPrefs.GetString("CountryCode", "");

            if (PlayerPrefs.HasKey("Reign"))
                ReignCode = PlayerPrefs.GetString("Reign", "");

            //Debug.Log(Country + " " + CountryCode + "  " + ReignCode);
        }
        else
        {
            string response = www.downloadHandler.text;
            JSONNode itemsData =  JSON.Parse(response);
            //Debug.Log(response);

            Country = itemsData["location"]["country"]["name"];
            CountryCode = itemsData["location"]["country"]["code"];
            ReignCode = itemsData["location"]["region"]["code"];

            PlayerPrefs.SetString("Country", Country);
            PlayerPrefs.SetString("CountryCode", CountryCode);
            PlayerPrefs.SetString("Reign", ReignCode);

            Debug.Log("Respose: " + response);
        }
    }
}
