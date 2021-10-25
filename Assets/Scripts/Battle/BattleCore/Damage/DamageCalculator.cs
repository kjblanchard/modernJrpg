using UnityEngine;

public class DamageCalculator 
{

    /// <summary>
    /// Calculates the damage that should be done to the defending battler from the attacking battlers ability
    /// </summary>
    /// <param name="attackingBattler">The attacking battler</param>
    /// <param name="defendingBattler">The defending battler</param>
    /// <param name="ability">The ability being used</param>
    /// <returns>The damage that is going to be done</returns>
    public static float CalculateAttackDamage(BattleStats attackingBattler, BattleStats defendingBattler, Ability ability)
    {
        var atkPow = (attackingBattler.BattlerStr+ability.AtkBonusDamage) * ability.AtkPowModifier;
        var atkDef = 0.0f;

        return atkPow - atkDef;
    }
}
