using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadManager : Pixelplacement.Singleton<DownloadManager>
{
    public IEnumerator GETAPI(string url, Text TextComp = null, bool isdownload = false, string path = null)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();
        while (!uwr.isDone)
        {
            Debug.Log(uwr.downloadProgress);
        }

        if (uwr.isDone)
        {
            if (isdownload)
            {
                File.WriteAllBytes(path, uwr.downloadHandler.data);
            }
            else
            {
                TextComp.text = uwr.downloadHandler.text;
            }
        }

        if (uwr.isNetworkError)
        {
            TextComp.text = uwr.isNetworkError.ToString();
        }


    }

    public void DownloadObjects(string url)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        unityWebRequest.SendWebRequest();
    }
}
