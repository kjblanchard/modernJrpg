using System;
using UnityEngine;

public class AnimationCompleteComponent : MonoBehaviour
{
    public event AnimationCompleteEventHandler AnimationCompleteEvent;
    public delegate void AnimationCompleteEventHandler(object sender, EventArgs e);
    public void OnAnimationCompleteEvent()
    {
        Debug.Log("Just tried firing the event");
        AnimationCompleteEvent?.Invoke(this, EventArgs.Empty);
    }
}
