using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Battle/BattleEncounterZone", order = 3)]

public class BattleEncounterZone : ScriptableObject
{
    public BattleEncounter[] BattleEncounters;

    [System.Serializable]
    public class BattleEncounter
    {
        public EnemyBattleGroup BattleGroups;
        public int EncounterWeight;
        public string LocationForBattle;
        public BattleMusic MusicToPlay;
    }
    [System.Serializable]
    public enum BattleMusic
    {
        Regular,
        Boss,
        Alt
    }

}

