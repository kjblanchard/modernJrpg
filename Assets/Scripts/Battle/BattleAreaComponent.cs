using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class BattleAreaComponent : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _polygonCollider2D;
    [SerializeField] private int _maxStepsForEncounter;
    [SerializeField] private BattleEncounterZone _battleEncounterZone;

    private BattleEncounterZone.BattleEncounter _nextEncounter;

    private int _remainingStepsUntilEncounter;

    private void Start()
    {
        _remainingStepsUntilEncounter = CalculateNextBattleSteps();
    }

    public bool UpdateSteps()
    {
        _remainingStepsUntilEncounter--;
        if (_remainingStepsUntilEncounter > 0) return false;
        _remainingStepsUntilEncounter = CalculateNextBattleSteps();
        return true;

    }
    private int CalculateNextBattleSteps()
    {
        var stepCount = Random.Range(_maxStepsForEncounter / 2, _maxStepsForEncounter);
        return stepCount;
    }

    private BattleEncounterZone.BattleEncounter CalculateNextEncounter()
    {
        var totalEncounterWeight = _battleEncounterZone.BattleEncounters.Select(x => x.EncounterWeight).Sum();
        var randomNumber = Random.Range(0, totalEncounterWeight);
        var currentCounter = 0;
        return (from _battleEncounter in _battleEncounterZone.BattleEncounters let groupCount = _battleEncounter.EncounterWeight + currentCounter where randomNumber <= groupCount select _battleEncounter).FirstOrDefault();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player")
            return;
        //var playerBattleComponent = col.GetComponent<PlayerBattleComponent>();
        var player = col.gameObject.GetComponent<Player>();
        if (!player)
            return;
        player.GetPlayerBattleComponent().UpdateBattleArea(this);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player")
            return;
        //var playerBattleComponent = col.GetComponent<PlayerBattleComponent>();
        //if (playerBattleComponent == null)
        //    return;
        var player = col.gameObject.GetComponent<Player>();
        if (!player)
            return;
        //player.PlayerBattleComponent.UpdateBattleArea(this);
        player.GetPlayerBattleComponent().UpdateBattleArea(null);
    }

}

