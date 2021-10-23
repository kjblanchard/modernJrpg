using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class Battler : MonoBehaviour

{

    /// <summary>
    /// The battlers battle stats that is used for displaying information and calculating things in battle
    /// </summary>
    [SerializeField] public BattleStats BattleStats { get; private set; }

    public DamageComponent BattlerDamageComponent { get; private set; }

    [SerializeField] public SpriteRenderer spriteComp;

    /// <summary>
    /// The battlers Time manager, which controls the turns and stuff
    /// </summary>
    public BattlerTimeManager BattlerTimeManager { get; private set; }

    [SerializeField]
    public BattlerClickHandler BattlerClickHandler;


    /// <summary>
    /// The battlers stats that should not be changed, this is assigned here for ENEMIES, so that we can assign their stats.  Probably move these to json eventually
    /// </summary>
    [SerializeField] private BattlerBaseStats _battlerBaseStats;
    private void Awake()
    {
        BattleStats = new BattleStats(_battlerBaseStats);
        BattlerTimeManager = new BattlerTimeManager(BattleStats);
        BattlerDamageComponent = new DamageComponent(BattleStats);

        BattlerDamageComponent.DeathCausedEvent += OnDeath;


    }

    private void Start()
    {

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

    public void OnDeath(object obj, EventArgs e)
    {

        spriteComp.color = Color.red;

    }

    //public void OnBattlerClicked(object obj, EventArgs e)
    //{

    //    Debug.Log($"This guy just got clicked, {BattleStats.BattlerDisplayName}");

    //}




}
