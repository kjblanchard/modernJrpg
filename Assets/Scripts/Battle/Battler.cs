using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Battler : MonoBehaviour

{
    /// <summary>
    /// Gets the gameobject that is attached to this battler.
    /// </summary>
    public GameObject BattlerGameObject => gameObject;

    public string GetNameToDisplayInBattle => string.IsNullOrEmpty(BattlerDisplayName) ? BattlerStats.BattlerName : BattlerDisplayName;


    /// <summary>
    /// The battlers stats
    /// </summary>
    [SerializeField] public BattlerStats BattlerStats;
    /// <summary>
    /// The sprite attached to this battler
    /// </summary>
    [SerializeField] public SpriteRenderer BattlerSprite;

    /// <summary>
    /// Gets the Confirmed turns from the battlers clock
    /// </summary>
    public float[] CurrentTurns => _battlerClock.Next20Turns;
    /// <summary>
    /// Gets the last generated turns from the battlers clock
    /// </summary>
    public float[] PotentialTurns => _battlerClock.PotentialNext20Turns;

    /// <summary>
    /// Used for enemies if there is more than one
    /// </summary>
    public string BattlerDisplayName = null;
    public Guid BattlerGuid;

    private BattlerClock _battlerClock;

    private void Awake()
    {
        _battlerClock = new BattlerClock();
        BattlerGuid = Guid.NewGuid();
    }



    public void ConfirmTurn()
    {
        _battlerClock.ConfirmTurn();
    }


    public float[] CalculatePotentialNext20Turns(float skillModifier = 1.0f, bool firstTurn = false)
    {

        return _battlerClock.CalculatePotentialTurns(BattlerStats, skillModifier, firstTurn);
    }





}
