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

    [SerializeField] private bool isAlreadyAdded;


    void Awake()
    {
        //PlayerPrefs.DeleteAll();
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
        Debug.Log($"AlreadyLoggedIn: {PlayerPrefs.GetString("isLoggedIn")}");
        if (PlayerPrefs.GetString("isLoggedIn") == "true")
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
        var email = "hgk@test.com";
        //StartCoroutine(GetRequest("http://serveapi.herokuapp.com/delta/getallusers"));
        //StartCoroutine(AddEmail($"http://serveapi.herokuapp.com/delta/addemail/{email}"));
        //StartCoroutine(SendData("http://serveapi.herokuapp.com/delta/adduser"));
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
            var response = JsonUtility.FromJson<ValidateSecretCode>(resp);

            if (response.message.Contains("Success"))
            {
                PostLoginEnableMenuScreen();
                PlayerPrefs.SetString("Volume1Enabled", response.status.ToString());
                Debug.Log("UserSigned: " + PlayerPrefs.GetString("Volume1Enabled"));
            }
        }
    }



    private IEnumerator OnClickLogin(string email, string password = null)
    {
        //var url = APIManager.Instance.APIurl.SignupUrl(email);
        var url = "https://serveapi.herokuapp.com/delta/adduser";
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        Debug.Log($"formData: {form.ToString()}");
        var uwr = UnityWebRequest.Post(url, form);
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
                yield return new WaitForSeconds(.1f);
            }

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
        if (!string.IsNullOrEmpty(emailField.text) && PlayerPrefs.GetString("isLoggedIn") != "true")
        {
            //Debug.LogError($"LoginBtnClicked");
            //StartCoroutine(OnClickLogin(emailField.text, passwordField.text));
            StartCoroutine(AddEmail(emailField.text));
            loginErrorText.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString("UserSigned", "NotLogged");
            loginErrorText.gameObject.SetActive(true);
            loginErrorText.text = "Email field shouldn't be empty";
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


    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }


    IEnumerator AddEmail(string email)
    {
        var uri = $"http://serveapi.herokuapp.com/delta/addemail/{email}";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nNewuserData: " + webRequest.downloadHandler.text);
                var json = JsonUtility.FromJson<EmailResponse>(webRequest.downloadHandler.text);

                if(json.status == "true")
                {
                    PostLoginEnableMenuScreen();
                    PlayerPrefs.SetString("isLoggedIn", json.status.ToString());
                }
            }
        }
    }

    IEnumerator SendData(string uri)
    {

        var email = "pradeep.hgk@yahoo.com";
        var password = "hfgklkd";

        LoginAPI loginAPI = new LoginAPI();
        loginAPI.email = email;
        //loginAPI.password = password;

        var json = JsonUtility.ToJson(loginAPI);

        using (var uwr = UnityWebRequest.Post(uri, json))
        {
            Debug.Log($"POSTData: {json}");

            yield return uwr.SendWebRequest();
            Debug.Log($"JSON: {json}");

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(": Error: " + uwr.error);
            }
            else
            {
                Debug.Log(":\nSendData: " + uwr.downloadHandler.text);
            }
        }
    }
}


[System.Serializable]
public class LoginAPI
{
    public string email;
    //public string password;
}


[System.Serializable]
public class EmailResponse
{
    public string data;
    public int code;
    public string status;
    public string date;
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




