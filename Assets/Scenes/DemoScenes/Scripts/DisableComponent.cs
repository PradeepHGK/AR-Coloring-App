using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableComponent : MonoBehaviour
{
    private Animator animator;
    private AnimationClip AnimationClip;
    private AnimationEvent evt;


    private void Awake()
    {
        //evt = new AnimationEvent();
        //animator = GetComponent<Animator>();

        ////evt.functionName = "DisableObjects";
        ////evt.time = 1.5f;
        //AnimationClip = animator.runtimeAnimatorController.animationClips[0];
        ////AnimationClip.AddEvent(evt);
        
    }

    private void Start()
    {
    }

    public AnimationHandler AnimationHandler;

    public void StopAnimation() { AnimationHandler.DisableLoadingScreen(); }
}
