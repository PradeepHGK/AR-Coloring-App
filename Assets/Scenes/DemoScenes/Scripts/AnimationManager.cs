using Pixelplacement;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    [SerializeField] private GameObject TrackableObject;
    [SerializeField] private bool isAnimationSwapped;

    void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += OnTrackingFound;
    }

    void OnDisable()
    {
        EventManager.Instance.OnTrackingFound -= OnTrackingFound;
    }

    private void OnTrackingFound(string trackableName, GameObject trackableObject)=> TrackableObject = trackableObject;

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
