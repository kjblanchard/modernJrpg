using System;
using UnityEngine;

public class Battler : MonoBehaviour

{

    /// <summary>
    /// The battlers battle stats that is used for displaying information and calculating things in battle
    /// </summary>
    [SerializeField] public BattleStats BattleStats { get; private set; }

    /// <summary>
    /// The battlers Time manager, which controls the turns and stuff
    /// </summary>
    public BattlerTimeManager BattlerTimeManager { get; private set; }

    /// <summary>
    /// The battlers GUID, this is generated to decipher between the battlers in game for any reason.  It changes every battle.
    /// </summary>
    public Guid BattlerGuid;

    /// <summary>
    /// The battlers stats that should not be changed, this is assigned here for ENEMIES, so that we can assign their stats.  Probably move these to json eventually
    /// </summary>
    [SerializeField] private BattlerBaseStats _battlerBaseStats;

    /// <summary>
    /// Initializes the battle stats and timekeeper for the battler
    /// </summary>
    public void CreateInitialStats()
    {
        BattleStats = new BattleStats(_battlerBaseStats);
        BattlerGuid = Guid.NewGuid();
        BattlerTimeManager = new BattlerTimeManager(BattleStats);
    }

    /// <summary>
    /// This is used to assign the players base battle stats and should only be used at the beginning of the battle when it is created.
    /// </summary>
    /// <param name="playerBattleStats"></param>
    public void AssignPlayerBaseBattleStats(BattlerBaseStats playerBattleStats)
    {
        //TODO this will need to actually generate the players stats and add it to it.
        _battlerBaseStats = playerBattleStats;
    }




}
