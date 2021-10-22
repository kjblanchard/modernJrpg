using System;
using DG.Tweening;
using UnityEngine;

public class BattlePlayerWindow : MonoBehaviour
{


    private const string _playerWindowOpenScale = "player1WindowOpenScaleTween";
    private const string _playerWindowOpenMove = "player1WindowOpenMoveTween";
    private const string _playerWindowOpenRotate = "player1WindowOpenRotateTween";

    private const int _attackButtonNum = 0;
    private const int _magicButtonNum = 1;
    private const int _defendButtonNum = 2;

    [SerializeField] private DotweenBroadcasterComponent _playerWindowOpenBroadcaster;

    [SerializeField] private BattleButton[] _battleButtons;

    private static BattleStateMachine _battleStateMachine;






    void Awake()
    {
        _playerWindowOpenBroadcaster.DotweenCompleteEvent += OnPlayerDotweenWindowOpenComplete;
        _playerWindowOpenBroadcaster.DotweenRewindCompleteEvent += OnPlayerWindowCloseComplete;

        _battleButtons[_attackButtonNum].BattleButtonBroadcaster.ButtonPressedEvent += OnPlayerAttackButtonPress;
        _battleButtons[_attackButtonNum].BattleButtonBroadcaster.ButtonHoveredEvent += OnPlayerAttackButtonHover;

        _battleButtons[_magicButtonNum].BattleButtonBroadcaster.ButtonPressedEvent += OnPlayerMagicButtonPress;
        _battleButtons[_defendButtonNum].BattleButtonBroadcaster.ButtonPressedEvent += OnPlayerDefendButtonPress;

    }

    void Start()
    {
        _battleStateMachine ??= FindObjectOfType<BattleStateMachine>();
    }

    public void OpenPlayerWindow()
    {
        DOTween.Restart(_playerWindowOpenMove);
        DOTween.Restart(_playerWindowOpenScale);
        DOTween.Restart(_playerWindowOpenRotate);
    }

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
        Debug.Log("Attack is hovered!");

    }

    private void OnPlayerAttackButtonPress(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;
        _battleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.PlayerTargetingState);
        //Change state

    }
    private void OnPlayerMagicButtonPress(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;
        Debug.Log("Magic!");

    }
    private void OnPlayerDefendButtonPress(object obj, EventArgs e)
    {
        if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState)
            return;
        Debug.Log("Defend!");

    }

}
