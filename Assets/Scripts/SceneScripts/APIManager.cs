﻿using System.Collections;
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
    public IEnumerator APICall(string url, Action<string> OnComplete)
    {
        Debug.Log($"LoginURL {url}");
        var uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            Debug.Log($"APIGetProgress: {uwr.downloadProgress}");
        }

        if (uwr.isNetworkError || uwr.isHttpError)
        {

            Debug.LogError($"NetworkError: {uwr.isNetworkError}");
        }
        else
        {
            var resp = uwr.downloadHandler.text;
            OnComplete(resp);
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

    public void ParseJSON<T>(string resp, T value)
    {
        Debug.Log(value.ToString());
        var t = JsonUtility.FromJson<T>(resp);
    }
}

[Serializable]
public class APIURLS
{
    public string APIBaseURL { get { return "http://54.179.176.189:3805/api/v1"; } }
    public string Loginurl(string username, string password) { return $"{APIBaseURL}/login/{username}/{password}"; }
    public string SubscribeURL(string email) { return $"{APIBaseURL}/login/{email}"; }
    public string SignupUrl(string email) { return $"{APIBaseURL}/signup/{email}"; }
    //public string SignupUrl(string username, string email, string password) { return $"/signup/{username}/{email}/{password}"; }
    public string BookValidationAPIurl(string secretCode) { return $"verifyBook/{secretCode}"; }
}


[Serializable]
public class Datum
{
    public int userid;
    public string username;
    public string email;
    public string password;
}

[Serializable]
public class LoginRoot
{
    public List<Datum> data;
    public string message;
}

[Serializable]
public class ValidateSecretCode
{
    public string message;
    public bool status;
}

public class Data
{
    public string _id;
    public string volume1;
    public int __v;
}

public class Root
{
    public bool status;
    public Data data;
    public int code;
    public string message;
}



[Serializable]
public class Subscribe
{
    public int code;
    public string message;
}