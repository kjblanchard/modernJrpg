using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Battle/BattlerStats", order = 1)]
public class BattlerStats : ScriptableObject
{
    public string BattlerName;
    public int BattlerHp;
    public int BattlerStr;
    public int BattlerSpd;
    public int BattlerLvl;
    public BattlerNames BattlerNameEnum;
}
