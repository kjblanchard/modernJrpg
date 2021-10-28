
public class Defend : StatusEffect
{
    public Defend(Battler battler,  BattleStats battlerStatsForReference) : base(battler, battlerStatsForReference, 2)
    {
    }

    
    public override float BeforeDamageTaken(int potentialDamage)
    {
        var newDamage = potentialDamage / 2;
        return newDamage;
    }
}
