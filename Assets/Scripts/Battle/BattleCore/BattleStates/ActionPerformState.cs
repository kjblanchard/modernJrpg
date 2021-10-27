using UnityEngine;

public class ActionPerformState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {
        _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();

        if (IsCurrentBattlerAttacking)
            _currentAbility = _currentBattler.battlerAttackAbility;
        if (IsCurrentBattlerDefending)
        {
            _currentAbility = _currentBattler.battlerDefendAbility;
            _targetBattler = _currentBattler;
        }

        var damageToCause = Mathf.RoundToInt(_currentBattler.BattlerDamageComponent.GiveDamage(_targetBattler.BattleStats, _currentAbility)) ;
        var damageToCauseAfterTargetStatusEffects =
            _targetBattler.StatusEffectComponent.ApplyBeforeDamageStatusEffects(damageToCause);
        _targetBattler.BattlerDamageComponent.TakeDamage(damageToCauseAfterTargetStatusEffects);
        _currentBattler.BattlerDamageComponent.TakeMpDamage(_currentAbility.MpCost);
        ApplyStatusEffectsOfAbility();
        TickCurrentBattlersStatusEffects();
        _currentBattler.StatusEffectComponent.RemoveStaleStatusEffects();
        _battleComponent.BattleGui.BattleNotifications.EnableSelectATarget(false);
        _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.BetweenTurnState);
        

    }

    private static void ApplyStatusEffectsOfAbility()
    {
        foreach (var _currentAbilityStatusEffect in _currentAbility.StatusEffects)
        {
            var shouldStatusEffectBeApplied =
                StatusEffectComponent.CheckToApplyStatusEffect(_currentAbilityStatusEffect.StatusEffectChance);
            if (shouldStatusEffectBeApplied)
                _targetBattler.StatusEffectComponent.AddStatusEffect(_currentAbilityStatusEffect.StatusEffect);
        }
    }
    private static void TickCurrentBattlersStatusEffects()
    {
        _currentBattler.StatusEffectComponent.ApplyAllPlayerEndStateStatus();
    }


    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        IsCurrentBattlerAttacking = false;
        IsCurrentBattlerDefending = false;
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }

}
