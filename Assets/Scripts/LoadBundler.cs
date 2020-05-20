using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;



public class LoadBundler : MonoBehaviour {

    public static LoadBundler loadbundle_Instance;

    private void Awake()
    {
        if (loadbundle_Instance == null)
        {
            loadbundle_Instance = this;
            DontDestroyOnLoad(loadbundle_Instance);
        }
    }


    //string targetName;
    public GameObject Parent;
    public Text ErrorText;
    public AudioClip[] Clips;
    public AudioSource Sounds;
    public string imageName;
    // Use this for initialization
    void Start () {

        ErrorText.text = "ScanSceneLoaded";

    }


    public void BackbtnClicked()
    {
        //StartCoroutine(BackbtnCalled());
        UI_Manager.screenChange = "FromScan";
        SceneManager.LoadScene("DeltaAR");

    }

    void CheckVuforiaStatus()
    {
        //Parent.GetComponent<DefaultTrackableEventHandler>().
    }


    public void ChangeAudio()
    {

        //UI_Manager.UI_instance.InstantiateModel(imageName);

        switch (imageName)
        {
            case "Cow":
                Debug.Log("CowAudio");
                Sounds.clip = Clips[0];
                Sounds.Play();
                break;
        }

    }
   /* public IEnumerator BackbtnCalled()
    {

        // SceneManager.LoadScene("DeltaAR");
        UI_Manager.screenChange = "FromScan";
       



        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("ScanScene");
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //loadingpercent.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                SceneManager.LoadScene("DeltaAR");
                //SceneManager.UnloadSceneAsync("DeltaAR");

            }

            yield return null;
        }

    }*/

    






  /*  public void InstantiateModel(string TrackableName)
    {
        ErrorText.text = "Entered Instantiate Model";

        Parent = GameObject.Find(TrackableName);
        Debug.Log("ParentName: " + Parent.name);

        AssetBundleRequest targetAssetBundleRequest1 = UI_Manager.UI_instance.assetbundle.LoadAssetAsync(TrackableName + ".prefab", typeof(GameObject));
        GameObject ModelGameObject1 = targetAssetBundleRequest1.asset as GameObject;
        GameObject Model = Instantiate(ModelGameObject1);
        Model.transform.SetParent(Parent.transform);



        // ErrorText.text = "AudioPlayed";

        AssetBundleRequest AudioAssetBundleRequest1 = UI_Manager.UI_instance.assetbundle.LoadAssetAsync(TrackableName + ".mp3", typeof(AudioClip));

        Debug.Log("AudioAssets: " + AudioAssetBundleRequest1.asset.GetType());
        if (AudioAssetBundleRequest1 != null)
        {
            Debug.Log("AudioAssets: Not Null");
            AudioClip audioGameObject1 = AudioAssetBundleRequest1.asset as AudioClip;
            Debug.Log("AudioClip: " + audioGameObject1.GetType() + "\n Name: " + audioGameObject1.name);
            if (audioGameObject1 != null)
            {
                Debug.Log("AudioClipName: " + audioGameObject1.name);
                Model.gameObject.AddComponent<AudioSource>();
                Model.gameObject.GetComponent<AudioSource>().clip = audioGameObject1;
                Model.gameObject.GetComponent<AudioSource>().Play();
            }
        }


        // switch (TrackableName)
        // {
        //     case "Cow":
        //         Model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //         Model.transform.localPosition = new Vector3(0, 0.25f, 0);
        //         break;
        //     case "Horse":
        //         Model.transform.localScale = new Vector3(2f, 2f, 2f);
        //         break;
        //     case "Rabbit":
        //         Model.transform.localScale = new Vector3(3f, 3f, 3f);
        //         break;


        // }



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
    */


    // Update is called once per frame
    void Update () {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackbtnClicked();
        }
    }
}
