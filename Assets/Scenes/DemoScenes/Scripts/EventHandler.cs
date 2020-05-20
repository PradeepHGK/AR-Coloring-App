using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pixelplacement;

public class EventHandler : Singleton<EventHandler>
{
    public event Action OnTracking;
    public void OnTrackingInvoke() { OnTracking(); }

    public event Action OnLostTracking;
    public void OnLostTrackingInvoke() { OnLostTracking(); }
}
