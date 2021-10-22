using System.Linq;
using UnityEngine;

public class BetweenTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        var allEnemies = _battleComponent.BattleData.EnemyBattlers;
        var areAnyEnemiesAlive = HandleWinScenario(allEnemies);
        if (!areAnyEnemiesAlive)
        {
            _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.BattleEndState);
            return;

        }

        var next20Turns = BattlerClock.Next20Battlers;
        float timeToSubtract = 0;
        Battler nextBattler;
        if (_currentBattler == null)
        {
            //This is the first turn of the battle.
            nextBattler = next20Turns[0];
            timeToSubtract = nextBattler.BattlerTimeManager.CurrentTurns[0];
        }
        else
        {

            if (_currentBattler.BattleStats.IsDead)
            {
                _currentBattler = next20Turns[1];
                _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.BetweenTurnState);

            }
            //This is between the turns
            nextBattler = next20Turns[1];
            //Handle if the next battler is actually the current battler
            timeToSubtract = (_currentBattler.BattleStats.BattlerGuid == nextBattler.BattleStats.BattlerGuid)
                ? nextBattler.BattlerTimeManager.CurrentTurns[1]
                : nextBattler.BattlerTimeManager.CurrentTurns[0];
            //Generate a new turn order based on his next turn
            _currentBattler.BattlerTimeManager.EndTurn();
        }

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

        BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers);
        BattlerClock.ConfirmNext20Battlers();
        var newTurns = BattlerClock.Next20Battlers;

        _battleComponent.BattleGui.LoadInitialTurnOrder(next20Turns);
        _battleComponent.BattleStateMachine.ChangeBattleState(_currentBattler.BattleStats.IsPlayer
            ? BattleStateMachine.BattleStates.PlayerTurnState
            : BattleStateMachine.BattleStates.EnemyTurnState);



    }

    private bool HandleWinScenario(Battler[] allEnemies)
    {
        return allEnemies.Any(_allEnemy => !_allEnemy.BattleStats.IsDead);
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
