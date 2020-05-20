using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Pixelplacement.Singleton<EventManager> {

    public event Action<string> OnTrackingFound;
    public void OnTrackingFoundInvoke(string trackableName)
    {
        OnTrackingFound.Invoke(trackableName);
    }
}
