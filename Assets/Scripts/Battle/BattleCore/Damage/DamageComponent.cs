using System;
using JetBrains.Annotations;
using UnityEngine;

public class DamageComponent
{
    public event DamageCausedEventHandler DamageCausedEvent;

    public delegate void DamageCausedEventHandler(object sender, EventArgs e);


    private BattleStats _battleStatsToReference;

    public DamageComponent(BattleStats battleStatsToReference)

    {
        _battleStatsToReference = battleStatsToReference;

    }

    public float GiveDamage(BattleStats targetBattler)
    {
        return DamageCalculator.CalculateAttackDamage(_battleStatsToReference, targetBattler, 1);

    }

    public void TakeDamage(float damage)
    {
        var newHpAmount = _battleStatsToReference.ApplyDamage(Mathf.RoundToInt(damage));
        Debug.Log($"The new hp is {newHpAmount}");
        OnDamageCaused(this,EventArgs.Empty);
    }

    public void OnDamageCaused(object sender, EventArgs e)
    {
        DamageCausedEvent?.Invoke(this,EventArgs.Empty);
    }
}
