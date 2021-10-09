using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class EnemyBattlerDatabase : MonoBehaviour
{
    [System.Serializable]
    public enum EnemyBattlerNames
    {
        Circle,
        BigCircle
    }

    [SerializeField]
    private SerializableDictionaryBase<EnemyBattlerNames, Battler> _enemyBattlerLookupDictionary;

    public Battler GetEnemyBattler(EnemyBattlerNames battlerName)
    {
        return _enemyBattlerLookupDictionary.TryGetValue(battlerName, out var potentialBattler) ? potentialBattler : null;
    }

}
