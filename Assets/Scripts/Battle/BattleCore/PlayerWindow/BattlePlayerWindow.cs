using System;
using DG.Tweening;
using UnityEngine;

public class BattlePlayerWindow : MonoBehaviour
{

    public Battler BattlerOwner { get; private set; }

    private const string _player1WindowOpenScale = "player1WindowOpenScaleTween";
    private const string _player1WindowOpenMove = "player1WindowOpenMoveTween";
    private const string _player1WindowOpenRotate = "player1WindowOpenRotateTween";

    [SerializeField] private DotweenBroadcasterComponent _player1WindowOpenBroadcaster;


    void Awake()
    {
        _player1WindowOpenBroadcaster.DotweenCompleteEvent += OnPlayer1DotweenWindowOpenComplete;
        _player1WindowOpenBroadcaster.DotweenRewindCompleteEvent += OnPlayer1DotweenWindowRewindComplete;
    }

    public void OpenPlayer1Window()
    {
        DOTween.Restart(_player1WindowOpenMove);
        DOTween.Restart(_player1WindowOpenScale);
        DOTween.Restart(_player1WindowOpenRotate);
    }

    public void ClosePlayerWindow()
    {
        DOTween.PlayBackwards(_player1WindowOpenRotate);
        DOTween.PlayBackwards(_player1WindowOpenScale);
        DOTween.PlayBackwards(_player1WindowOpenMove);

    }

    public void AssignBattlerToWindow(Battler battlerToOwnThisWindow)
    {
        BattlerOwner = battlerToOwnThisWindow;
    }

    public bool CheckIfOwner(Guid guid)
    {
        if (BattlerOwner is null)
            return false;
        return BattlerOwner.BattlerGuid == guid;
    }


    private void OnPlayer1DotweenWindowOpenComplete(object obj, EventArgs e)
    {
        //ClosePlayerWindow();

    }

    private void OnPlayer1DotweenWindowRewindComplete(object obj, EventArgs e)
    {
        //OpenPlayer1Window();

    }

}
