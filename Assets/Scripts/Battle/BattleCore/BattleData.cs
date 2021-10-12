using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{

    public Battler[] EnemyBattlers { get; private set; }
    public Battler[] PlayerBattlers { get; private set; }
    private GameObject[] _enemyPrefabArray;
    private GameObject[] _playerPrefabArray;
    private PersistantData.BattleData _battleData;


    [SerializeField] private BattlerDatabase _battlerDatabase;
    [SerializeField] private SpawnLocations _spawnLocations;


    public void SetBattleData(PersistantData.BattleData newData)
    {
        _battleData = newData;
        EnemyBattlers = LoadEnemyBattlers(_battleData.BattleEncounter.BattleGroups.EnemyBattlers, out _enemyPrefabArray);
        PlayerBattlers = LoadPlayerBattlers(_battleData.PlayerBattlers, out _playerPrefabArray);
    }

    private Battler[] LoadEnemyBattlers(EnemyBattleGroup.EnemyNameToLocation[] enemyBattlerNames, out GameObject[] enemyPrefabs)
    {
        var count = enemyBattlerNames.Length;
        var battlerArray = new Battler[count];
        enemyPrefabs = new GameObject[count];

        for (var i = 0; i < enemyBattlerNames.Length; i++)
        {
            var enemyBattlerInfo = enemyBattlerNames[i];
            var enemySpawnLocation = _spawnLocations.EnemySpawnLocation[enemyBattlerInfo.LocationToSpawnInBattle];
            var _enemyBattlerGameObject = _battlerDatabase.InstantiateBattler(enemyBattlerInfo.EnemyBattler, enemySpawnLocation, out var battler);
            enemyPrefabs[i] = _enemyBattlerGameObject;
            battlerArray[i] = battler;
            Debug.Log($@"The battler that just got spawned is named {battler.BattlerStats.BattlerName} and his hp is {battler.BattlerStats.BattlerHp}, and his strength is {battler.BattlerStats.BattlerStr}");
        }
        return battlerArray;
    }

    private Battler[] LoadPlayerBattlers(PlayerParty.PlayerStatsAndName[] playerStatsAndNames,
        out GameObject[] playerPrefabs)
    {
        var playerBattlerArray = new Battler[3];
        playerPrefabs = new GameObject[3];

        for (int i = 0; i < playerStatsAndNames.Length; i++)
        {
            var playerBattleInfo = playerStatsAndNames[i];
            var playerSpawnLocation = _spawnLocations.PlayerSpawnLocations[i];
            if (playerBattleInfo.Character == BattlerNames.Default)
                break;
            var playerPrefabGameObject = _battlerDatabase.InstantiateBattler(playerBattleInfo.Character,
                playerSpawnLocation, out var battler, playerBattleInfo.BattlerStats);
            playerBattlerArray[i] = battler;
            playerPrefabs[i] = playerPrefabGameObject;

        }

        return playerBattlerArray;
    }

}
