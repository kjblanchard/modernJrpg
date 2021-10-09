using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private EnterBattleCamera _enterBattleCamera;
    [SerializeField] private BlurOptimized _blurOptimized;


    /// <summary>
    /// Enables the blur component on this camera, or disables it
    /// </summary>
    /// <param name="shouldBlur"></param>
    public void BlurBackground(bool shouldBlur)
    {
        _blurOptimized.enabled = shouldBlur;

    }
}
