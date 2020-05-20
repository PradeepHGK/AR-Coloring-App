using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public AnimationHandler AnimationHandler;

    public void StopAnimation()
    {
        AnimationHandler.DisableLoadingScreen();
    }
}
