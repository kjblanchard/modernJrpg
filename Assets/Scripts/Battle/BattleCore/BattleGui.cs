using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BattleGui : MonoBehaviour
{
    public BattlePlayerWindow Player1Window => _playerWindows[0];
    public BattlePlayerWindow Player2Window => _playerWindows[1];
    public BattlePlayerWindow Player3Window => _playerWindows[2];

    public BattleNotificationsGui BattleNotifications => _battleNotificationsGui;

    private const string _fadeInTweenId = "fadeIn";
    [SerializeField] private PlayerHud _playerHudComponent;
    [SerializeField] private PlayerHud _enemyHudComponent;
    [SerializeField] private DotweenBroadcasterComponent _fadeInTweenBroadcasterComponent;
    [SerializeField] private TurnOrderGui _mainTurnOrderGui;

    [SerializeField] private BattlePlayerWindow[] _playerWindows;
    [SerializeField] private BattleNotificationsGui _battleNotificationsGui;

    public delegate void BattleGuiEventHandler(object sender, EventArgs e);
    public event BattleGuiEventHandler BattleFadeInEvent;


    private void Start()
    {
        _fadeInTweenBroadcasterComponent.DotweenCompleteEvent += OnFadeInComplete;

    }

    /// <summary>
    /// Loads the battlers into the hud, and populates their stats into it initially for the battle start.
    /// </summary>
    /// <param name="playerBattlers"></param>
    public void LoadInitialPlayerHud(Battler[] playerBattlers)
    {
        _playerHudComponent.LoadBattlersIntoHud(playerBattlers);
        _playerHudComponent.LoadInitialHudItems();

    }
    /// <summary>
    /// Loads the enemy hud for battle start.
    /// </summary>
    /// <param name="enemyBattlers"></param>
    public void LoadInitialEnemyHud(Battler[] enemyBattlers)
    {
        _enemyHudComponent.LoadBattlersIntoHud(enemyBattlers);
        _enemyHudComponent.LoadInitialHudItems();

    }

    /// <summary>
    /// Loads the initial battles turns into the turn order gui
    /// </summary>
    /// <param name="next20TurnBattlers"></param>
    public void LoadInitialTurnOrder(Battler[] next20TurnBattlers)
    {
        var battlerNames = next20TurnBattlers.ToList().Select(battler => battler.BattleStats.BattlerDisplayName).ToArray();
        _mainTurnOrderGui.InitializeTurnOrderTexts(battlerNames);

    }



    /// <summary>
    /// Starts the fade in that should be played after everything is loaded in the load state.
    /// </summary>
    public void StartFadeIn()
    {
        DOTween.Play(_fadeInTweenId);
    }

    /// <summary>
    /// This should put us into the next battlers turn once this is complete, for now just let us know that it is done
    /// </summary>
    private void OnFadeInComplete(object obj, EventArgs e)
    {
        
        OnBattleFadeInComplete();
        
    }
    private void OnBattleFadeInComplete()
    {
        BattleFadeInEvent?.Invoke(this,EventArgs.Empty);
    }
}
