using System;
using UnityEngine;

public class DamageComponent
{

    public event DamageCausedEventHandler DamageCausedEvent;

    public event DeathEventHandler DeathCausedEvent;

    public delegate void DamageCausedEventHandler(object sender, int e);
    public delegate void DeathEventHandler(object sender, EventArgs e);


    private BattleStats _battleStatsToReference;

    public DamageComponent(BattleStats battleStatsToReference)

    {
        _battleStatsToReference = battleStatsToReference;

    }

    public float GiveDamage(BattleStats targetBattler, Ability ability)
    {
        return DamageCalculator.CalculateAttackDamage(_battleStatsToReference, targetBattler, ability);

    }

    public void TakeDamage(float damage)
    {
        var damageAmount = Mathf.RoundToInt(damage);
        var newHpAmount = _battleStatsToReference.ApplyDamage(damageAmount);
        if (newHpAmount == 0)
        {
            _battleStatsToReference.IsDead = true;
            OnDeath(this,EventArgs.Empty);
        }
        OnDamageCaused(this,damageAmount);
    }

    public void OnDamageCaused(object sender, int e)
    {
        DamageCausedEvent?.Invoke(this, e);
    }

    public void OnDeath(object sender, EventArgs e)
    {
        DeathCausedEvent?.Invoke(this, e);

    }
}
