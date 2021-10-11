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
    private SerializableDictionaryBase<EnemyBattlerNames, GameObject> _enemyBattlerLookupDictionary;

    public GameObject GetEnemyBattlerGameObject(EnemyBattlerNames battlerName, out Battler theBattlerForThePrefab)
    {

        _enemyBattlerLookupDictionary.TryGetValue(battlerName, out var potentialBattler);
        var instantiatedBattler = Instantiate(potentialBattler);
        theBattlerForThePrefab = potentialBattler.GetComponent<Battler>();
        return instantiatedBattler;

    }

}
