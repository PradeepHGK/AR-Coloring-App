using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Pixelplacement.Singleton<EventManager> {

    public event Action<string, GameObject> OnTrackingFound;
    public void OnTrackingFoundInvoke(string trackableName, GameObject trackableObject)
    {
        OnTrackingFound.Invoke(trackableName, trackableObject);
    }


    public event Action OnTrackingLost;
    public void OnTrackingLostInvoke()
    {
        OnTrackingLost.Invoke();
    }

    public event Action DownloadAssetbundle;
    public void DownloadAssetbundleInvoke()
    {
        DownloadAssetbundle.Invoke();
    }

    public event Action<AudioClip> PlayAudioFromBundle;
    public void PlayAudioInvoke(AudioClip audioClip) { PlayAudioFromBundle.Invoke(audioClip); }
}
