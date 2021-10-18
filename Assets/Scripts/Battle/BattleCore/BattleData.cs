using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleData : MonoBehaviour
{

    public Battler[] EnemyBattlers { get; private set; }
    public Battler[] PlayerBattlers { get; private set; }

    public Battler[] AllBattlers => EnemyBattlers.Concat(PlayerBattlers).Where(x => x != null).ToArray();


    [SerializeField] private BattlerDatabase _battlerDatabase;
    [SerializeField] private SpawnLocations _spawnLocations;

    private PersistantData.BattleData _battleData;


    /// <summary>
    /// Sets the battle data from the persistent data from the overworld.
    /// </summary>
    /// <param name="newData"></param>
    public void SetBattleData(PersistantData.BattleData newData)
    {
        _battleData = newData;
    }

    /// <summary>
    /// Instantiates the player battlers and the enemy battlers based on battle data
    /// </summary>
    public void ConfigureAllBattlers()
    {
        EnemyBattlers = BattlerLoader.LoadEnemyBattlers(_battleData.BattleEncounter.BattleGroups.EnemyBattlers,_spawnLocations,_battlerDatabase);
        PlayerBattlers = BattlerLoader.LoadPlayerBattlers(_battleData.PlayerBattlers,_spawnLocations,_battlerDatabase);

    }



}
