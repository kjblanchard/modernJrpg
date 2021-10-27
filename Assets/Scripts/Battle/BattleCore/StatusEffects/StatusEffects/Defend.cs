
public class Defend : StatusEffect
{
    public Defend(BattleStats battlerStatsForReference) : base(battlerStatsForReference, 2)
    {
    }

    
    public override float BeforeDamageTaken(int potentialDamage)
    {
        var newDamage = potentialDamage / 2;
        return newDamage;
    }
}
