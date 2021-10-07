using UnityEngine;

public class PersistantData : MonoBehaviour
{
    public static PersistantData instance;
    private BattleData _battleData;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void UpdateBattleData(BattleData battleData)
    {
        _battleData = battleData;
    }

    public void UpdateBattleData(BattleEncounterZone.BattleEncounter battleEncounter)
    {
        _battleData.BattleEncounter = battleEncounter;
    }
    public void UpdateBattleData(Battler[] battlers)
    {
        _battleData.PlayerBattlers = battlers;
    }






    /// <summary>
    /// Nested class to use for holding information about the battle data
    /// </summary>
    [System.Serializable]
    public class BattleData
    {
        public BattleData(BattleEncounterZone.BattleEncounter nextBattleGroup, Battler[] battlers)
        {
            BattleEncounter = nextBattleGroup;
            PlayerBattlers = battlers;
        }
        public BattleEncounterZone.BattleEncounter BattleEncounter;
        public Battler[] PlayerBattlers;
    }
}
