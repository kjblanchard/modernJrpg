using System;
using DG.Tweening;
using UnityEngine;

public class BattlePlayerWindow : MonoBehaviour
{

    //Tween names
    private const string _playerWindowOpenScale = "player1WindowOpenScaleTween";
    private const string _playerWindowOpenMove = "player1WindowOpenMoveTween";
    private const string _playerWindowOpenRotate = "player1WindowOpenRotateTween";

    /// <summary>
    /// The button that will be attached to the attack command
    /// </summary>
    private const int _attackButtonNum = 0;
    /// <summary>
    /// The button attached to the magic command
    /// </summary>
    private const int _magicButtonNum = 1;
    /// <summary>
    /// The button attached to the defend button
    /// </summary>
    private const int _defendButtonNum = 2;

    /// <summary>
    /// This is for knowing when the tweens have finished playing for opening and closing the windows
    /// </summary>
    [SerializeField] private DotweenBroadcasterComponent _playerWindowOpenBroadcaster;

    /// <summary>
    /// Holder for all of the battle buttons
    /// </summary>
    [SerializeField] private BattleButton[] _battleButtons;

    /// <summary>
    /// For handling state changes on button presses
    /// </summary>
    [SerializeField] private BattleStateMachine _battleStateMachine;


    private void Awake()
    {
        SubscribeToPlayerWindowTweenEvents();
        SubscribeToAttackButtonEvents();
        SubscribeToMagicButtonEvents();
        SubscribeToDefendButtonEvents();
    }

    private void SubscribeToDefendButtonEvents()
    {
        _battleButtons[_defendButtonNum].BattleButtonBroadcaster.ButtonPressedEvent += OnPlayerDefendButtonPress;
    }

    private void SubscribeToMagicButtonEvents()
    {
        _battleButtons[_magicButtonNum].BattleButtonBroadcaster.ButtonPressedEvent += OnPlayerMagicButtonPress;
    }

    private void SubscribeToAttackButtonEvents()
    {
        _battleButtons[_attackButtonNum].BattleButtonBroadcaster.ButtonPressedEvent += OnPlayerAttackButtonPress;
        _battleButtons[_attackButtonNum].BattleButtonBroadcaster.ButtonHoveredEvent += OnPlayerAttackButtonHover;
    }

    private void SubscribeToPlayerWindowTweenEvents()
    {
        _playerWindowOpenBroadcaster.DotweenCompleteEvent += OnPlayerDotweenWindowOpenComplete;
        _playerWindowOpenBroadcaster.DotweenRewindCompleteEvent += OnPlayerWindowCloseComplete;
    }

    /// <summary>
    /// Starts all the tweens to open the window with an animation
    /// </summary>
    public void OpenPlayerWindow()
    {
        DOTween.Restart(_playerWindowOpenMove);
        DOTween.Restart(_playerWindowOpenScale);
        DOTween.Restart(_playerWindowOpenRotate);
    }

    /// <summary>
    /// Starts all the tweens in reverse to close the window with an animation
    /// </summary>
    public void ClosePlayerWindow()
    {
        DOTween.PlayBackwards(_playerWindowOpenRotate);
        DOTween.PlayBackwards(_playerWindowOpenScale);
        DOTween.PlayBackwards(_playerWindowOpenMove);
    }

    private void OnPlayerDotweenWindowOpenComplete(object obj, EventArgs e)
    {

    }

    private void OnPlayerWindowCloseComplete(object obj, EventArgs e)
    {
        _battleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.ActionPerformState);


    }
    private void OnPlayerAttackButtonHover(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;

    }

    private void OnPlayerAttackButtonPress(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;
        _battleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.PlayerTargetingState);

    }
    private void OnPlayerMagicButtonPress(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;

    }
    private void OnPlayerDefendButtonPress(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;

    }

}
