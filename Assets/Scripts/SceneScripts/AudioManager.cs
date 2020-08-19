using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private bool _audioplayenable = false;

    void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.loop = true;
        ScanUIManager.Instance.AudioPlayBtn.onClick.AddListener(AudioPlayPause);
        ScanUIManager.Instance.AudioMuteBtn.onClick.AddListener(AudioPlayPause);
    }


    private void OnEnable()
    {
        EventManager.Instance.PlayAudioFromBundle += SetAudioClip;
        EventManager.Instance.OnTrackingFound += OnTrackingFound;
        EventManager.Instance.OnTrackingLost += OnTrackingLost;
    }


    private void OnDisable()
    {
        EventManager.Instance.PlayAudioFromBundle -= SetAudioClip;
        EventManager.Instance.OnTrackingFound -= OnTrackingFound;
        EventManager.Instance.OnTrackingLost -= OnTrackingLost;
    }
    private void SetAudioClip(AudioClip clip)
    {
        AudioSource.clip = clip;
        Debug.Log($"ClipName: {clip.name}");
    }



    private void OnTrackingFound(string arg1, GameObject arg2) => AudioSource.Play();

    private void OnTrackingLost()
    {
        ScanUIManager.Instance.AudioPlayBtn.gameObject.SetActive(true);
        ScanUIManager.Instance.AudioMuteBtn.gameObject.SetActive(false);
        AudioSource.Pause();
    }

    private void AudioPlayPause()
    {
        _audioplayenable = !_audioplayenable;
        ScanUIManager.Instance.AudioPlayBtn.gameObject.SetActive(_audioplayenable);
        ScanUIManager.Instance.AudioMuteBtn.gameObject.SetActive(!_audioplayenable);
        if (_audioplayenable)
        {
            AudioSource.Play();
        }
        else
        {
            AudioSource.Pause();
        }
    }
}
