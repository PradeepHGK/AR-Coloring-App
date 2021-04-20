using Pixelplacement;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScanUIManager : Singleton<ScanUIManager>
{
    [Header("Screen References")]
    [SerializeField] private GameObject ProductScreen;
    [SerializeField] private GameObject ScanScreen;
    [SerializeField] private GameObject downloadimage;

    [Space]
    [SerializeField] private GameObject ActivationScreen;
    [SerializeField] private InputField keyField;
    [SerializeField] private Text keyValidationMessage;
    [SerializeField] private Button ActivateButton;
    [Space]
    [SerializeField] private GameObject backButton;


    public Slider downloadProgressbar { get { return Progressbar; } set { Progressbar = value; } }
    [SerializeField] private Slider Progressbar;

    public float progressvalue;

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
    public Button AudioPlayBtn;
    public Button AudioMuteBtn;
    public Button AnimationPlayBtn;
    public Button AnimationPauseBtn;

    #region Unity Methods
    private void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += OnTrackingFound;
        EventManager.Instance.OnTrackingLost += OnTrackingLost;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTrackingFound -= OnTrackingFound;
        EventManager.Instance.OnTrackingLost -= OnTrackingLost;
    }

    void Start()
    {
        backbtn.SetActive(true);
        errortext.text = Application.persistentDataPath.Contains("volume1").ToString();
        CheckBundleAvailability();

        //Button listeners
        ActivateButton.onClick.AddListener(OnClickActivation);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Backbtn();
    }
    #endregion

    #region Event Methods
    private void OnTrackingFound(string arg1, GameObject arg2)
    {
        ScanScreen.gameObject.SetActive(true);
    }

    private void OnTrackingLost()
    {
        ScanScreen.gameObject.SetActive(false);
    }
    #endregion

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
            downloadimage.GetComponent<Image>().fillAmount = 0;
            downloadProgressbar.gameObject.SetActive(false);
            UI_Manager.Instance.changescr = screenStates.ScanScreen;
            Debug.Log("StartScanFileExist");
        }
    }

    public void OnClickChapters()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + "volume1"))
        {
            Screen.orientation = ScreenOrientation.Landscape;
            backButton.SetActive(true);
            downloadimage.GetComponent<Image>().fillAmount = 1;
            UI_Manager.Instance.changescr = screenStates.ActivationScreen;
            backbtn.GetComponent<Button>().interactable = false;
            ActivationScreen.SetActive(true);

            //StartCoroutine(AssetbundleManager.Instance.AssetBundleDownload(delegate ()
            //{
            //    AssetbundleManager.Instance.IsBundleDownloading = false;
            //    backbtn.GetComponent<Button>().interactable = true;
            //}));

            //TODO: Enable Download Assetbundle
            //EventManager.Instance.DownloadAssetbundleInvoke();
        }
        else
        {
            //Debug.Log("BundleLoaded");
            Screen.orientation = ScreenOrientation.Landscape;
            ScanScreen.SetActive(true);
            ProductScreen.SetActive(false);
            StartCoroutine(AssetbundleManager.Instance.LoadBundles());
            downloadimage.GetComponent<Image>().fillAmount = 0;
            backbtn.GetComponent<Button>().interactable = true;
            UI_Manager.Instance.changescr = screenStates.ScanScreen;
        }
    }

    private IEnumerator ValidateCode(string secretCode)
    {
        //var url = APIManager.Instance.APIurl.SignupUrl(email);
        var url = $"https://serveapi.herokuapp.com/delta/getallkey/{secretCode}";
        var uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            Debug.Log($"APIGetProgress: {uwr.downloadProgress}");
        }

        Debug.LogError($"{uwr.error}");
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError($"NetworkError: {uwr.isNetworkError} {uwr.isHttpError}");
        }
        else
        {
            while (!uwr.isDone)
            {
                //yield return new WaitForSeconds(.1f);
            }

            var resp = uwr.downloadHandler.text;
            Debug.Log($"GETresp: {resp}");
            var response = JsonUtility.FromJson<Root>(resp);

            if (response.status)
            {
                PlayerPrefs.SetString("Volume1Enabled", response.message);
                Debug.Log("UserSigned: " + PlayerPrefs.GetString("Volume1Enabled"));

                keyValidationMessage.text = response.message;

                Screen.orientation = ScreenOrientation.Landscape;
                backButton.SetActive(false);
                ActivationScreen.SetActive(false);
                ProductScreen.gameObject.SetActive(true);

                StartCoroutine(AssetbundleManager.Instance.AssetBundleDownload(delegate ()
                {
                    ProductScreen.gameObject.SetActive(false);
                    AssetbundleManager.Instance.IsBundleDownloading = false;
                    backbtn.SetActive(true);
                    backbtn.GetComponent<Button>().interactable = true;
                }));
            }
            else
            {
                keyValidationMessage.text = response.message;
            }
        }
    }

    public void OnClickActivation()
    {
        if (!string.IsNullOrEmpty(keyField.text))
        {
            //Screen.orientation = ScreenOrientation.Landscape;
            //backButton.SetActive(false);
            //ProductScreen.gameObject.SetActive(true);
            StartCoroutine(ValidateCode(keyField.text));
            ActivateButton.interactable = false;
            UI_Manager.Instance.changescr = screenStates.downloading;
        }
        else
        {
            ActivateButton.interactable = true;
            keyValidationMessage.text = "Please enter the secret code";
        }
    }

    public void Backbtn()
    {
        switch (UI_Manager.Instance.changescr)
        {
            case screenStates.ActivationScreen:
                backButton.SetActive(false);
                ProductScreen.SetActive(true);
                UI_Manager.Instance.changescr = screenStates.productsList;
                break;

            case screenStates.downloading:
                //Print to wait
                break;

            case screenStates.productsList:
                if (AssetbundleManager.Instance.DeltaAssetbundle != null)
                {
                    AssetbundleManager.Instance.DeltaAssetbundle.Unload(false);
                }

                downloadimage.GetComponent<Image>().fillAmount = 0;
                downloadingText.GetComponent<Text>().text = "";
                SceneManager.LoadScene("DeltaAR");
                UI_Manager.screenChange = "FromScan";
                UI_Manager.Instance.changescr = screenStates.menuscreen;
                break;

            case screenStates.ScanScreen:
                //ScanScreen.SetActive(false);
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
