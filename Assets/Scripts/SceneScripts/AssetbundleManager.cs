using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using System;

public class AssetbundleManager : Pixelplacement.Singleton<AssetbundleManager>
{
    private AssetBundle assetbundle;
    public AssetBundle DeltaAssetbundle { get { return assetbundle; } }
    private WWW www;
    private ScanUIManager uiHandler;
    private GameObject Parent;
    //private string bundleurl = "https://pradeepdevbuckets.s3.ap-south-1.amazonaws.com/volume1";
    private string bundleurl = "https://deltabackend.s3-ap-southeast-1.amazonaws.com/volume1";
    private bool _animatorParameter;

    private GameObject loadingImage;
    public GameObject LoadingImage { get { return loadingImage; } set { loadingImage = value; } }
    public GameObject ParentRef;
    private bool isBundleDownloading;
    public bool IsBundleDownloading { get { return isBundleDownloading; } set { isBundleDownloading = value; } }
    void OnEnable()
    {
        EventManager.Instance.DownloadAssetbundle += DownloadAssetbundle;
    }

    void OnDisable()
    {
        EventManager.Instance.DownloadAssetbundle -= DownloadAssetbundle;
    }

    private void DownloadAssetbundle()
    {
        StartCoroutine(AssetBundleDownload(delegate () { Debug.Log("LoadBundles"); ScanUIManager.Instance.OnClickChapters(); }));
    }


    // Use this for initialization
    void Start()
    {
        Caching.ClearCache();
    }

    public IEnumerator AssetBundleDownload(Action OnComplete)
    {
        isBundleDownloading = true;
        bool isDownloadInterrupted = false;
        string fbxFile = Application.persistentDataPath + "/" + "volume1";

        www = new WWW(bundleurl);
        ScanUIManager.Instance.DownloadingText.GetComponent<Text>().text = "Downloading please wait..";
        while (!www.isDone && www.error == null)
        {
            ScanUIManager.Instance.downloadProgressbar.value = www.progress;
#if UNITY_EDITOR
            Debug.Log(www.progress * 100);
#endif
            ScanUIManager.Instance.DownloadImage.GetComponent<Image>().fillAmount -= www.progress / 100 * Time.deltaTime;
            if (ScanUIManager.Instance.DownloadImage.GetComponent<Image>().fillAmount == 0)
            {
                //ScanUIManager.Instance.DownloadImage.GetComponent<Text>().text = "Processing please wait...";
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                isDownloadInterrupted = true;
                break;
            }
            yield return null;
        }

        if ((www.error != null && !www.isDone) || isDownloadInterrupted == true)
        {
            if (isDownloadInterrupted)
            {
                print("Connection Error.... Retrying Download");
                StartCoroutine(AssetBundleDownload(delegate () { Debug.Log("NetworkInteruppted"); ScanUIManager.Instance.OnClickChapters(); }));
            }
            else
            {
                print("Error : " + www.error);
            }
        }

        if (www != null && www.isDone && www.error == null)
        {
            ScanUIManager.Instance.DownloadImage.GetComponent<Image>().fillAmount = 0;
            FileStream stream = new FileStream(fbxFile, FileMode.Create);
            stream.Write(www.bytes, 0, www.bytes.Length);
            stream.Close();
            OnComplete.Invoke();
        }
    }

    public IEnumerator LoadBundles()
    {
        if (assetbundle != null)
        {
            assetbundle.Unload(false);
            Debug.Log("bundle_unloaded");
        }

        while (!Caching.ready)
        {
            yield return null;
        }

        Debug.Log("File------" + File.Exists(Application.persistentDataPath + " / " + "volume1"));

        WWW loadBundles = WWW.LoadFromCacheOrDownload("file://" + Application.persistentDataPath + "/" + "volume1", 1);
        yield return loadBundles;

        assetbundle = loadBundles.assetBundle;

        if (!string.IsNullOrEmpty(loadBundles.error))
        {
            Debug.Log(loadBundles.error);
            yield return null;
        }

        if (loadBundles.error != null)
        {
            Debug.Log("Error: " + loadBundles.error);
        }
        else
        {
            assetbundle = loadBundles.assetBundle;
            foreach (string assetName in assetbundle.GetAllAssetNames())
            {
                Debug.Log("#" + assetName);
            }
        }
    }

    public void PlayAnimation()
    {
        //GameObject _animatorModel = GameObject.FindGameObjectWithTag("Model");
        Animator _animatorModel = ParentRef.transform.GetChild(0).gameObject.GetComponent<Animator>();
        Debug.Log("Animator_Exist");
        if (_animatorParameter == false)
        {
            //animBtnPlay.SetActive(false);
            //animBtnStop.SetActive(true);
            _animatorModel.SetBool("Run", true);
            _animatorParameter = true;
            Debug.Log("Animator_Run");
        }
        else
        {
            //animBtnPlay.SetActive(true);
            //animBtnStop.SetActive(false);
            _animatorModel.GetComponent<Animator>().SetBool("Run", false);
            _animatorParameter = false;
            Debug.Log("Animator_Stop");
        }
    }

    IEnumerator DownloadBundle()
    {
        //Reference for downloading Assetbundles - DownloadHandler
        //https://docs.unity3d.com/Manual/UnityWebRequest-CreatingDownloadHandlers.html

        using (UnityWebRequest getassetbundle = UnityWebRequest.GetAssetBundle("https://s3-ap-southeast-1.amazonaws.com/deltabundlebucket/delta"))
        {
            //getassetbundle.downloadHandler = new DownloadHandlerFile(fbxFile);

            //yield return getassetbundle.SendWebRequest();
            yield return null;
            //AsyncOperation bundlereq = getassetbundle.SendWebRequest();

            //while (!bundlereq.isDone)
            //{
            //    Debug.Log(bundlereq.progress);
            //    scanobj.downloaderFile.fillAmount = bundlereq.progress;

            //}


            //UnityWebRequestAsyncOperation newsync = getassetbundle.
            UnityWebRequestAsyncOperation newreq = getassetbundle.SendWebRequest();
            //Debug.Log(newreq.webRequest.downloadProgress);

            while (!newreq.isDone)
            {
                Debug.Log(newreq.progress);
            }

            if (!getassetbundle.isDone)
            {
                Debug.Log("Download Not Completed");
            }
            else
            {
                //AssetBundle assetbundleDelta = DownloadHandlerAssetBundle.GetContent(getassetbundle);
            }
        }
    }

    /*    IEnumerator GetAssetBundle()
        {
            UnityWebRequest www = UnityWebRequest.GetAssetBundle(bundleurl);
            yield return www.SendWebRequest();
            Debug.Log(www.downloadProgress);
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                Debug.Log(www.responseCode);
            }
        }
    */

    /* IEnumerator Loadbundles()
     {
         var loadbundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.persistentDataPath, bundleName[0]));

         yield return loadbundle;

         var myassetbundle = loadbundle.assetBundle;

         AssetBundleRequest assetbundleRequest = myassetbundle.LoadAssetAsync<GameObject>("");

     }*/
}
