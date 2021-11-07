
[System.Serializable]
public class BattlerGambit
{
    public GambitTarget ConditionTarget;
    public GambitCondition Condition;
    public int ConditionValue;

    public GambitTarget ConstraintTarget;
    public GambitCondition ConstraintCondition;
    public int ConstraintValue;
    public AbilityAndWeight[] AbilityToPerform;
    public StatusEffectList StatusEffectToCheckForConstraint;

    public BattlerGambit(bool isPlayer, Ability battlersAttackAbility)
    {
        ConditionTarget = (isPlayer) ? GambitTarget.Enemies : GambitTarget.Players;
        Condition = GambitCondition.Random;
        AbilityToPerform = new[]
        {
            (new AbilityAndWeight{Ability = battlersAttackAbility, Weight = 100})
        };

    }
}
    [System.Serializable]
    public class AbilityAndWeight
    {
        public Ability Ability;
        public int Weight = 100;
    }

[System.Serializable]
public enum GambitTarget
{
    Default,
    Players,
    Enemies,
    Self,
}

[System.Serializable]
public enum GambitCondition
{
    Default,
    None,
    HpNot100,
    LeastHpPercent,
    HpGreater,
    HpLess ,
    MpGreater,
    MpLess,
    Random,
    IsDead,
    StatusEffectNotExist,
    SingleTarget,
    TargetNumGreaterThan,

}

