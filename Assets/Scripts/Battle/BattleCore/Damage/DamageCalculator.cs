using UnityEngine;

public class DamageCalculator 
{

    public static float CalculateAttackDamage(BattleStats attackingBattler, BattleStats defendingBattler, Ability ability)
    {
        var atkPow = (attackingBattler.BattlerStr+ability.AtkBonusDamage) * ability.AtkPowModifier;
        var atkDef = 0.0f;

        return atkPow - atkDef;
    }
}
