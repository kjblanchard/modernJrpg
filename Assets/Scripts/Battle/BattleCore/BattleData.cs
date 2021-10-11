using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{

    public Battler[] EnemyBattlers { get; private set; }
    private GameObject[] _enemyPrefabArray;
    private PersistantData.BattleData _battleData;


    [SerializeField] private EnemyBattlerDatabase _enemyBattlerDatabase;


    public void SetBattleData(PersistantData.BattleData newData)
    {
        _battleData = newData;
        EnemyBattlers = LoadEnemyBattlers(_battleData.BattleEncounter.BattleGroups.EnemyBattlers, out _enemyPrefabArray);
    }

    private Battler[] LoadEnemyBattlers(EnemyBattlerDatabase.EnemyBattlerNames[] enemyBattlerNames, out GameObject[] enemyPrefabs)
    {
        var count = enemyBattlerNames.Length;
        var battlerArray = new Battler[count];
        enemyPrefabs = new GameObject[count];

        for (var i = 0; i < enemyBattlerNames.Length; i++)
        {
            var _enemyBattlerGameObject = _enemyBattlerDatabase.GetEnemyBattlerGameObject(enemyBattlerNames[i], out var battler);
            battlerArray[i] = battler;
            Debug.Log($@"The battler that just got spawned is named {battler.BattlerStats.BattlerName} and his hp is {battler.BattlerStats.BattlerHp}, and his strength is {battler.BattlerStats.BattlerStr}");
        }
        return battlerArray;
    }

}
