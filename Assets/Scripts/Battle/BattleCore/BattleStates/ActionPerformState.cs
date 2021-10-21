using UnityEngine;

public class ActionPerformState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {
        var damageToCause = _currentBattler.BattlerDamageComponent.GiveDamage(_targetBattler.BattleStats);
        _targetBattler.BattlerDamageComponent.TakeDamage(damageToCause);

        _battleComponent.BattleGui.Player1Window.ClosePlayerWindow();
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
