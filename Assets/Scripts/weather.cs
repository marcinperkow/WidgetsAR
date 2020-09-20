using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using SimpleJSON;

public class weather : MonoBehaviour
{   
    public GameObject tempTextObject;
    public GameObject humidityTextObject;
    public GameObject windTextObject;
    public GameObject tempBar;
    public GameObject humBar;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=89c29e5f4f1a691286f2d0e9bae7049d&units=imperial";

    void Start()
    {
        InvokeRepeating("GetDataFromWeb", 2f, 30f);
    }

    void GetDataFromWeb()
    {
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            
            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                JSONNode data = JSON.Parse(webRequest.downloadHandler.text);
                tempTextObject.GetComponent<TextMeshPro>().text = data["main"]["temp"] + " F";
                humidityTextObject.GetComponent<TextMeshPro>().text = data["main"]["humidity"] + "%";
                windTextObject.GetComponent<TextMeshPro>().text = data["wind"]["speed"] + " mph " + data["wind"]["deg"];
                tempBar.transform.localScale = new Vector3(transform.localScale.x, data["main"]["temp"] * 0.01f, transform.localScale.z);
                tempBar.transform.position = new Vector3(transform.position.x, data["main"]["temp"] * 0.01f + 0.46f, transform.position.z);
            }
        }
    }
}
