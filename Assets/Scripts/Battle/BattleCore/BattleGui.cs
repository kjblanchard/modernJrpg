using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BattleGui : MonoBehaviour
{
    private const string _fadeInTweenId = "fadeIn";
    [SerializeField] private PlayerHud _playerHudComponent;
    [SerializeField] private PlayerHud _enemyHudComponent;
    [SerializeField] private DotweenBroadcasterComponent _fadeInTweenBroadcasterComponent;
    [SerializeField] private TurnOrderGui _mainTurnOrderGui;



    private void Start()
    {
        _fadeInTweenBroadcasterComponent.DotweenCompleteEvent += OnFadeInComplete;

    }

    public void LoadInitialPlayerHud(Battler[] playerBattlers)
    {
        _playerHudComponent.LoadPlayerBattlers(playerBattlers);
        _playerHudComponent.LoadInitialPlayerHudItems();

    }
    public void LoadInitialEnemyHud(Battler[] enemyBattlers)
    {
        _enemyHudComponent.LoadPlayerBattlers(enemyBattlers);
        _enemyHudComponent.LoadInitialPlayerHudItems();

    }

    public void LoadInitialTurnOrder(Battler[] next20TurnBattlers)
    {
        var battlerNames = next20TurnBattlers.ToList().Select(battler => battler.GetNameToDisplayInBattle).ToArray();
        

        _mainTurnOrderGui.InitializeTurnOrderTexts(battlerNames);

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
