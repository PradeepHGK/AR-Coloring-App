using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
public class BundlerHander : Pixelplacement.Singleton<BundlerHander>
{

    AssetBundle assetbundle;
    WWW www;
    Scan_uiHandler uiHandler;
    GameObject Parent;
    public GameObject loadingImage;
    public GameObject ParentRef;

    [Space(10)]
    public GameObject animBtnPlay, animBtnStop;

    string bundleurl = "https://s3-ap-southeast-1.amazonaws.com/deltabundlebucket/delta";

    // Use this for initialization
    void Start()
    {
        uiHandler = GameObject.Find("Bundle_Controller").GetComponent<Scan_uiHandler>();
        Caching.ClearCache();
    }

    public void CallAssetBundleDownload()
    {

        StartCoroutine(AssetBundleDownload());

    }

    public IEnumerator AssetBundleDownload()
    {

        bool isDownloadInterrupted = false;

        string fbxFile = Application.persistentDataPath + "/" + "delta.unity3d";

        www = new WWW(bundleurl);
        uiHandler.downloadingText.GetComponent<Text>().text = "Download please wait..";
        while (!www.isDone && www.error == null)
        {

#if UNITY_EDITOR
            Debug.Log(www.progress * 100);
#endif

            //PlayerPrefs.SetString("AssetBundle", "Download");
            uiHandler.downloadimage.GetComponent<Image>().fillAmount -= www.progress / 10 * Time.deltaTime;
            if (uiHandler.downloadimage.GetComponent<Image>().fillAmount == 0)
            {
                uiHandler.downloadingText.GetComponent<Text>().text = "Processing please wait...";
            }

            Debug.Log("notDone");

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

            }
            else
            {
                print("Error : " + www.error);
            }
        }


        if (www != null && www.isDone && www.error == null)
        {

            uiHandler.downloadimage.GetComponent<Image>().fillAmount = 0;
            FileStream stream = new FileStream(fbxFile, FileMode.Create);
            stream.Write(www.bytes, 0, www.bytes.Length);
            stream.Close();

            uiHandler.downloadingText.GetComponent<Text>().text = "Please click Chapter 1";
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


        Debug.Log("File------" + File.Exists(Application.persistentDataPath + " / " + "delta.unity3d"));
        Debug.Log("Assetbundle_bool: " + Application.persistentDataPath.Contains("delta.unity3d"));


        WWW loadBundles = WWW.LoadFromCacheOrDownload("file://" + Application.persistentDataPath + "/" + "delta.unity3d", 1);
        yield return loadBundles;

        assetbundle = loadBundles.assetBundle;

        if (!string.IsNullOrEmpty(loadBundles.error))
        {
            Debug.Log(loadBundles.error);
            yield return null;
        }

        if (loadBundles.error != null)
        {
            print("Error: " + loadBundles.error);
        }
        else
        {

            assetbundle = loadBundles.assetBundle;
            foreach (string assetName in assetbundle.GetAllAssetNames())
            {
                print("#" + assetName);
            }

            //if (uiHandler._load_downloded_bundle == true)
            //{
            //    InstantiateModel(DefaultTrackableEventHandler.mTrackableBehaviour.TrackableName);
            //}




        }

    }

    public void InstantiateModel(string TrackableName)
    {

        loadingImage.SetActive(true);

        //Parent = new GameObject();
        //Parent.name = "Parent";


        Parent = GameObject.Find(TrackableName);
        ParentRef = Parent;

        AssetBundleRequest targetAssetBundleRequest1 = assetbundle.LoadAssetAsync(TrackableName + ".prefab", typeof(GameObject));
        GameObject ModelGameObject1 = targetAssetBundleRequest1.asset as GameObject;
        GameObject Model = Instantiate(ModelGameObject1);
        Model.transform.SetParent(Parent.transform);


        AssetBundleRequest AudioAssetBundleRequest1 = assetbundle.LoadAssetAsync(TrackableName + ".mp3", typeof(AudioClip));

        Debug.Log("AudioAssets: " + AudioAssetBundleRequest1.asset.GetType());

        if (AudioAssetBundleRequest1 != null)
        {
            Debug.Log("AudioAssets: Not Null");
            AudioClip audioGameObject1 = AudioAssetBundleRequest1.asset as AudioClip;
            Debug.Log("AudioClip: " + audioGameObject1.GetType() + "\n Name: " + audioGameObject1.name);
            if (audioGameObject1 != null)
            {
                Debug.Log("AudioClipName: " + audioGameObject1.name);
                Parent.gameObject.AddComponent<AudioSource>();
                Parent.gameObject.GetComponent<AudioSource>().clip = audioGameObject1;
                Parent.gameObject.GetComponent<AudioSource>().Play();
            }
        }


        loadingImage.SetActive(false);

        switch (TrackableName)
        {
            case "Cow":
                Model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Model.transform.localPosition = new Vector3(0, 0.25f, 0);
                break;
            case "Horse":
                Model.transform.localScale = new Vector3(2f, 2f, 2f);
                break;
            case "Rabbit":
                Model.transform.localScale = new Vector3(3f, 3f, 3f);
                break;


        }


        AssignRC();
    }


    void AssignRC()
    {
        Component[] ModelSkinMesh = Parent.transform.GetChild(0).GetComponentsInChildren<SkinnedMeshRenderer>();
        Component[] ModelMesh = Parent.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();

        foreach (SkinnedMeshRenderer Mesh in ModelSkinMesh)
        {
            Mesh.gameObject.AddComponent<RC_Get_Texture>();

        }

        foreach (MeshRenderer Mesh in ModelMesh)
        {
            Mesh.gameObject.AddComponent<RC_Get_Texture>();

        }
    }

    private bool _animatorParameter;
    public void PlayAnimation()
    {
        //GameObject _animatorModel = GameObject.FindGameObjectWithTag("Model");
        Animator _animatorModel = ParentRef.transform.GetChild(0).gameObject.GetComponent<Animator>();

        Debug.Log("Animator_Exist");
        if (_animatorParameter == false)
        {
            animBtnPlay.SetActive(false);
            animBtnStop.SetActive(true);
            _animatorModel.SetBool("Run", true);
            _animatorParameter = true;
            Debug.Log("Animator_Run");
        }
        else
        {
            animBtnPlay.SetActive(true);
            animBtnStop.SetActive(false);
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


    IEnumerator GetAssetBundle()
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


    /* IEnumerator Loadbundles()
     {
         var loadbundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.persistentDataPath, bundleName[0]));

         yield return loadbundle;

         var myassetbundle = loadbundle.assetBundle;

         AssetBundleRequest assetbundleRequest = myassetbundle.LoadAssetAsync<GameObject>("");

     }*/


    // Update is called once per frame
    void Update()
    {

        //if (PlayerPrefs.GetString("AssetBundle") == "Download")
        //{
        //    UI_Manager.UI_instance.productscreen.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().fillAmount -= www.progress * 100;

        //}


    }
}
