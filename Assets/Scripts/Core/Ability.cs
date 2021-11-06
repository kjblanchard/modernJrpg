using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stats/Ability", order = 0)]
public class Ability : ScriptableObject
{

    public Guid Id = new();
    public string Name;
    public int MpCost;
    public float AtkPowModifier;
    public int AtkBonusDamage;
    public StatusEffectAndChance[] StatusEffects;
    public string Description;
    public AbilityType Type;
    public AbilityAtkType AttackType;
    public TargetingType TargetType;


    public enum AbilityName
    {
        Default,
        BaseAttack,
        Thunder,
        Defend
    }

    public enum AbilityType
    {
        Default,
        Magic,
        Skill
    }

    public enum AbilityAtkType
    {
        Default,
        Physical,
        Magic,
        Neutral
    }

    public enum TargetingType
    {
        Default,
        Any,
        All,
        AllPlayer,
        AllEnemy,
        Self,
        Random
    }

    [System.Serializable]
    public struct StatusEffectAndChance
    {
        public StatusEffectList StatusEffect;
        public float StatusEffectChance;
    }

}
