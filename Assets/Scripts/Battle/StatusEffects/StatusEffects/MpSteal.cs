
public class MpSteal : StatusEffect
{
    public const int length = 0;
    public MpSteal(Battler battler) : base(battler, length)
    {
        StatusEffectName = StatusEffectList.MpSteal;
    }

    public override void OnDamageGiven(int damageGiven)
    {
        _battlerToReference.BattlerDamageComponent.TakeMpDamage(-damageGiven);
    }
}
