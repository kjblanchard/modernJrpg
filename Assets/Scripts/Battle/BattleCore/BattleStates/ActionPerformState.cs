using UnityEngine;

public class ActionPerformState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {
        _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();

        if (_currentAbility == null)
            _currentAbility = _battleComponent.BattleData.GetAbilityByName(Ability.AbilityName.BaseAttack);
        var damageToCause = _currentBattler.BattlerDamageComponent.GiveDamage(_targetBattler.BattleStats, _currentAbility);
        _targetBattler.BattlerDamageComponent.TakeDamage(damageToCause);
        _battleComponent.BattleGui.BattleNotifications.EnableSelectATarget(false);
        _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.BetweenTurnState);
        

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

}
