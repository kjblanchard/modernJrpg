using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Stats/Ability", order = 0)]
public class Ability : ScriptableObject
{

    public Guid Id = new Guid();
    public string Name;
    public int MpCost;
    public float AtkPowModifier;
    public int AtkBonusDamage;
    public AbilityType Type;
    public AbilityAtkType AttackType;


    public enum AbilityName
    {
        Default,
        BaseAttack,
        Thunder
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

}
