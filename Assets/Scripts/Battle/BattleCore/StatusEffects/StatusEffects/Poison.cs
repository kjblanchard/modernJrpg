
using Cinemachine;

[System.Serializable]
public class Poison : StatusEffect
{
    public Poison(Battler battler,  BattleStats battlerStatsForReference) : base(battler, battlerStatsForReference, 3)
    {
        
    }


    public override void PlayerStart()
    {
        var damageToGive = _battlerToReference.BattleStats.BattlerMaxHp / 4;
        _battlerToReference.BattlerDamageComponent.TakeDamage(damageToGive);
        base.PlayerStart();
    }
}
