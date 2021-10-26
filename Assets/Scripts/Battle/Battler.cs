using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

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

    [SerializeField] public Transform LocationForDamageDisplay;


    /// <summary>
    /// The battlers stats that should not be changed, this is assigned here for ENEMIES, so that we can assign their stats.  Probably move these to json eventually
    /// </summary>
    [SerializeField] private BattlerBaseStats _battlerBaseStats;

    [SerializeField] private DOTweenAnimation _deathMoveTween;
    [SerializeField] private DOTweenAnimation _deathColorTween;
    [SerializeField] private DOTweenAnimation _deathFadeTween;


    private void Awake()
    {
        BattleStats = new BattleStats(_battlerBaseStats);
        BattlerTimeManager = new BattlerTimeManager(BattleStats);
        BattlerDamageComponent = new DamageComponent(BattleStats);

        BattlerDamageComponent.DeathCausedEvent += OnDeath;
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
        SoundController.Instance.PlaySfxOneShot(SoundController.Sfx.BattleEnemyDeath);
        _deathMoveTween.DORestart();
        _deathColorTween.DORestart();
        _deathMoveTween.DORestart();

    }

    private IEnumerator WaitForQuick()
    {
        yield return new WaitForSeconds(0.10f);
    }

}
