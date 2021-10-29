using UnityEngine;

public class GetBig : StatusEffect
{
    private const byte _statusTurnLength = 4;
    public GetBig(Battler battler) : base(battler, _statusTurnLength)
    {
        StatusEffectStatModifiers.Str = 3;
    }

    
}
