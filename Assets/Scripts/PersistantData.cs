using UnityEngine;

public class PersistantData : MonoBehaviour
{
    public static PersistantData instance;
    private BattleData _battleData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void UpdateBattleData(BattleData battleData)
    {
        _battleData = battleData;
    }

    public void UpdateBattleData(BattleEncounterZone.BattleEncounter battleEncounter)
    {
        _battleData.BattleEncounter = battleEncounter;
    }
    public void UpdateBattleData(PlayerParty.PlayerStatsAndName[] battlers)
    {
        _battleData.PlayerBattlers = battlers;
    }


    public BattleData GetBattleData()
    {
        return _battleData;

    }






    /// <summary>
    /// Nested class to use for holding information about the battle data
    /// </summary>
    [System.Serializable]
    public class BattleData
    {
        public BattleData(BattleEncounterZone.BattleEncounter nextBattleGroup, PlayerParty.PlayerStatsAndName[] playerParty)
        {
            BattleEncounter = nextBattleGroup;
            PlayerBattlers = playerParty;
        }
        public BattleEncounterZone.BattleEncounter BattleEncounter;
        public PlayerParty.PlayerStatsAndName[] PlayerBattlers;
    }
}
