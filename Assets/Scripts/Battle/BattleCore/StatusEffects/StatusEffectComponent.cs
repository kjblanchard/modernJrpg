using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatusEffectComponent
{
    public StatusEffectComponent(Battler battlerToReference)
    {
        _battler = battlerToReference;

    }

    private Battler _battler;
    private List<StatusEffect> _statusEffectList = new List<StatusEffect>();
    private List<StatusEffect> _statusEffectListToRemove = new List<StatusEffect>();

    public static bool CheckToApplyStatusEffect(float percentChance)
    {
        var randomNumber = Random.Range(0, 100);
        return randomNumber <= percentChance;
    }
    public void AddStatusEffect(StatusEffectList statusEffectToAdd)
    {
        if (_statusEffectList.Any(x => x.StatusEffectName == statusEffectToAdd))
            return;
        var instantiatedStatusEffect = SpawnStatusEffect(statusEffectToAdd, _battler.BattleStats);
        instantiatedStatusEffect.StatusEffectEndEvent += OnStatusEffectEnd;
        _statusEffectList.Add(instantiatedStatusEffect);
    }

    public void RemoveStaleStatusEffects()
    {
        _statusEffectListToRemove.ForEach(x =>
        {
            _statusEffectList.Remove(x);
        });
        _statusEffectListToRemove.Clear();
    }

    public float ApplyBeforeDamageStatusEffects(int beginningDamage)
    {
        var newDamage = beginningDamage;
        _statusEffectList.ForEach(statusEffect =>
        {
            newDamage = Mathf.RoundToInt(statusEffect.BeforeDamageTaken(newDamage));
        });
        return newDamage;
    }

    public void ApplyAllPlayerEndStateStatus()
    {
        _statusEffectList.ForEach(statusEffect =>
        {
            statusEffect.PlayerEnd();
        });
    }

    public static StatusEffect SpawnStatusEffect(StatusEffectList statusEffectToSpawn, BattleStats battleStats)
    {
        return statusEffectToSpawn switch
        {
            StatusEffectList.Default => null,
            StatusEffectList.Poison => new Poison(battleStats),
            StatusEffectList.Defend => new Defend(battleStats),
            _ => throw new ArgumentOutOfRangeException(nameof(statusEffectToSpawn), statusEffectToSpawn, null)
        };
    }

    private void OnStatusEffectEnd(StatusEffect status)
    {
        _statusEffectListToRemove.Add(status);
        status.StatusEffectEndEvent -= OnStatusEffectEnd;
    }
}



public enum StatusEffectList
{
    Default,
    Poison,
    Defend,
}
