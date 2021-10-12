using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public PlayerStatsAndName[] CurrentParty => _playerStatsAndNames;
    [SerializeField] private PlayerStatsAndName[] _playerStatsAndNames;


    public PlayerStatsAndName GetPlayerStats(int playerNumber)
    {
        return new PlayerStatsAndName {BattlerStats = _playerStatsAndNames[0].BattlerStats, Character = BattlerNames.Kevin};

        //return new BattlerStats
        //{
        //    BattlerName = "hello",
        //    BattlerStr = 1,
        //    BattlerHp = 50
        //};

    }


    [System.Serializable]
    public struct PlayerStatsAndName
    {
        public BattlerStats BattlerStats;
        public BattlerNames Character;

    }
}
