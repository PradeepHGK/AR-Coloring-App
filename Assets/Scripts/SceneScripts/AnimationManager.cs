using Pixelplacement;
using System;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    [SerializeField] private GameObject TrackableObject;
    [SerializeField] private bool isAnimationSwapped;

    void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += OnTrackingFound;
        EventManager.Instance.OnTrackingLost += OnTrackingLost;
    }


    void OnDisable()
    {
        EventManager.Instance.OnTrackingFound -= OnTrackingFound;
    }

    private void OnTrackingFound(string trackableName, GameObject trackableObject) => TrackableObject = trackableObject;
    
    private void OnTrackingLost()
    {
        isAnimationSwapped = false;
        ScanUIManager.Instance.AnimationPlayBtn.gameObject.SetActive(true);
        ScanUIManager.Instance.AnimationPauseBtn.gameObject.SetActive(false);
    }

    void Start()
    {
        ScanUIManager.Instance.AnimationPlayBtn.onClick.AddListener(PlayActionAnimation);
        ScanUIManager.Instance.AnimationPauseBtn.onClick.AddListener(PlayActionAnimation);
    }

    private void PlayActionAnimation()
    {
        isAnimationSwapped = !isAnimationSwapped;
        ScanUIManager.Instance.AnimationPlayBtn.gameObject.SetActive(!isAnimationSwapped);
        ScanUIManager.Instance.AnimationPauseBtn.gameObject.SetActive(isAnimationSwapped);

        if (isAnimationSwapped)
        {
            TrackableObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Action", true);
        }
        else
        {
            TrackableObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Action", false);
        }
    }
}
