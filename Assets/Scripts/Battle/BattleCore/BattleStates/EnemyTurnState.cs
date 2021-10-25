using System.Collections;
using UnityEngine;

public class EnemyTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        _targetBattler = _battleComponent.BattleData.PlayerBattlers[0];
        _currentAbility = _battleComponent.BattleData.GetAbilityByName(Ability.AbilityName.BaseAttack);
        StartCoroutine(DisplayBattleMessageCo());
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }

    private static IEnumerator DisplayBattleMessageCo()
    {
        _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification($"The enemy {_currentBattler.BattleStats.BattlerDisplayName} attacks {_targetBattler.BattleStats.BattlerDisplayName}");
        yield return new WaitForSeconds(1);
        _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();
        _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.ActionPerformState);
    }

}
