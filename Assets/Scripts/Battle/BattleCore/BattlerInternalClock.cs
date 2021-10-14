using UnityEngine;

public class BattlerInternalClock
{
    private const ushort _maxClockValue = 2048;
    private const byte _turnsToCalculate = 20;


    public float[] CalculateTurns(BattlerStats battlerStats, float skillSpeedModifier = 1.0f, bool initialTurn = false)
    {
        var clockValues = new ClockValues { CurrentLevel = battlerStats.BattlerLvl, SpeedStat = battlerStats.BattlerSpd };
        clockValues.CurrentSpeed = initialTurn ? Random.Range(0, clockValues.CurrentLevel + clockValues.CurrentSpeed) : 0.0f;
        var futureTurnTimes = new float[_turnsToCalculate];
        var eachTurnClock = CalculateClock(clockValues, skillSpeedModifier);
        futureTurnTimes[0] = eachTurnClock + clockValues.CurrentSpeed;
        for (var i = 1; i < _turnsToCalculate; i++)
        {
            futureTurnTimes[i] = futureTurnTimes[0] + eachTurnClock*i;
        }
        return futureTurnTimes;

    }


    private float CalculateClock(ClockValues clockValuesToCalculateWith, float skillSpeedModifier)
    {
        return (_maxClockValue) / (clockValuesToCalculateWith.CurrentLevel +
                                     clockValuesToCalculateWith.SpeedStat) * skillSpeedModifier;
    }

    public class ClockValues
    {
        public int SpeedStat;
        public int CurrentLevel;
        public float CurrentSpeed;
    }
}
