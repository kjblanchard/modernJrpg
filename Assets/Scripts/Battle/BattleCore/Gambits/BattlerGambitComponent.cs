using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattlerGambitComponent : MonoBehaviour
{
    private BattlerGambit[] _battlerGambits;
    public bool isGambitsEnabled = false;
    public BattlerGambitComponent(BattlerGambit[] battlerGambits)
    {
        _battlerGambits = battlerGambits;
    }

    public Tuple<Battler, Ability> ChooseAction(Battler[] enemyBattlers, Battler[] playerBattlers, Battler currentBattler)
    {
        Battler targetBattler;
        Ability ability;

        foreach (var _battlerGambit in _battlerGambits)
        {
            Battler[] targetBattlers = _battlerGambit.ConditionTarget switch
            {
                GambitTarget.Default => throw new Exception("Borked"),
                GambitTarget.Players => playerBattlers,
                GambitTarget.Enemies => enemyBattlers,
                GambitTarget.Self => new[]{currentBattler},
                _ => throw new ArgumentOutOfRangeException()
            };

            foreach (var _targetBattler in targetBattlers)
            {
                List<Battler> livingBattlers;
                int randomNumber;

                switch (_battlerGambit.Condition)
                {
                    case GambitCondition.Default:
                        throw new Exception("Borked");
                    case GambitCondition.HpGreater:
                        if (_targetBattler.BattleStats.BattlerCurrentHp >= _battlerGambit.ConditionValue)
                        {
                            return new Tuple<Battler, Ability>(_targetBattler, _battlerGambit.AbilityToPerform);
                        }
                        break;
                    case GambitCondition.HpLess:
                        break;
                    case GambitCondition.MpGreater:
                        if (currentBattler.BattleStats.BattlerCurrentMp > _battlerGambit.ConditionValue &&
                            CheckIfMpIsAvailable(currentBattler.BattleStats.BattlerCurrentMp,
                                _battlerGambit.AbilityToPerform.MpCost))
                            return new Tuple<Battler, Ability>(_targetBattler, _battlerGambit.AbilityToPerform);
                        continue;
                    case GambitCondition.MpLess:
                        break;
                    case GambitCondition.Random:
                        livingBattlers = targetBattlers.Where(battler =>
                            battler is not null && battler.BattleStats.IsDead == false).ToList();
                        randomNumber = Random.Range(0, livingBattlers.Count() - 1);
                        return new Tuple<Battler, Ability>(livingBattlers[randomNumber],
                            _battlerGambit.AbilityToPerform);
                    case GambitCondition.IsDead:
                        break;
                    case GambitCondition.None:
                        break;
                    case GambitCondition.HpNot100:
                        break;
                    case GambitCondition.LeastHp:
                        livingBattlers = targetBattlers.Where(battler =>
                            battler is not null && battler.BattleStats.IsDead == false).ToList();
                        var potentialTarget = livingBattlers.Where(battler =>
                           battler.BattleStats.BattlerCurrentHp < battler.BattleStats.BattlerMaxHp);
                        switch (potentialTarget.Count())
                        {
                            case 0:
                                randomNumber = Random.Range(0, livingBattlers.Count() - 1);
                                if (CheckIfMpIsAvailable(currentBattler.BattleStats.BattlerCurrentMp, _battlerGambit.AbilityToPerform.MpCost) && CheckConstraint(_battlerGambit.ConstraintCondition, _battlerGambit.ConstraintValue, currentBattler))
                                    return new Tuple<Battler, Ability>(livingBattlers[randomNumber],
                                        _battlerGambit.AbilityToPerform);
                                continue;
                            case 1:
                                if (CheckIfMpIsAvailable(currentBattler.BattleStats.BattlerCurrentMp, _battlerGambit.AbilityToPerform.MpCost) && CheckConstraint(_battlerGambit.ConstraintCondition, _battlerGambit.ConstraintValue, currentBattler))
                                    return new Tuple<Battler, Ability>(potentialTarget.First(),
                                        _battlerGambit.AbilityToPerform);
                                continue;
                            default:
                                var target = potentialTarget
                                    .OrderByDescending(battler => battler.BattleStats.BattlerCurrentHp).First();
                                if (CheckIfMpIsAvailable(currentBattler.BattleStats.BattlerCurrentMp, _battlerGambit.AbilityToPerform.MpCost) && CheckConstraint(_battlerGambit.ConstraintCondition, _battlerGambit.ConstraintValue, currentBattler))
                                    return new Tuple<Battler, Ability>(target, _battlerGambit.AbilityToPerform);
                                continue;
                        }

                    case GambitCondition.StatusEffectNotExist:
                        if (!_targetBattler.StatusEffectComponent.HasStatus(_battlerGambit
                            .StatusEffectToCheckForConstraint) && CheckIfMpIsAvailable(currentBattler.BattleStats.BattlerCurrentMp,_battlerGambit.AbilityToPerform.MpCost) && CheckConstraint(_battlerGambit.ConstraintCondition, _battlerGambit.ConstraintValue, currentBattler))
                            return new Tuple<Battler, Ability>(_targetBattler, _battlerGambit.AbilityToPerform);
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }

        return null;

    }

    private bool CheckConstraint(GambitCondition constraintToCheckFor, int value, Battler constraintTarget)
    {
        return constraintToCheckFor switch
        {
            GambitCondition.Default => true,
            GambitCondition.None => true,
            GambitCondition.HpNot100 => throw new Exception("Not in"),
            GambitCondition.LeastHp => throw new Exception("Not in"),
            GambitCondition.HpGreater => throw new Exception("Not in"),
            GambitCondition.HpLess => throw new Exception("Not in"),
            GambitCondition.MpGreater => constraintTarget.BattleStats.BattlerCurrentMp >= value,
            GambitCondition.MpLess => throw new Exception("Not in"),
            GambitCondition.Random => throw new Exception("Not in"),
            GambitCondition.IsDead => throw new Exception("Not in"),
            GambitCondition.StatusEffectNotExist => throw new Exception("Not in"),
            _ => throw new ArgumentOutOfRangeException(nameof(constraintToCheckFor), constraintToCheckFor, null)
        };
    }

    private bool CheckIfMpIsAvailable(int currentMp, int abilityMpCost) => currentMp >= abilityMpCost;


}

