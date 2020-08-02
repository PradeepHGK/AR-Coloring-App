using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private GameObject TrackableObject;
    private bool isAnimationSwapped;

    void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += OnTrackingFound;
    }

    void OnDisable()
    {
        EventManager.Instance.OnTrackingFound -= OnTrackingFound;
    }

    private void OnTrackingFound(string trackableName, GameObject trackableObject)
    {
        TrackableObject = trackableObject;
    }

    void Start()
    {
        ScanUIManager.Instance.AnimationPlayBtn.onClick.AddListener(PlayActionAnimation);
        ScanUIManager.Instance.AnimationPauseBtn.onClick.AddListener(PlayActionAnimation);
    }

    private void PlayActionAnimation()
    {
        isAnimationSwapped = !isAnimationSwapped;
        if (isAnimationSwapped)
        {
            TrackableObject.transform.GetChild(0).GetComponent<Animation>().clip = TrackableObject.transform.GetChild(0).GetComponent<Animation>().GetClip("Action");
        }
        else
            TrackableObject.transform.GetChild(0).GetComponent<Animation>().clip = TrackableObject.transform.GetChild(0).GetComponent<Animation>().GetClip("Idle");
    }
}
