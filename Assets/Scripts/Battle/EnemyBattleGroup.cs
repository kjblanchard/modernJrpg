using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A battle group of enemies
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Battle/EnemyBattleGroup", order = 2)]
public class EnemyBattleGroup : ScriptableObject
{
    public EnemyBattlerDatabase.EnemyBattlerNames[] EnemyBattlers;
}
