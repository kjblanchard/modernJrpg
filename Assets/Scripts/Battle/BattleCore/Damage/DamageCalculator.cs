using UnityEngine;

public class DamageCalculator 
{

    public static float CalculateAttackDamage(BattleStats attackingBattler, BattleStats defendingBattler, float modifier)
    {
        var atkPow = attackingBattler.BattlerStr * modifier;
        var atkDef = 0.0f;

        return atkPow - atkDef;
    }
}
