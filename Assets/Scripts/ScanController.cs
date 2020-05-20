using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ScanController : MonoBehaviour {

    

    public Button backBtn;

	public GameObject MainMenuPanel;
	public GameObject ScanUIPanel;

	public GameObject SplashPanel;
	public Button audioBtn;

	public Button mutebutton;
	public Button playAudioBtn;
	public Button pauseAudioBtn;

	[Header ("MainMenu")]
	[Space (20)]
	public Button tryDemo;
	public Button infoBtn;	
	public Button myProducts;
	public Button feedback;
	public Button helpVideosBtn;

	public AudioClip testClip;




	// Use this for initialization
	void Start () {

		//Changing the screen to landscape
		// Screen.orientation = ScreenOrientation.LandscapeRight;
		// Debug.Log("ScreenOrientation" + Screen.orientation);
		SplashPanel.gameObject.SetActive(true);
		MainMenuPanel.gameObject.SetActive(false);
		ScanUIPanel.gameObject.SetActive(false);

		playAudioBtn.onClick.AddListener(AudioPlayPause);
		pauseAudioBtn.onClick.AddListener(AudioPlayPause);
		audioBtn.onClick.AddListener(MuteAudios);
		backBtn.onClick.AddListener(BackbtnClicked);
		mutebutton.onClick.AddListener(MuteAudios);
		tryDemo.onClick.AddListener(ClickTryDemo);
		feedback.onClick.AddListener(feedbackBtnClicked);
		helpVideosBtn.onClick.AddListener(HelpVideosClicked);
	}


	public bool audioplayenable;
	void AudioPlayPause(){

		if (!audioplayenable){
			
			playAudioBtn.gameObject.SetActive(false);
			pauseAudioBtn.gameObject.SetActive(true);
			// GameObject.Find("ScanController").AddComponent<AudioSource>();
			GameObject.Find("ScanController").GetComponent<AudioSource>().clip = testClip;
			GameObject.Find("ScanController").GetComponent<AudioSource>().Play();
			audioplayenable = true;

		} else{

			pauseAudioBtn.gameObject.SetActive(false);
			playAudioBtn.gameObject.SetActive(true);
			GameObject.Find("ScanController").GetComponent<AudioSource>().Pause();
			audioplayenable = false;

			
			

		}


	}



	bool muteaudio;
	void MuteAudios(){

		if(!muteaudio){
			
			audioBtn.gameObject.SetActive(false);
			mutebutton.gameObject.SetActive(true);
			this.gameObject.GetComponent<AudioSource>().mute = true;
			muteaudio = true;
			// Debug.Log("muteFalse");

		} else {
			audioBtn.gameObject.SetActive(true);
			mutebutton.gameObject.SetActive(false);
			this.gameObject.GetComponent<AudioSource>().mute = false;
			muteaudio = false;
			// Debug.Log("muteTrue");

		}

	}


		void ClickTryDemo(){
		
		ScanUIPanel.gameObject.SetActive(true);
		MainMenuPanel.gameObject.SetActive(false);
		Screen.orientation = ScreenOrientation.LandscapeLeft;


		PlayerPrefs.SetString("MobileBackBtn", "InScanScreen");
	}


	void BackbtnClicked(){

		Screen.orientation = ScreenOrientation.Portrait;
		ScanUIPanel.gameObject.SetActive(false);
		MainMenuPanel.gameObject.SetActive(true);
		
	
		if (MainMenuPanel.gameObject.activeSelf == true){

			PlayerPrefs.SetString("MobileBackBtn", "InMainMenu");
		}
	}

	void feedbackBtnClicked(){

		Application.OpenURL("mailto:contactus@delta.com");
	}


	void HelpVideosClicked(){

		Application.OpenURL("https://youtu.be/fFWL-UrjOkU?list=RDfFWL-UrjOkU");
	}
	
	// Update is called once per frame
	void Update () {

		string backBtnValues = PlayerPrefs.GetString("MobileBackBtn");

if (Input.GetKey(KeyCode.Escape))

		switch(backBtnValues){

			case "InScanScreen":
			Screen.orientation = ScreenOrientation.Portrait;
			ScanUIPanel.gameObject.SetActive(false);
			MainMenuPanel.gameObject.SetActive(true);
		
			break;

			case "InMainMenu":
                    

				

			break;

		}

		
	}
}
