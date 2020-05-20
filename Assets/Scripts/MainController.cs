using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour {


	//MenuScreen Buttons
	public Button tryDemo;
	public Canvas mainMenuCanvas;
	public Canvas splashCanvas;

	public Canvas scanUICanvas;
	// double timetoload = 3.0f;
	

	// Use this for initialization
	void Start () {

	
		mainMenuCanvas.gameObject.SetActive(false);
		scanUICanvas.gameObject.SetActive(false);
		splashCanvas.gameObject.SetActive(true);
		// LoadSceneInScene();
		tryDemo.onClick.AddListener(ClickTryDemo);
	}


	void LoadSceneInScene(){

		// SceneManager.CreateScene("AR_Color");
		// SceneManager.SetActiveScene(SceneManager.GetSceneByName("AR_Color"));
		
		string currentSceneName = SceneManager.GetActiveScene().name;
		Debug.Log(currentSceneName);

		if (currentSceneName == "UI"){

			SceneManager.LoadScene("ScanScene");
			Screen.orientation = ScreenOrientation.LandscapeRight; 
			Debug.Log("Orientation: " + Screen.orientation);
		}
	
	}



	void ClickTryDemo(){

		// SceneManager.LoadSceneAsync("ScanScene", LoadSceneMode.Single);
		scanUICanvas.gameObject.SetActive(true);
		mainMenuCanvas.gameObject.SetActive(false);


		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	
	// Update is called once per frame
	void Update () {

		// timetoload -= Time.deltaTime;
		// if (timetoload <= 0 ){
		// 	splashCanvas.gameObject.SetActive(false);
		// 	mainMenuCanvas.gameObject.SetActive(true);
		// 	Debug.Log("loadUI");

		// }



		
	}
}
