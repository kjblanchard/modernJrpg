using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Battle/Battler", order = 1)]
public class Battler : ScriptableObject
{
    public string BattlerName;
    public int BattlerHp;
    public int BattlerStr;

}
