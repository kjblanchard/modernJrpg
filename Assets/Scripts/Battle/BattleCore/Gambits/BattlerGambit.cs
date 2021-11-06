
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

