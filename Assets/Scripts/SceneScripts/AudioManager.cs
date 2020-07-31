using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private bool audioplayenable;

    void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        ScanUIManager.Instance.playAudioBtn.onClick.AddListener(AudioPlayPause);
        ScanUIManager.Instance.pauseAudioBtn.onClick.AddListener(AudioPlayPause);
    }

    private void OnEnable() 
    {
        EventManager.Instance.PlayAudioFromBundle += SetAudioClip;
    }

    private void OnDisable() 
    {
        EventManager.Instance.PlayAudioFromBundle -= SetAudioClip;
    }

    private void SetAudioClip(AudioClip clip) { AudioSource.clip = clip; }

    public void AudioPlayPause()
    {
        if (AudioSource.clip != null)
        {
            audioplayenable = !audioplayenable;
            ScanUIManager.Instance.playAudioBtn.gameObject.SetActive(audioplayenable);
            ScanUIManager.Instance.pauseAudioBtn.gameObject.SetActive(!audioplayenable);
            if (audioplayenable)
            {
                AudioSource.Play();
            }
            else
            {
                AudioSource.Pause();
            }
        }
    }

}
