using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScanScene : MonoBehaviour {

    public AudioClip[] audios;
    public Button playaudioBtn, pauseaudiobtn, muteaudiobtn, audioenablebtn;
    public string imageName;
    public Image downloaderFile;
    public Text text1;

    [Header("Screens")]
    public GameObject downloadingScreen, Scanscreen;

    AssetBundle assetBundle;
    public LoadBundler loadBundleRef;
    //public List<int, >

    Dictionary<string, string[]> storenames = new Dictionary<string, string[]>();
    List<AssetBundle> ab = new List<AssetBundle>();

    //Singleton Pattern
    public static ScanScene Scan_instance;


    private void Awake()
    {
        if (Scan_instance == null)
        {
            Scan_instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }
    }

    //public static ScanScene scan_instance
    //{
    //    get
    //    {
    //        if (Scan_instance == null)
    //        {
    //            Scan_instance = new ScanScene();
                
    //        }
    //        return Scan_instance;
    //    }
    //}




    // Use this for initialization
    void Start () {


        downloaderFile.fillAmount = 0;
        

        //storenames.Add(imageName, assetBundle.GetAllAssetNames());

        
		
	}

    

    public bool audioBool;
    public void AudioButtonClicked()
    {
        
        if (!audioBool)
        {

            audioBool = true;
            Debug.Log("audioPlay: " + audioBool);
            
            pauseaudiobtn.gameObject.SetActive(true);
            playaudioBtn.gameObject.SetActive(false);
            Debug.Log("targetNames" + imageName);
           
            switch (imageName) {
                

                case "Horse":
                this.gameObject.GetComponent<AudioSource>().clip = audios[0];
                this.gameObject.GetComponent<AudioSource>().Play();
                break;

                case "Rabbit":
                this.gameObject.GetComponent<AudioSource>().clip = audios[1];
                this.gameObject.GetComponent<AudioSource>().Play();
                    
                break;

                case "Cow":
                this.gameObject.GetComponent<AudioSource>().clip = audios[2];
                    this.gameObject.GetComponent<AudioSource>().Play();
                    Debug.Log("CowDetected");
                    break;

             
            }
           
            
        }else
        {
            Debug.Log("audioPause: " + audioBool);
            pauseaudiobtn.gameObject.SetActive(false);
            playaudioBtn.gameObject.SetActive(true);
            this.gameObject.GetComponent<AudioSource>().Pause();
            audioBool = false;
        }
        

}



    public void InfobtnClicked()
    {
        
    }


	
    bool PlayandMuteBool;
    public void PlayandMuteBtn()
    {
        if(!PlayandMuteBool)
        {
            audioenablebtn.gameObject.SetActive(false);
            muteaudiobtn.gameObject.SetActive(true);

            this.gameObject.GetComponent<AudioSource>().Pause();
            PlayandMuteBool = true;
            
        } else
        {
            muteaudiobtn.gameObject.SetActive(false);
            audioenablebtn.gameObject.SetActive(true);
            this.gameObject.GetComponent<AudioSource>().Play();
            PlayandMuteBool = false;

        }

    }



	// Update is called once per frame
	void Update () {

       
		
	}
}
