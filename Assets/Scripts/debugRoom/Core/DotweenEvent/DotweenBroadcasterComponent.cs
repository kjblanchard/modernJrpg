using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DotweenBroadcasterComponent : MonoBehaviour
{
    [SerializeField] public DOTweenAnimation AnimationToControl;
    public event DotweenCompleteEventHandler DotweenCompleteEvent;
    public event DotweenRewindEventHandler DotweenRewindEvent;

    public delegate void DotweenCompleteEventHandler(object sender, EventArgs e);
    public delegate void DotweenRewindEventHandler(object sender, EventArgs e);

    public void OnDotweenCompleteEvent()
    {
        DotweenCompleteEvent?.Invoke(this,EventArgs.Empty);
    }
    public void OnDotweenRewindEvent()
    {
        DotweenRewindEvent?.Invoke(this,EventArgs.Empty);
    }



}

