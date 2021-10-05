using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Battle/EnemyBattleGroup", order = 2)]
public class EnemyBattleGroup : ScriptableObject
{
    public Battler[] EnemyBattlers;
}
