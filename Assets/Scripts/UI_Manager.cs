using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class UI_Manager : Pixelplacement.Singleton<UI_Manager>
{
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
	AssetBundle assetbundle;

	public static string screenChange;
	public static screenStates changescr;
	public Text DownloadText = null;

	[Header("Screens")]
	public GameObject menuScreen;
	public GameObject splashscreen, progrssbar, signScreen;

	float waitTime = 2.0f;
	public bool coolingDown = true;

	//LoadingPercent
	public Text loadingpercent;

	//public GameObject Parent;
	//public Text ErrorText;

	void Start()
	{
		//downloadBundle = GameObject.Find("Directional Light").GetComponent <DownloadAssetBundle>();
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

		//Begin to load the Scene you specify
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("ScanScene");
		//Don't let the Scene activate until you allow it to
		asyncOperation.allowSceneActivation = false;
		Debug.Log("Pro :" + asyncOperation.progress);
		//When the load is still in progress, output the Text and progress bar
		while (!asyncOperation.isDone)
		{

			//Output the current progress
			loadingpercent.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

			//loadingImage.GetComponent<Image>().fillAmount = asyncOperation.progress * 100;

			// Check if the load has finished
			if (asyncOperation.progress >= 0.9f)
			{
				//Change the Text to show the Scene is ready
				loadingpercent.text = "SceneLoading";
				//Wait to you press the space key to activate the Scene
				// if (Input.GetKeyDown(KeyCode.Space))
				//Activate the Scene
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
		//send feedback
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





