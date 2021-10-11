using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public Battler[] EnemyBattlers { get; private set; }
    private PersistantData.BattleData _battleData;


    [SerializeField] private EnemyBattlerDatabase _enemyBattlerDatabase;


    public void SetBattleData(PersistantData.BattleData newData)
    {
        _battleData = newData;
        EnemyBattlers = LoadEnemyBattlers(_battleData.BattleEncounter.BattleGroups.EnemyBattlers);
    }

    private Battler[] LoadEnemyBattlers(EnemyBattlerDatabase.EnemyBattlerNames[] enemyBattlerNames)
    {
        var count = enemyBattlerNames.Length;
        var battlerArray = new Battler[count];

        for (var i = 0; i < enemyBattlerNames.Length; i++)
        {
            var battler = _enemyBattlerDatabase.GetEnemyBattler(enemyBattlerNames[i]);
            battlerArray[i] = battler;
            Debug.Log($@"The battler that just got spawned is named {battler.BattlerName} and his hp is {battler.BattlerHp}, and his strength is {battler.BattlerStr}");
        }
        return battlerArray;
    }

}
