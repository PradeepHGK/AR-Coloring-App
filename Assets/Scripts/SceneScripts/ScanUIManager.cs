using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixelplacement;
using System;

public class ScanUIManager : Singleton<ScanUIManager>
{
    [Header("Screen References")]
    [SerializeField] private GameObject ProductScreen;
    [SerializeField] private GameObject ActivationScreen;
    [SerializeField] private GameObject ScanScreen;
    [SerializeField] private GameObject downloadimage;
    public GameObject DownloadImage
    {
        get
        {
            return downloadimage;
        }

        set
        {
            downloadimage = value;
        }
    }
    [SerializeField] private GameObject downloadingText;
    public GameObject DownloadingText
    {
        get
        {
            return downloadingText;
        }
        set
        {
            downloadingText = value;
        }
    }

    [SerializeField] private GameObject backbtn;
    public Text errortext;

    [Header("Play&Stop Buttons")]
    public GameObject animBtnPlay;
    public GameObject animBtnStop;
    public Button playAudioBtn;
    public Button pauseAudioBtn;

    public bool _load_downloded_bundle;

    private void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += OnTrackingFound;
        EventManager.Instance.OnTrackingLost += OnTrackingLost;
    }

    private void OnTrackingLost()
    {
        ScanScreen.SetActive(false);
    }

    private void OnTrackingFound(string arg1, GameObject arg2)
    {
        ScanScreen.SetActive(true);
    }

    void Start()
    {
        backbtn.SetActive(true);
        errortext.text = Application.persistentDataPath.Contains("volume1").ToString();

        CheckBundleAvailability();
    }

    private void CheckBundleAvailability(Action<string> OnComplete = null)
    {
        if (!File.Exists(Application.persistentDataPath + "/" + "volume1"))
        {
            ProductScreen.SetActive(true);
            downloadimage.GetComponent<Image>().fillAmount = 1;
            UI_Manager.Instance.changescr = screenStates.productsList;
            Debug.Log("StartScanFileNotExist");
        }
        else
        {
            ScanScreen.SetActive(true);
            StartCoroutine(AssetbundleManager.Instance.LoadBundles());
            _load_downloded_bundle = true;
            UI_Manager.Instance.changescr = screenStates.ScanScreen;
            Debug.Log("StartScanFileExist");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Backbtn();
        }
    }

    public void Chapter1()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + "volume1"))
        {
            Screen.orientation = ScreenOrientation.Portrait;
            ActivationScreen.SetActive(true);
            downloadimage.GetComponent<Image>().fillAmount = 1;
            UI_Manager.Instance.changescr = screenStates.ActivationScreen;
            EventManager.Instance.DownloadAssetbundleInvoke();
        }
        else
        {
            Screen.orientation = ScreenOrientation.Landscape;
            ScanScreen.SetActive(true);
            ProductScreen.SetActive(false);
            StartCoroutine(AssetbundleManager.Instance.LoadBundles());
            downloadimage.GetComponent<Image>().fillAmount = 0;
            UI_Manager.Instance.changescr = screenStates.ScanScreen;
        }
    }


    public void ClickActivation()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        ActivationScreen.SetActive(false);
        ProductScreen.gameObject.SetActive(true);
        StartCoroutine(AssetbundleManager.Instance.AssetBundleDownload());

        UI_Manager.Instance.changescr = screenStates.downloading;
    }


    public void Backbtn()
    {
        switch (UI_Manager.Instance.changescr)
        {
            case screenStates.ActivationScreen:

                ActivationScreen.SetActive(false);
                ProductScreen.SetActive(true);
                UI_Manager.Instance.changescr = screenStates.productsList;
                break;

            case screenStates.downloading:
                //Print to wait
                break;

            case screenStates.productsList:
                if (AssetbundleManager.Instance.DeltaAssetbundle != null)
                {
                    Debug.Log("BackBtn_BundleUnloaded");
                    AssetbundleManager.Instance.DeltaAssetbundle.Unload(false);
                }

                downloadimage.GetComponent<Image>().fillAmount = 0;
                downloadingText.GetComponent<Text>().text = "";
                SceneManager.LoadScene("DeltaAR");
                UI_Manager.screenChange = "FromScan";
                UI_Manager.Instance.changescr = screenStates.menuscreen;
                break;

            case screenStates.ScanScreen:
                ScanScreen.SetActive(false);
                ProductScreen.SetActive(true);
                UI_Manager.Instance.changescr = screenStates.productsList;
                downloadimage.GetComponent<Image>().fillAmount = 0;
                downloadingText.GetComponent<Text>().text = "";
                break;
        }
    }

    public void UIBackBtn()
    {
        downloadimage.GetComponent<Image>().fillAmount = 0;
        downloadingText.GetComponent<Text>().text = "";
        ScanScreen.SetActive(false);
        ProductScreen.SetActive(true);
        backbtn.SetActive(true);
        UI_Manager.Instance.changescr = screenStates.menuscreen;
    }

}
