
[System.Serializable]
public class BattlerGambit
{
    public GambitTarget ConditionTarget;
    public GambitCondition Condition;
    public int ConditionValue;

    public GambitTarget ConstraintTarget;
    public GambitCondition ConstraintCondition;
    public int ConstraintValue;
    public Ability AbilityToPerform;
    public StatusEffectList StatusEffectToCheckForConstraint;
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
    LeastHp,
    HpGreater,
    HpLess ,
    MpGreater,
    MpLess,
    Random,
    IsDead,
    StatusEffectNotExist,

}

