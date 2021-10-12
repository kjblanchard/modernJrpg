using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class BattlerDatabase : MonoBehaviour
{

    [SerializeField]
    private SerializableDictionaryBase<BattlerNames, GameObject> _enemyBattlerLookupDictionary;

    public GameObject InstantiateBattler(BattlerNames battlerName, Transform locationToSpawnAt, out Battler theBattlerForThePrefab, BattlerStats battlerStats = null)
    {
        _enemyBattlerLookupDictionary.TryGetValue(battlerName, out var potentialBattler);
        var instantiatedBattler = Instantiate(potentialBattler, locationToSpawnAt);
        theBattlerForThePrefab = potentialBattler.GetComponent<Battler>();
        if (battlerStats)
            theBattlerForThePrefab.BattlerStats = battlerStats;
        return instantiatedBattler;
    }


}
[System.Serializable]
public enum BattlerNames
{

    Default,
    Kevin,
    Todd,
    Cory,
    Melissa,
    Circle,
    BigCircle
}
