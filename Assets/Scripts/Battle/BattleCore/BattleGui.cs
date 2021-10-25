using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BattleGui : MonoBehaviour
{
    private const string _fadeInTweenId = "fadeIn";

    public BattlePlayerWindow Player1Window => _playerWindows[0];
    public BattlePlayerWindow Player2Window => _playerWindows[1];
    public BattlePlayerWindow Player3Window => _playerWindows[2];
    public BattleMagicWindow Player1MagicWindow => _playerMagicWindows[0];
    public BattleMagicWindow Player2MagicWindow => _playerMagicWindows[1];
    public BattleMagicWindow Player3MagicWindow => _playerMagicWindows[2];


    public BattleNotificationsGui BattleNotifications => _battleNotificationsGui;

    [SerializeField] private PlayerHud _playerHudComponent;
    [SerializeField] private PlayerHud _enemyHudComponent;
    [SerializeField] private DotweenBroadcasterComponent _fadeInTweenBroadcasterComponent;
    [SerializeField] private TurnOrderGui _mainTurnOrderGui;

    [SerializeField] private BattlePlayerWindow[] _playerWindows;
    [SerializeField] private BattleMagicWindow[] _playerMagicWindows;
    [SerializeField] private BattleNotificationsGui _battleNotificationsGui;


    public event BattleGuiEventHandler BattleFadeInEvent;
    public event BattleGuiEventHandler BattleFadeOutEvent;
    public delegate void BattleGuiEventHandler(object sender, EventArgs e);


    private void Start()
    {
        _fadeInTweenBroadcasterComponent.DotweenCompleteEvent += OnBattleFadeInComplete;
        _fadeInTweenBroadcasterComponent.DotweenRewindCompleteEvent += OnBattleFadeOutComplete;
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
    /// Loads the turns into the turn order gui
    /// </summary>
    /// <param name="next20TurnBattlers"></param>
    public void LoadTurnOrderIntoGui(Battler[] next20TurnBattlers)
    {
        var battlerNames = next20TurnBattlers.ToList().Select(battler => battler.BattleStats.BattlerDisplayName).ToArray();
        _mainTurnOrderGui.InitializeTurnOrderTexts(battlerNames);

    }

    /// <summary>
    /// Loads the player battlers abilities into their respective magic windows.
    /// </summary>
    /// <param name="battlers"></param>
    public void LoadPlayersMagicIntoWindows(Battler[] battlers)
    {
        for (var i = 0; i < battlers.Length; i++)
        {
            var currentBattler = battlers[i];
            if(currentBattler == null)
                continue;
            _playerMagicWindows[i].LoadAbilitiesIntoButtons(currentBattler.BattleStats.Abilities,currentBattler);
        }
    }

    /// <summary>
    /// Starts the fade in that should be played after everything is loaded in the load state.
    /// </summary>
    public void StartFadeIn()
    {
        DOTween.Restart(_fadeInTweenId);
    }

    /// <summary>
    /// Play the fade out at 3x speed so that it goes faster.
    /// </summary>
    public void StartFadeOut()
    {
        DOTween.timeScale = 3.0f;
        DOTween.PlayBackwards(_fadeInTweenId);
    }

    /// <summary>
    /// This event is called when the fadein is complete.  Probably starts the between turn state.  This is chained from the dotween broadcaster event firing, then this fires.
    /// </summary>
    private void OnBattleFadeInComplete(object obj, EventArgs e)
    {
        BattleFadeInEvent?.Invoke(this,EventArgs.Empty);
    }

    /// <summary>
    /// This event is called when the fadeout is complete.  probably switches the scene.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void OnBattleFadeOutComplete(object obj, EventArgs e)
    {
        DOTween.timeScale = 1.0f;
        BattleFadeOutEvent?.Invoke(this,EventArgs.Empty);
    }
}
