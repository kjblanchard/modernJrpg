using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleData : MonoBehaviour
{

    public Battler[] EnemyBattlers { get; private set; }
    public Battler[] PlayerBattlers { get; private set; }

    public Battler[]  AllBattlers => EnemyBattlers.Concat(PlayerBattlers).ToArray();


    [SerializeField] private BattlerDatabase _battlerDatabase;
    [SerializeField] private SpawnLocations _spawnLocations;

    private BattlerLoader _battlerLoader;
    private PersistantData.BattleData _battleData;

    private void Awake()
    {
        _battlerLoader = new BattlerLoader();
    }

    public void SetBattleData(PersistantData.BattleData newData)
    {
        _battleData = newData;
        EnemyBattlers = _battlerLoader.LoadEnemyBattlers(_battleData.BattleEncounter.BattleGroups.EnemyBattlers,_spawnLocations,_battlerDatabase);
        PlayerBattlers = _battlerLoader.LoadPlayerBattlers(_battleData.PlayerBattlers,_spawnLocations,_battlerDatabase);
    }



}
