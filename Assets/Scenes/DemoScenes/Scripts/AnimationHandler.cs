using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

public class AnimationHandler : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject closeBtn;
    public AudioClip[] audioSources;
    public AudioSource AudioSource;

    public event Action AudioEvent;
    public void AudioEvenTrigger() { AudioEvent.Invoke(); }
    bool isAudioPlaying;
    public DefaultTrackableEventHandler DefaultTrackableEventHandler;
    ImageTargetBehaviour ImageTargetBehaviour;

    private void OnEnable()
    {
        EventHandler.Instance.OnTracking += AssignAudio;
        EventHandler.Instance.OnLostTracking += StopAudio;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnTracking -= AssignAudio;
        EventHandler.Instance.OnLostTracking -= StopAudio;
    }

    // Start is called before the first frame update
    void Start()
    {
        ImageTargetBehaviour = new ImageTargetBehaviour();
           
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableLoadingScreen() 
    {
        LoadingScreen.SetActive(false);
        closeBtn.SetActive(true);
    }

    public void QuitApp() { Application.Quit(); }

    public void AssignAudio()
    {
        //Debug.Log("Target: " + DefaultTrackableEventHandler.myString);
        //switch (DefaultTrackableEventHandler.name)
        //{
        //    case "Horse2020":
        //        AudioSource.clip = audioSources[0];
        //        AudioHandler();
        //        break;
        //    case "Rabbit2020":
        //        AudioSource.clip = audioSources[1];
        //        AudioHandler();
        //        break;
        //}
    }

    public void AudioHandler()
    {
        //if (isAudioPlaying)
        //{
        //    AudioSource.Play();
        //    isAudioPlaying = false;
        //}
        //else
        //{
        //    AudioSource.Stop();
        //    isAudioPlaying = true;
        //}

        AudioSource.Play();
    }

    void StopAudio() { AudioSource.Stop(); }

        
}
