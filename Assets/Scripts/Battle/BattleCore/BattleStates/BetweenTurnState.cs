using System.Linq;
using UnityEngine;

public class BetweenTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        var next20Turns = BattlerClock.Next20Battlers;
        var nextBattler = _currentBattler is null ? next20Turns[0] : next20Turns[1];
        var timeToSubtract = nextBattler.BattlerTimeManager.CurrentTurns[0];
        var allBattlers = _battleComponent.BattleData.AllBattlers;

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
