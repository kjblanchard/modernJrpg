using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// This state should load as many things as possible at the beginning of the battle while the screen is black to reduce loading during the battle.
/// </summary>
public class BattleLoadingState : BattleState
{
    private void Start()
    {
        SubscribeToGuiFadeInEvent();
        _battleComponent.BattleGui.BattleFadeOutEvent += OnFadeOutComplete;

    }

    public override void StartState(params bool[] startupBools)
    {
        BattleMusicHandler.LoadBattleMusic();
        PopulateBattleDataFromPersistentData();
        var allBattlers = InstantiateBattlers();
        CorrectDuplicateEnemyNames(allBattlers.ToList());
        CalculateInitialTurnsForBattlers(allBattlers);
        var initialTurnOrder = CreateInitialTurnOrder(allBattlers);
        InitializeTurnOrderGui(initialTurnOrder);
        InitializePlayerMagic(_battleComponent.BattleData.PlayerBattlers);
        InitializeGuiHuds(_battleComponent.BattleData.PlayerBattlers, _battleComponent.BattleData.EnemyBattlers);
        InitializeBattlerDamageDisplays(allBattlers);
        InitializeBattlersClickHandlers();
        StartBattleFadeIn();
    }

    private static void InitializeBattlersClickHandlers()
    {
        _battleComponent.BattleStateMachine.GetStateByBattleState<PlayerTargetingState>(BattleStateMachine.BattleStates
            .PlayerTargetingState).LoadBattleClicks();
    }

    private void InitializeBattlerDamageDisplays(Battler[] allBattlers)
    {
        _battleComponent.BattleGui.BattleNotifications.SpawnDamageTexts();
        SubscribeBattlersToDamageDisplay(allBattlers);
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
                _battler.BattleStats.AddBattlerNamePostFix(letterToAppend);
                letterToAppend++;
            }
        }

    }

    /// <summary>
    /// Calculates the initial 20 turns for battle and then confirms them so that there is an initial display
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
    /// Generates and confirms the initial 20 turns of the battle
    /// </summary>
    /// <param name="allBattlers">All battlers that are going to </param>
    /// <returns></returns>
    private static Battler[] CreateInitialTurnOrder(Battler[] allBattlers)
    {
        var next20Turns = BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers);
        BattlerClock.ConfirmNext20Battlers();
        return next20Turns;

    }

    /// <summary>
    /// Initializes the turn order gui from the turn order
    /// </summary>
    /// <param name="battlers"></param>
    private static void InitializeTurnOrderGui(Battler[] turnOrder)
    {
        _battleComponent.BattleGui.LoadTurnOrderIntoGui(turnOrder);
    }

    /// <summary>
    /// Initializes both of the Huds from the battlers.
    /// </summary>
    private static void InitializeGuiHuds(Battler[] playerBattlers, Battler[] enemyBattlers)
    {
        _battleComponent.BattleGui.LoadInitialPlayerHud(playerBattlers);
        _battleComponent.BattleGui.LoadInitialEnemyHud(enemyBattlers);
    }

    private static void InitializePlayerMagic(Battler[] playerBattlers)
    {
        _battleComponent.BattleGui.LoadPlayersMagicIntoWindows(playerBattlers);
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
        bool[] stateBools = { true };
        _battleComponent.BattleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.BetweenTurnState, stateBools);

    }
    private void SubscribeBattlersToDamageDisplay(Battler[] allBattlers)
    {
        foreach (var _allBattler in allBattlers)
        {
            _allBattler.BattlerDamageComponent.DamageCausedEvent += (object obj, int e) =>
            {
                var textToDisplay = _battleComponent.BattleGui.BattleNotifications.GetTmpTextFromQueue();
                textToDisplay.transform.position =
                    Camera.main.WorldToScreenPoint(_allBattler.LocationForDamageDisplay.transform.position);
                textToDisplay.PlayDamage(e.ToString());
                textToDisplay.PutBackInQueue = () =>
                {
                    _battleComponent.BattleGui.BattleNotifications.ReturnDamageTextToQueue(textToDisplay);
                };
            };
        }
    }

    public void OnFadeOutComplete(object obj, EventArgs e)
    {
        BattleMusicHandler.StopBattleWin();
        SceneController.ChangeGameScene(SceneController.GameScenesEnum.DebugRoom);
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
