using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Battle/BattlerStats", order = 1)]
public class BattlerBaseStats : ScriptableObject
{
    
    public string BattlerName;
    public int BattlerHp;
    public int BattlerMp;
    public int BattlerStr;
    public int BattlerSpd;
    public int BattlerLvl;
    public bool IsPlayer = false;
    public BattlerNames BattlerNameEnum;
    public Ability[] Abilities;
}
