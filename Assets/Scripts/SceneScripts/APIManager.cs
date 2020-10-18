using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.Networking;
using System.Text;
using System;

public class APIManager : Singleton<APIManager>
{
    public APIURLS APIurl = new APIURLS();

    /// <summary>
    /// Use this method to get api call
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator APICall(string url)
    {
        var uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            Debug.Log($"APIGetProgress: {uwr.downloadProgress}");
        }

        if (uwr.isNetworkError || uwr.isHttpError)
        {

        }
        else
        {
            var resp = uwr.downloadHandler.text;
            Debug.Log($"GETresp: {resp}");
        }
    }

    /// <summary>
    /// Use this method to do all post call 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public IEnumerator APIPOSTCall(string url, string data)
    {
        var jsonData = JsonUtility.ToJson(data);

        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.uploadHandler.contentType = "application/json";
        yield return uwr.SendWebRequest();


        while (!uwr.isDone)
        {
            Debug.Log($"APIPOSTgress: {uwr.downloadProgress}");
        }

        if (uwr.isNetworkError || uwr.isHttpError)
        {

        }
        else
        {
            var resp = uwr.downloadHandler.text;
            Debug.Log($"POSTresp: {resp}");
        }
    }
}

[Serializable]
public class APIURLS
{
    public string APIBaseURL { get { return "http://100.25.160.49:3805/api/v1/"; } }

    public string Loginurl(string username, string password) { return $"login/{username}/{password}"; }
    public string SignupUrl(string username, string email, string password) { return $"/signup/{username}/{email}/{password}"; }
    public string BookValidationAPIurl(string secretCode) { return $"verifyBook/{secretCode}"; }
}