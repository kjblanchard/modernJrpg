using System;
using System.Linq;
using Random = UnityEngine.Random;

public class BattlerGambitComponent 
{
    private BattlerGambit[] _battlerGambits;
    public bool isGambitsEnabled = false;
    public BattlerGambitComponent(BattlerGambit[] battlerGambits)
    {
        _battlerGambits = battlerGambits;
    }

    public Tuple<Battler, Ability> ChooseAction(Battler[] enemyBattlers, Battler[] playerBattlers, Battler currentBattler)
    {
        return (
        from battlerGambit in _battlerGambits
        let targetBattlers = GetPotentialTargetsBasedOnGambitTarget(battlerGambit.ConditionTarget, enemyBattlers, playerBattlers, currentBattler)
        let potentialBattler = GetTargetBasedOnGambitCondition(battlerGambit, targetBattlers)
        let abilityToUse = ChooseAbilityFromAbilities(battlerGambit.AbilityToPerform,currentBattler)
        where potentialBattler is not null
        where CheckIfMpIsAvailable(currentBattler.BattleStats.BattlerCurrentMp, abilityToUse.MpCost)
        where CheckConstraint(battlerGambit.ConstraintCondition, battlerGambit.ConstraintValue, GetPotentialTargetsBasedOnGambitTarget(battlerGambit.ConstraintTarget, enemyBattlers, playerBattlers, currentBattler))
        select new Tuple<Battler, Ability>(potentialBattler, abilityToUse)).FirstOrDefault();

    }

    private Ability ChooseAbilityFromAbilities(AbilityAndWeight[] abilitiesForThisGambit, Battler currentBattler)
    {
        if (abilitiesForThisGambit.Length == 1)
            return abilitiesForThisGambit[0].Ability;
        var usableAbilityList =
            abilitiesForThisGambit.Where(ability => ability.Ability.MpCost <= currentBattler.BattleStats.BattlerCurrentMp).OrderByDescending(ability => ability.Weight);
        if (usableAbilityList is null)
            throw new Exception("You need an ability in the list that can be used at all times");

        var totalAbilityWeight = usableAbilityList.Sum(ability => ability.Weight);
        var randomNumber = Random.Range(0, totalAbilityWeight);
        var currentCounter = 0;
        foreach (var _abilityAndWeight in usableAbilityList)
        {
            var totalWeight = currentCounter + _abilityAndWeight.Weight;
            if (randomNumber <= totalWeight)
                return _abilityAndWeight.Ability;
            currentCounter += _abilityAndWeight.Weight;
        }

        throw new Exception("Something happened here and you didn't match in the foreach");
    }
    private Battler[] GetPotentialTargetsBasedOnGambitTarget(GambitTarget targetToGet, Battler[] enemyBattlers, Battler[] playerBattlers, Battler currentBattler)
    {
        return targetToGet switch
        {
            GambitTarget.Default => null,
            GambitTarget.Players => playerBattlers,
            GambitTarget.Enemies => enemyBattlers,
            GambitTarget.Self => new[] { currentBattler },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Battler GetTargetBasedOnGambitCondition(BattlerGambit battlerGambit, Battler[] conditionTarget)
    {
        return battlerGambit.Condition switch
        {
            GambitCondition.Default => null,
            GambitCondition.None => null,
            GambitCondition.HpNot100 => null,
            GambitCondition.LeastHpPercent => LeastHp(conditionTarget),
            GambitCondition.HpGreater => CheckHpGreater(conditionTarget, battlerGambit.ConditionValue),
            GambitCondition.HpLess => CheckHpLess(conditionTarget, battlerGambit.ConditionValue),
            GambitCondition.MpGreater => null,
            GambitCondition.MpLess => null,
            GambitCondition.Random => CheckRandom(conditionTarget, battlerGambit.ConditionValue),
            GambitCondition.IsDead => null,
            GambitCondition.StatusEffectNotExist => CheckStatusEffectNotExist(conditionTarget, battlerGambit.StatusEffectToCheckForConstraint),
            GambitCondition.SingleTarget => CheckSingleTarget(conditionTarget),
            GambitCondition.TargetNumGreaterThan => null,
            _ => throw new ArgumentOutOfRangeException("Ugh")
        };
    }

    private Battler CheckHpLess(Battler[] battlersToCheck, int value)
    {
        return battlersToCheck
            .FirstOrDefault(battler => !battler.BattleStats.IsDead && battler.BattleStats.BattlerCurrentHp <= value);
    }

    private Battler CheckSingleTarget(Battler[] battlersToCheck)
    {
        return battlersToCheck.Length == 1 ? battlersToCheck.FirstOrDefault() : null;
    }

    

    /// <summary>
    /// Checks the array of battlers based on the value thrown in.  Returns enemies HP percent that is greater than the value, and orders them by the lowest and returns that one.
    /// </summary>
    /// <param name="battlersToCheck"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private Battler CheckHpGreater(Battler[] battlersToCheck, int value)
    {
        return battlersToCheck
            .Where(battler => !battler.BattleStats.IsDead && battler.BattleStats.BattlerCurrentHp >= value)
            .OrderByDescending(battler => battler.BattleStats.BattlerCurrentHpPercent())
            .FirstOrDefault();
    }

    private Battler CheckRandom(Battler[] battlersToCheck, int value)
    {

        var livingBattlers = battlersToCheck.Where(battler =>
            battler is not null && battler.BattleStats.IsDead == false).ToArray();
        var randomNumber = Random.Range(0, livingBattlers.Count() - 1);
        return livingBattlers[randomNumber];
    }

    private Battler LeastHp(Battler[] battlersToCheck)
    {
        return battlersToCheck.Where(battler => !battler.BattleStats.IsDead)
            .OrderByDescending(battler => battler.BattleStats.BattlerCurrentHpPercent()).FirstOrDefault();
    }

    private Battler CheckStatusEffectNotExist(Battler[] battlersToCheck, StatusEffectList statusToCheckFor)
    {
        return battlersToCheck
            .FirstOrDefault(battler => !battler.StatusEffectComponent.HasStatus(statusToCheckFor));
    }

    private bool CheckConstraint(GambitCondition constraintToCheckFor, int value, Battler[] constraintTarget)
    {
        return constraintToCheckFor switch
        {
            GambitCondition.Default => true,
            GambitCondition.None => true,
            GambitCondition.HpNot100 => throw new Exception("Not in"),
            GambitCondition.LeastHpPercent => throw new Exception("Not in"),
            GambitCondition.HpGreater => throw new Exception("Not in"),
            GambitCondition.HpLess => throw new Exception("Not in"),
            GambitCondition.MpGreater => constraintTarget.Any(battler => battler.BattleStats.BattlerCurrentMp >= value),
            GambitCondition.MpLess => throw new Exception("Not in"),
            GambitCondition.Random => throw new Exception("Not in"),
            GambitCondition.IsDead => throw new Exception("Not in"),
            GambitCondition.StatusEffectNotExist => throw new Exception("Not in"),
            GambitCondition.SingleTarget => constraintTarget.Length == 1,
            GambitCondition.TargetNumGreaterThan => constraintTarget.Count(target => !target.BattleStats.IsDead) >= value,
            _ => throw new ArgumentOutOfRangeException(nameof(constraintToCheckFor), constraintToCheckFor, null)
        };
    }

    private bool CheckIfMpIsAvailable(int currentMp, int abilityMpCost) => currentMp >= abilityMpCost;


}

