using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{

    [SerializeField] private EnemyBattlerDatabase _enemyBattlerDatabase;

    private EnemyBattleGroup _currentBattleGroup;
    // Start is called before the first frame update
    void Start()
    {
        LoadBattlersDebug();
    }

    private void LoadBattlersDebug()
    {
        var battleData = PersistantData.instance.GetBattleData();
        var battleEnemies = battleData.BattleEncounter.BattleGroups.EnemyBattlers;
        foreach (var _battleEnemy in battleEnemies)
        {
            var battler = _enemyBattlerDatabase.GetEnemyBattler(_battleEnemy);

            Debug.Log(
                $@"The battler is {battler.BattlerName} and his hp is {battler.BattlerHp} and his strength is {battler.BattlerStr}");
        }
    }
}
