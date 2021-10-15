using UnityEngine;

public class BattlerInternalClock
{
    
    public float[] Next20Turns { get; private set; } = new float[_turnsToCalculate];

    private const ushort _maxClockValue = 1024;
    private const byte _turnsToCalculate = 20;
    private const byte _initialTurnVariance = 10;

    private readonly float[] _temporaryNext20Turns = new float[_turnsToCalculate];


    public void ConfirmTurn()
    {
        Next20Turns = _temporaryNext20Turns;
    }

    public float[] CalculateTurns(BattlerStats battlerStats, float skillSpeedModifier = 1.0f, bool initialTurn = false)
    {
        var currentSpeed = initialTurn ? Random.Range(0, battlerStats.BattlerLvl + battlerStats.BattlerSpd) * _initialTurnVariance : 0.0f;
        var eachTurnClock = CalculateClock(battlerStats.BattlerLvl,battlerStats.BattlerSpd, skillSpeedModifier);
        _temporaryNext20Turns[0] = eachTurnClock + currentSpeed;
        for (var i = 1; i < _turnsToCalculate; i++)
        {
            _temporaryNext20Turns[i] = _temporaryNext20Turns[0] + eachTurnClock*i;
        }
        return _temporaryNext20Turns;
    }


    private static float CalculateClock(int battlerLevel, int battlerSpeed, float skillSpeedModifier)
    {
        return (_maxClockValue) / (float)(battlerLevel +
                                     battlerSpeed) * skillSpeedModifier;
    }

}
