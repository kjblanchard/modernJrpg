using System.Linq;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class BetweenTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        var next20Turns = BattlerClock.Next20Battlers;
        var nextBattler = _currentBattler is null ? next20Turns[0] : next20Turns[1];
        float timeToSubtract = 0;
        if (_currentBattler != null)
            timeToSubtract = (_currentBattler.BattlerTimeManager.CurrentTurns[0] == 0) ? nextBattler.BattlerTimeManager.CurrentTurns[1] : nextBattler.BattlerTimeManager.CurrentTurns[0];
        else
        {
            timeToSubtract = nextBattler.BattlerTimeManager.CurrentTurns[0];
        }
        var allBattlers = _battleComponent.BattleData.AllBattlers;

        //Temporary until switching to queue
        if (_currentBattler != null)
        {
            var newArray = test(_currentBattler.BattlerTimeManager.CurrentTurns);
            _currentBattler.BattlerTimeManager.Testing(newArray);
        }

        foreach (var _battler in allBattlers)
        {
            _battler.BattlerTimeManager.SubtractBattleTime(timeToSubtract);

            var next5Turns = _battler.BattlerTimeManager.CurrentTurns.Take(5).ToList();
            next5Turns.ForEach(i =>
            {
                Debug.Log($"The battler {_battler.BattleStats.BattlerDisplayName} and his speed time is {i}");
            });
        }

        _currentBattler = nextBattler;
        //UpdateTheGui after this has changed.
        _battleComponent.BattleStateMachine.ChangeBattleState(_currentBattler.BattleStats.IsPlayer
            ? BattleStateMachine.BattleStates.PlayerTurnState
            : BattleStateMachine.BattleStates.EnemyTurnState);

        BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers);
        BattlerClock.ConfirmNext20Battlers();
        var newTurns = BattlerClock.Next20Battlers;

        _battleComponent.BattleGui.LoadInitialTurnOrder(next20Turns);


    }
    static float[] test(float[] numbers)
    {
        int size = numbers.Length;
        float[] shiftNums = new float[size];

        for (int i = 0; i < size; i++)
        {
            shiftNums[i] = numbers[(i + 1) % size];
        }
        return shiftNums;
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }
}
