using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BetweenTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        if (CheckForWinCondition()) return;

        var isFirstTurn = CheckForFirstTurn(startupBools);
        var next20Turns = BattlerClock.Next20Battlers;
        var nextBattler = NextBattlerSelection(isFirstTurn, next20Turns);
        var timeToSubtract = CalculateTimeToSubtractForAllBattlers(isFirstTurn, _currentBattler, nextBattler);
        _currentBattler?.BattlerTimeManager.EndTurn();
        var allBattlers = _battleComponent.BattleData.AllBattlers;
        _currentBattler = nextBattler;
        UpdateBattlerClocks(allBattlers, timeToSubtract);
        _battleComponent.BattleGui.LoadInitialTurnOrder(next20Turns);
        _battleComponent.BattleStateMachine.ChangeBattleState(_currentBattler.BattleStats.IsPlayer
            ? BattleStateMachine.BattleStates.PlayerTurnState
            : BattleStateMachine.BattleStates.EnemyTurnState);
    }

    private static void UpdateBattlerClocks(Battler[] allBattlers, float timeToSubtract)
    {
        SubtractTimeFromAllBattlers(allBattlers, timeToSubtract);
        BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers);
        BattlerClock.ConfirmNext20Battlers();
    }

    private static void SubtractTimeFromAllBattlers(Battler[] allBattlers, float timeToSubtract)
    {

        foreach (var _battler in allBattlers)
        {
            _battler.BattlerTimeManager.SubtractBattleTime(timeToSubtract);

            var next5Turns = _battler.BattlerTimeManager.CurrentTurns.Take(5).ToList();
            next5Turns.ForEach(i =>
            {
                Debug.Log($"The battler {_battler.BattleStats.BattlerDisplayName} and his speed time is {i}");
            });
        }
    }

    private float CalculateTimeToSubtractForAllBattlers(bool isFirstTurn, Battler currentBattler, Battler nextBattler)
    {
        if (isFirstTurn)
            return nextBattler.BattlerTimeManager.CurrentTurns[0];
        var isNextBattlerTheCurrentBattler =
            currentBattler?.BattleStats.BattlerGuid == nextBattler.BattleStats.BattlerGuid;
        if (isNextBattlerTheCurrentBattler)
            return nextBattler.BattlerTimeManager.CurrentTurns[1];
        return nextBattler.BattlerTimeManager.CurrentTurns[0];
    }

    private static bool CheckForFirstTurn(IReadOnlyList<bool> startupBools) => startupBools != null && startupBools[0];

    private Battler NextBattlerSelection(bool IsFirstTurn, Battler[] nextBattlers)
    {
        if (IsFirstTurn)
            return nextBattlers[0];

        for (var i = 1; i < nextBattlers.Length; i++)
        {
            if (nextBattlers[i].BattleStats.IsDead)
                continue;
            return nextBattlers[i];
        }

        return IsFirstTurn ? nextBattlers[0] : nextBattlers[1];
    }

    private bool CheckForWinCondition()
    {
        var allEnemies = _battleComponent.BattleData.EnemyBattlers;
        var areAnyEnemiesAlive = allEnemies.Any(_allEnemy => !_allEnemy.BattleStats.IsDead);
        if (areAnyEnemiesAlive) return false;
        _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.BattleEndState);
        return true;
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
