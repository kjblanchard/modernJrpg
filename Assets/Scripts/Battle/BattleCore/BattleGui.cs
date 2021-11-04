using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleGui : MonoBehaviour
{
    /// <summary>
    /// Tells if the gui is loading or not, useful for polling for animations and stuff.  This is powered by the events below and should subscribe to them.
    /// </summary>
    public static bool IsAnimationPlaying => _loadingDictionary.Count > 0;
    public BattlePlayerWindow GetPlayerWindow(Battler battler) => _playerWindows[battler.BattleStats.BattlerNumber];
    public BattleMagicWindow GetMagicWindow(Battler battler) => _playerMagicWindows[battler.BattleStats.BattlerNumber];

    public BattleNotificationsGui BattleNotifications => _battleNotificationsGui;
    public PlayerHud PlayerHud => _playerHudComponent;
    public PlayerHud EnemyHud => _enemyHudComponent;
    public TurnOrderGui TurnOrder => _mainTurnOrderGui;
    public BattleGuiTransitionComponent BattleTransitionComponent => _battleGuiTransitionComponent;
    public BattleRewardScreen BattleRewardScreen => _battleRewardScreen;

    private static readonly Dictionary<Guid, object> _loadingDictionary = new();
    [SerializeField] private PlayerHud _playerHudComponent;
    [SerializeField] private PlayerHud _enemyHudComponent;
    [SerializeField] private TurnOrderGui _mainTurnOrderGui;
    [SerializeField] private BattlePlayerWindow[] _playerWindows;
    [SerializeField] private BattleMagicWindow[] _playerMagicWindows;
    [SerializeField] private BattleNotificationsGui _battleNotificationsGui;
    [SerializeField] private BattleGuiTransitionComponent _battleGuiTransitionComponent;
    [SerializeField] private BattleRewardScreen _battleRewardScreen;

    public void DisableAllCanvasForRewardScreen()
    {
        _playerHudComponent.DisplayCanvas(false);
        _enemyHudComponent.DisplayCanvas(false);
        _mainTurnOrderGui.DisplayCanvas(false);
        foreach (var _battlePlayerWindow in _playerWindows)
        {
            _battlePlayerWindow?.DisplayCanvas(false);
        }
        foreach (var _playerMagicWindow in _playerMagicWindows)
        {
            _playerMagicWindow?.DisplayCanvas(false);
        }
        _battleNotificationsGui.DisplayCanvas(false);
    }



    private void Awake()
    {
        SubscribeMagicWindowsToGuiLoadingEvents();
    }

    /// <summary>
    /// Subscribes the magic windows to use the loading events so that this will know when we are loading based on the dictionary.
    /// </summary>
    private void SubscribeMagicWindowsToGuiLoadingEvents()
    {
        foreach (var _playerMagicWindow in _playerMagicWindows)
        {
            if (_playerMagicWindow is not null)
                _playerMagicWindow.GuiLoadingEvent += OnGuiLoadingEvent;
        }
    }

    /// <summary>
    /// This is used to handle when a gui component is loading or not, the dictionary is what determines if the scene is loading.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    public void OnGuiLoadingEvent(object obj, GuiLoadingEventArgs e)
    {
        if (e.IsLoading)
            _loadingDictionary.Add(e.Id, obj);
        else
            _loadingDictionary.Remove(e.Id);
    }
}
