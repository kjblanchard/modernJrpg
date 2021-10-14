using UnityEngine;

public class BattlerLoader 
{
    /// <summary>
    /// Loads the enemy battlers by calling into the battler database.
    /// </summary>
    /// <param name="enemyBattlerNames">An array of Enemy names to location number in reference to the scene locations of enemies to spawn</param>
    /// <param name="theSpawnLocations">A reference to the spawn locations on the scene</param>
    /// <param name="theBattlerDatabase">The battler database that holds the dictionary of prefabs to names</param>
    /// <returns></returns>
    public Battler[] LoadEnemyBattlers(EnemyBattleGroup.EnemyNameToLocation[] enemyBattlerNames, SpawnLocations theSpawnLocations, BattlerDatabase theBattlerDatabase)
    {
        var count = enemyBattlerNames.Length;
        var battlerArray = new Battler[count];

        for (var i = 0; i < enemyBattlerNames.Length; i++)
        {
            var enemyBattlerInfo = enemyBattlerNames[i];
            var enemySpawnLocation = theSpawnLocations.EnemySpawnLocation[enemyBattlerInfo.LocationToSpawnInBattle];
            var enemyBattler = theBattlerDatabase.InstantiateBattler(enemyBattlerInfo.EnemyBattler, enemySpawnLocation);
            battlerArray[i] = enemyBattler;
            enemyBattler.GetNext20Turns();
        }
        return battlerArray;
    }
    /// <summary>
    /// Loads the enemy battlers by calling into the battler database.
    /// </summary>
    /// <param name="playerStatsAndNames">An array of Player names to stats number in reference to the scene locations of players to spawn</param>
    /// <param name="theSpawnLocations">A reference to the spawn locations on the scene</param>
    /// <param name="theBattlerDatabase">The battler database that holds the dictionary of prefabs to names</param>
    /// <returns></returns>
    public Battler[] LoadPlayerBattlers(PlayerParty.PlayerStatsAndName[] playerStatsAndNames, SpawnLocations theSpawnLocations, BattlerDatabase theBattlerDatabase)
    {
        var playerBattlerArray = new Battler[3];

        for (var i = 0; i < playerStatsAndNames.Length; i++)
        {
            var playerBattleInfo = playerStatsAndNames[i];
            var playerSpawnLocation = theSpawnLocations.PlayerSpawnLocations[i];
            if (playerBattleInfo.Character == BattlerNames.Default)
                break;
            var battler = theBattlerDatabase.InstantiateBattler(playerBattleInfo.Character,
                playerSpawnLocation, playerBattleInfo.BattlerStats);
            playerBattlerArray[i] = battler;
            battler.GetNext20Turns();
        }

        return playerBattlerArray;
    }
}
