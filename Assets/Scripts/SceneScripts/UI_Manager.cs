using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SwipeMenu;

public class UI_Manager : Pixelplacement.Singleton<UI_Manager>
{

    public static string screenChange;
    public screenStates changescr;
    public Text DownloadText = null;

    [Header("Screens")]
    public GameObject menuScreen;
    public GameObject splashscreen, progrssbar, signScreen;

    [SerializeField] private float waitTime = 2.0f;
    public bool coolingDown = true;

    public Text loadingpercent;

    private AssetBundle assetbundle;

    void Start()
    {
        progrssbar.GetComponent<Image>().fillAmount = 0f;
        //StartCoroutine(CallSplash());
        splashscreen.gameObject.SetActive(true);
        menuScreen.gameObject.SetActive(false);

        if (screenChange == "FromScan")
        {
            //hidesplash();
            coolingDown = false;
            splashscreen.SetActive(false);
            menuScreen.SetActive(true);
        }
        //changescr = screenStates.menuscreen;
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
        StartCoroutine(callTryDemo());
    }

    //call try demo
    public IEnumerator callTryDemo()
    {
        changescr = screenStates.ScanScreen;
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("ScanScene");
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        while (!asyncOperation.isDone)
        {
            loadingpercent.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            if (asyncOperation.progress >= 0.9f)
            {
                loadingpercent.text = "SceneLoading";
                asyncOperation.allowSceneActivation = true;
                SceneManager.LoadScene("ScanScene");
            }
            yield return null;
        }
    }

    public void Login()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        signScreen.SetActive(false);
        splashscreen.SetActive(false);
        menuScreen.gameObject.SetActive(true);
        changescr = screenStates.signin;
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
        //Fill Amount 
        if (coolingDown == true)
        {
            progrssbar.GetComponent<Image>().fillAmount += 0.4f / waitTime * Time.deltaTime;

            if (progrssbar.GetComponent<Image>().fillAmount == 1.0f)
            {
                Screen.orientation = ScreenOrientation.Portrait;

                signScreen.SetActive(true);
                //HideSplash();
                //Here we need to call login screen
                coolingDown = false;
            }
        }

        //Mobile Back button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (changescr)
            {
                case screenStates.menuscreen:
                    changescr = screenStates.AppQuit;
                    Debug.Log("FirstTimePressed");
                    break;

                case screenStates.AppQuit:
                    Application.Quit();
                    Debug.Log("AppQuit");
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
};




