using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This state should load as many things as possible at the beginning of the battle while the screen is black to reduce loading during the battle.
/// </summary>
public class BattleLoadingState : BattleState
{
    private BattlerClickHandler[] _playerClicks;
    private BattlerClickHandler[] _enemyClicks;

    private void Start()
    {
        SubscribeToGuiFadeEvents();
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
        LoadBattlersClickHandlers();
        StartBattleFadeIn();
    }

    private void InitializeBattlerDamageDisplays(Battler[] allBattlers)
    {
        _battleComponent.BattleGui.BattleNotifications.SpawnDamageTexts();
        SubscribeBattlersToDamageDisplay(allBattlers);
    }

    /// <summary>
    /// Subscribes to the gui event for fading in, so that we can fire a function when it is complete.
    /// </summary>
    private void SubscribeToGuiFadeEvents()
    {
        _battleComponent.BattleGui.BattleFadeInEvent += OnGuiFadeInComplete;
        _battleComponent.BattleGui.BattleFadeOutEvent += OnFadeOutComplete;
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

    /// <summary>
    /// Loads the players and enemies clicks to handle what happens when you click on them.
    /// </summary>
    private void LoadBattlersClickHandlers()
    {
        _playerClicks = InitializeClickArray(_battleComponent.BattleData.PlayerBattlers);
        _enemyClicks = InitializeClickArray(_battleComponent.BattleData.EnemyBattlers);
    }

    /// <summary>
    /// Fills the click handler with data based on the battlers passed in.
    /// </summary>
    /// <param name="battlersToCreateDataWith"></param>
    /// <returns></returns>
    private static BattlerClickHandler[] InitializeClickArray(Battler[] battlersToCreateDataWith)
    {
        var array = new BattlerClickHandler[battlersToCreateDataWith.Length];
        for (var i = 0; i < battlersToCreateDataWith.Length; i++)
        {
            var battler = battlersToCreateDataWith[i];
            if (battler == null) continue;
            var battlerClick = array[i] = battler.BattlerClickHandler;
            battlerClick._battleButtonBroadcaster.ButtonPressedEvent +=
                GenerateButtonPressedFunction(battler);
            battlerClick._battleButtonBroadcaster.ButtonHoveredEvent += GenerateButtonHoveredFunction(battler);
            battlerClick._battleButtonBroadcaster.ButtonHoveredLeaveEvent +=
                GenerateButtonHoverLeaveFunction(battler);
        }
        return array;
    }

    /// <summary>
    /// Generates a function when the battler is clicked on.
    /// </summary>
    /// <param name="battlerClicked"></param>
    /// <returns></returns>
    private static BattleButtonBroadcaster.BattleButtonActionEventHandler GenerateButtonPressedFunction(Battler battlerClicked)
    {
        return
            (obj, e) =>
                {
                    if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battlerClicked.BattleStats.IsDead)
                        return;
                    _targetBattler = battlerClicked;
                    _battleComponent.BattleGui.ClosePlayerBattleWindow(_currentBattler);
                    _battleComponent.BattleGui.ClosePlayerMagicWindow(_currentBattler);
                };
    }

    /// <summary>
    /// Generates a function for when the player is hovered.
    /// </summary>
    /// <param name="battlerClicked"></param>
    /// <returns></returns>
    private static BattleButtonBroadcaster.BattleButtonActionEventHandler GenerateButtonHoveredFunction(
        Battler battlerClicked)
    {
        return
           (obj, e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battlerClicked.BattleStats.IsDead)
                    return;
                _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification($"{battlerClicked.BattleStats.BattlerDisplayName}");
                battlerClicked.spriteComp.color = Color.yellow;
            };
    }

    /// <summary>
    /// Generates a function for when the battler isn't hovered anymore.
    /// </summary>
    /// <param name="battlerHovered"></param>
    /// <returns></returns>
    private static BattleButtonBroadcaster.BattleButtonActionEventHandler GenerateButtonHoverLeaveFunction(
        Battler battlerHovered)
    {
        return
           (obj, e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battlerHovered.BattleStats.IsDead)
                    return;
                _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();
                battlerHovered.spriteComp.color = Color.white;
            };
    }

    /// <summary>
    /// Handles what happens when the fade out happens.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
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
