using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SwipeMenu;
using Vuforia;
using UnityEngine.Networking;

public class UI_Manager : Pixelplacement.Singleton<UI_Manager>
{
    public static string screenChange;
    public screenStates changescr;
    public Text DownloadText = null;

    [Header("Screens")]
    public GameObject menuScreen;
    public GameObject splashscreen, progrssbar, signScreen;

    [Space]
    [SerializeField] private GameObject RegisterScreen;
    [SerializeField] private GameObject LoginScreen;
    [SerializeField] private Text loginErrorText;

    [Space]
    [SerializeField] private float waitTime = 2.0f;
    public bool coolingDown = true;
    public Text loadingpercent;
    private AssetBundle assetbundle;

    [Header("InputFields")]
    [SerializeField] private InputField emailField;
    [SerializeField] private InputField passwordField;
    [SerializeField] private InputField userNameField;


    [Header("Buttons")]
    [SerializeField] private Button SignInButton;
    [SerializeField] private Button SignUpButton;

    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    void Start()
    {
        //button references 
        SignInButton.onClick.AddListener(Login);

        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);

        progrssbar.GetComponent<UnityEngine.UI.Image>().fillAmount = 0f;

        // || screenChange == "FromScan"
        if (PlayerPrefs.GetString("UserSigned") == "Success")
        {
            coolingDown = false;
            splashscreen.SetActive(false);
            menuScreen.SetActive(true);
            Debug.Log("MenuScreen");
            PostLoginEnableMenuScreen();
        }
        else
        {
            Debug.Log("SplashScreen");
            splashscreen.gameObject.SetActive(true);
            menuScreen.gameObject.SetActive(false);
        }
        //changescr = screenStates.menuscreen;
    }


    private IEnumerator OnClickLogin(string email, string password = null)
    {
        var url = APIManager.Instance.APIurl.SignupUrl(email);

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
            var resp = uwr.downloadHandler.text;
            Debug.Log($"GETresp: {resp}");
            var response = JsonUtility.FromJson<Subscribe>(resp);

            if (response.message.Contains("Success"))
            {
                PostLoginEnableMenuScreen();
                PlayerPrefs.SetString("UserSigned", "Success");
                Debug.Log("UserSigned: " + PlayerPrefs.GetString("UserSigned"));
            }
        }
    }

    private void PostLoginEnableMenuScreen()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        signScreen.SetActive(false);
        splashscreen.SetActive(false);
        menuScreen.gameObject.SetActive(true);
        changescr = screenStates.signin;
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

    private void AssignClickListener()
    {

    }

    public void HideSplash()
    {
        splashscreen.gameObject.SetActive(false);
        menuScreen.gameObject.SetActive(true);
    }

    public void tryDemo()
    {
        StartCoroutine(OnClickTryDemo());
    }

    public IEnumerator OnClickTryDemo()
    {
        changescr = screenStates.ScanScreen;
        SceneManager.LoadScene("ScanScene");
        yield return new WaitForSeconds(2);
    }

    public void Login()
    {
        if (!string.IsNullOrEmpty(emailField.text))
        {
            //Debug.LogError($"LoginBtnClicked");
            StartCoroutine(OnClickLogin(emailField.text));
            loginErrorText.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString("UserSigned", "NotLogged");
            loginErrorText.gameObject.SetActive(true);
            loginErrorText.text = "Email field shouldn't be empty";
        }

        if (PlayerPrefs.GetString("UserSigned") == "Success")
        {

        }
        else
        {
            Debug.Log($"PostLoginEnableMenuScreen");
        }
    }

    public void Products()
    {
        SceneManager.LoadScene("ScanScene");
        changescr = screenStates.productsList;
    }

    public void HelpVideo()
    {
        //Add video links 
        Application.OpenURL("https://youtu.be/fFWL-UrjOkU?list=RDfFWL-UrjOkU");
        changescr = screenStates.helpvideo;
    }

    public void MyAccount()
    {
        //Add user account details
    }

    public void Feedback()
    {
        Application.OpenURL("mailto:contactus@delta.com");
        changescr = screenStates.feedback;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown == true)
        {
            progrssbar.GetComponent<UnityEngine.UI.Image>().fillAmount += 0.4f / waitTime * Time.deltaTime;

            if (progrssbar.GetComponent<UnityEngine.UI.Image>().fillAmount == 1.0f)
            {
                Screen.orientation = ScreenOrientation.Portrait;
                if (PlayerPrefs.GetString("UserSigned") != "Success")
                {
                    signScreen.SetActive(true);
                    coolingDown = false;
                }
            }
        }

        //Mobile Back button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (changescr)
            {
                case screenStates.menuscreen:
                    changescr = screenStates.AppQuit;
                    break;

                case screenStates.AppQuit:
                    Application.Quit();
                    break;
            }
        }
    }
}

public enum screenStates
{
    LoginScreen,
    ScanScreen,
    menuscreen,
    productsList,
    ActivationScreen,
    downloading,
    signin,
    signup,
    helpvideo,
    feedback,
    AppQuit
}




