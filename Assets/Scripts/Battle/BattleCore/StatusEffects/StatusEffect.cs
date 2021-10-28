[System.Serializable]
public abstract class StatusEffect 
{
    public event StatusEffectEndEventArgs StatusEffectEndEvent;
    public delegate void StatusEffectEndEventArgs(StatusEffect obj);

    protected StatusEffect(Battler battler, BattleStats battlerStatsForReference, int initialLength)
    {
        _battlerToReference = battler;
        _battlerStatsToReference = battlerStatsForReference;
        statusInitialLength = initialLength;
        statusTurnsRemaining = statusInitialLength;

    }

    public Battler _battlerToReference;
    public StatModifiers StatusEffectStatModifiers { get; private set; }
    private BattleStats _battlerStatsToReference;

    public StatusEffectList StatusEffectName;

    protected readonly int statusInitialLength;
    protected int statusTurnsRemaining;

    public virtual void StatusEffectStart(){}
    public virtual void PlayerStart(){}

    /// <summary>
    /// Make sure you call this to increment the remaining time and potentially call the event to fire.
    /// </summary>
    public virtual void PlayerEnd()
    {
        statusTurnsRemaining--;
        if(statusTurnsRemaining <=0)
            OnStatusEffectEnd(this);
    }

    public virtual float BeforeDamageTaken(int potentialDamage)
    {
        return potentialDamage;
    }

    public void OnStatusEffectEnd(StatusEffect obj)
    {
        StatusEffectEndEvent?.Invoke(this);
    }



    public class StatModifiers
    {
        public int Hp;
        public int Mp;
        public int Str;
        public int Def;
        public int Spd;
    }
}
