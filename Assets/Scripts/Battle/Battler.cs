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

    [SerializeField] public BattlerStats BattlerStats;
    [SerializeField] public SpriteRenderer BattlerSprite;

    private BattlerInternalClock _battlerInternalClock;

    private void Awake()
    {
        _battlerInternalClock = new BattlerInternalClock();

    }

    public float[] GetNext20Turns()
    {

        return  _battlerInternalClock.CalculateTurns(BattlerStats, 1, true);
    }



}
