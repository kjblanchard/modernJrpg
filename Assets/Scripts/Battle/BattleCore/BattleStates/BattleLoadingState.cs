using System;
using System.Collections.Generic;
using System.Linq;

public class BattleLoadingState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {

        SubscribeToGuiFadeInEvent();
        PopulateBattleDataFromPersistentData();
        var allBattlers = InstantiateBattlers();
        //InitializeBattlerStats(allBattlers);
        CorrectDuplicateEnemyNames(allBattlers.ToList());
        CalculateInitialTurnsForBattlers(allBattlers);
        InitializeTurnOrderGui(allBattlers);
        InitializeHuds(_battleComponent.BattleData.PlayerBattlers,_battleComponent.BattleData.EnemyBattlers);
        StartBattleFadeIn();
    }

    /// <summary>
    /// Subscribes to the gui event for fading in, so that we can fire a function when it is complete.
    /// </summary>
    private void SubscribeToGuiFadeInEvent()
    {
        _battleComponent.BattleGui.BattleFadeInEvent += OnGuiFadeInComplete;
    }

    /// <summary>
    /// Grab the data from the persistent data so that we can use it throughout the battle.
    /// </summary>
    private static void PopulateBattleDataFromPersistentData()
    {
        _battleComponent.BattleData.SetBattleData(PersistantData.instance.GetBattleData());
    }

    /// <summary>
    /// Instantiate the battlers in from the battle data
    /// </summary>
    /// <returns>An array of all the battlers that have been loaded in</returns>
    private static Battler[] InstantiateBattlers()
    {
        _battleComponent.BattleData.ConfigureAllBattlers();
        var allBattlers = _battleComponent.BattleData.AllBattlers;
        return allBattlers;
    }
    /// <summary>
    /// Generates the initial stats for all the batters based on their stats.
    /// </summary>
    /// <param name="battlers"></param>
    private static void InitializeBattlerStats(Battler[] battlers)
    {
        CorrectDuplicateEnemyNames(battlers.ToList());
        CalculateInitialTurnsForBattlers(battlers);
    }

    /// <summary>
    /// Checks all the enemies and adds a prefix to their name if they are duplicates
    /// </summary>
    private static void CorrectDuplicateEnemyNames(List<Battler> battlerList)
    {
        var groupsOfDuplicateEnemies = from battler in battlerList
                    group battler by battler.BattleStats.BattlerNameEnum
            into battlerTypes
                    where battlerTypes.Count() > 1
                    select battlerTypes;

        foreach (var group in groupsOfDuplicateEnemies)
        {
            var letterToAppend = 'A';
            foreach (var _battler in group)
            {
                _battler.BattleStats.AddBattlerNamePostFix(letterToAppend.ToString());
                letterToAppend++;
            }
        }

    }

    /// <summary>
    /// Calculates the initial 20 turns for battle and then confirms them
    /// </summary>
    /// <param name="battlers"></param>
    private static void CalculateInitialTurnsForBattlers(Battler[] battlers)
    {
        foreach (var _battler in battlers)
        {
            _battler.BattlerTimeManager.CalculatePotentialNext20Turns(1.0f, true);
            _battler.BattlerTimeManager.ConfirmTurn();

        }
    }

    /// <summary>
    /// Initializes the turn order gui from the turn order
    /// </summary>
    /// <param name="battlers"></param>
    private static void InitializeTurnOrderGui(Battler[] battlers)
    {
        var next20Turns = BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers);
        BattlerClock.ConfirmNext20Battlers();
        _battleComponent.BattleGui.LoadInitialTurnOrder(next20Turns);
    }

    /// <summary>
    /// Initializes both of the Huds from the battlers.
    /// </summary>
    private static void InitializeHuds(Battler[] playerBattlers, Battler[] enemyBattlers)
    {
        _battleComponent.BattleGui.LoadInitialPlayerHud(playerBattlers);
        _battleComponent.BattleGui.LoadInitialEnemyHud(enemyBattlers);
    }


    /// <summary>
    /// Calls into the battle gui and starts the fade in.
    /// </summary>
    private static void StartBattleFadeIn()
    {
        _battleComponent.BattleGui.StartFadeIn();
    }


    /// <summary>
    /// This is used to handle when the fade in is complete to keep the logic here instead of in the battle gui
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void OnGuiFadeInComplete(object obj, EventArgs e)
    {
        bool[] stateBools = {true};
        _battleComponent.ChangeBattleState(BattleStateMachine.BattleStates.BetweenTurnState, stateBools);

    }
    public override void StateUpdate()
    {
    }

    public override void EndState()
    {
    }

    public override void ResetState()
    {
    }
}
