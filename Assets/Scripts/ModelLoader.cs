using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;

public class ModelLoader : Pixelplacement.Singleton<ModelLoader> 
{
    private void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += LoadModel_OnTrackingFound;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTrackingFound -= LoadModel_OnTrackingFound;
    }

    private void LoadModel_OnTrackingFound(string trackableName)
    {
        Debug.LogWarning("trackableName -->" + trackableName);
    }
}
