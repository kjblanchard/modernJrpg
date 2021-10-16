using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleGui : MonoBehaviour
{
    private const string _fadeInTweenId = "fadeIn";
    [SerializeField] private PlayerHud _playerHudComponent;
    [SerializeField] private DotweenBroadcasterComponent _fadeInTweenBroadcasterComponent;



    private void Start()
    {
        _fadeInTweenBroadcasterComponent.DotweenCompleteEvent += OnFadeInComplete;

    }

    public void LoadInitialPlayerHud(Battler[] playerBattlers)
    {
        _playerHudComponent.LoadPlayerBattlers(playerBattlers);
        _playerHudComponent.LoadInitialPlayerHudItems();

    }

    public void StartFadeIn()
    {
        DOTween.Play(_fadeInTweenId);
    }

    private void OnFadeInComplete(object obj, EventArgs e)
    {
        Debug.Log("Animation Finished");
    }
}
