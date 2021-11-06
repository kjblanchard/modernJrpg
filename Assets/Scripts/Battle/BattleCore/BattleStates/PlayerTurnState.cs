using System.Collections;
using UnityEngine;

public class PlayerTurnState : BattleState
{
    //public bool isGambitEnabled = true;
    public override void StartState(params bool[] startupBools)
    {
        if (_currentBattler.BattlerGambitComponent.isGambitsEnabled)
        {
            var targetAndAbility = _currentBattler.BattlerGambitComponent.ChooseAction(_battleComponent.BattleData.EnemyBattlers, _battleComponent.BattleData.PlayerBattlers, _currentBattler);
            _targetBattler = targetAndAbility.Item1;
            _currentAbility = targetAndAbility.Item2;
            _currentBattler.StatusEffectComponent.ApplyAllPlayerStartStateStatus();
            StartCoroutine(DisplayBattleMessageCo());
            return;

        }
        IsCurrentBattlerAttacking = false;
        IsCurrentBattlerDefending = false;
        var battleWindow = _battleComponent.BattleGui.GetPlayerWindow(_currentBattler);
        var magicMenu = _battleComponent.BattleGui.GetMagicWindow(_currentBattler);
        magicMenu.CheckForSufficientMana(_currentBattler.BattleStats.BattlerCurrentMp);
        _currentBattler.StatusEffectComponent.ApplyAllPlayerStartStateStatus();
        battleWindow.OpenPlayerWindow();
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }
    private static IEnumerator DisplayBattleMessageCo()
    {
        _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification($"{_currentBattler.BattleStats.BattlerDisplayName} attacks {_targetBattler.BattleStats.BattlerDisplayName} with {_currentAbility.Name}");
        yield return new WaitForSeconds(1);
        _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();
        _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.ActionPerformState);
    }

}
