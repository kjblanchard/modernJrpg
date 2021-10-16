using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class BattleLoadingState : BattleState
{
    [SerializeField] private HorizontalLayoutGroup hlg;
    public override void StartState(params bool[] startupBools)
    {
        _battleComponent.BattleData.SetBattleData(PersistantData.instance.GetBattleData());
        _battleComponent.BattleData.SetBattlers();
        var allBattlers = _battleComponent.BattleData.AllBattlers;
        CheckForDuplicateEnemies();
        _battleComponent.BattleGui.LoadInitialPlayerHud(_battleComponent.BattleData.PlayerBattlers);
        _battleComponent.BattleGui.LoadInitialEnemyHud(_battleComponent.BattleData.EnemyBattlers);
        CalculateInitialTurnsForBattlers(allBattlers);
        var next20Turns = BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers, true);
        _battleComponent.BattleGui.LoadInitialTurnOrder(next20Turns);
        _battleComponent.BattleGui.StartFadeIn();
        //Move into between turn state at end of fade in

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

    /// <summary>
    /// Calculates the initial 20 turns for battle and then confirms them
    /// </summary>
    /// <param name="battlers"></param>
    private void CalculateInitialTurnsForBattlers(Battler[] battlers)
    {
        foreach (var _battler in battlers)
        {
            _battler.CalculatePotentialNext20Turns(1.0f, true);
            _battler.ConfirmTurn();

        }

    }

    private void CheckForDuplicateEnemies()
    {
        var enemyBattlers = _battleComponent.BattleData.EnemyBattlers.ToList();
        CheckForDuplicatesQuery(enemyBattlers);
        foreach (var _enemyBattler in enemyBattlers)
        {
            Debug.Log($"The enemies renamed name is {_enemyBattler.GetNameToDisplayInBattle}");
        }

    }

    private void CheckForDuplicatesQuery(List<Battler> battlerList)
    {
        var query = from battler in battlerList
                    group battler by battler.BattlerStats.BattlerNameEnum
            into battlerTypes
                    where battlerTypes.Count() > 1
                    select battlerTypes;


        foreach (var group in query)
        {
            var letterToAppend = 'A';
            foreach (var _battler in group)
            {
                _battler.BattlerDisplayName = $"{_battler.BattlerStats.BattlerName} {letterToAppend}";
                letterToAppend++;
            }
        }

    }
}
