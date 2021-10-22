using System;
using UnityEngine;

/// <summary>
/// This is the battle stats in battle, and this should be modified with stat changes, etc
/// </summary>
[System.Serializable]
public class BattleStats
{
    public BattleStats(BattlerBaseStats battlersStats)
    {
        _theBattlersBaseBaseStats = battlersStats;
        BattlerGuid = Guid.NewGuid();
        BattlerCurrentHp = BattlerMaxHp;
    }

    private BattlerBaseStats _theBattlersBaseBaseStats;

    /// <summary>
    /// The battlers name postfixed with the postfix if it exists
    /// </summary>
    public string BattlerDisplayName =>
        (string.IsNullOrWhiteSpace(_battlerNamePostFix)
            ? _theBattlersBaseBaseStats.BattlerName
            : $"{_theBattlersBaseBaseStats.BattlerName} {_battlerNamePostFix}");


    public Guid BattlerGuid { get; }
    public int BattlerNumber { get; private set; }

    public int BattlerCurrentHp { get; private set; }
    public int BattlerMaxHp => _theBattlersBaseBaseStats.BattlerHp;
    public int BattlerStr => _theBattlersBaseBaseStats.BattlerStr;
    public int BattlerSpd => _theBattlersBaseBaseStats.BattlerSpd;
    public int BattlerLvl => _theBattlersBaseBaseStats.BattlerLvl;
    public BattlerNames BattlerNameEnum => _theBattlersBaseBaseStats.BattlerNameEnum;
    public bool IsPlayer => _theBattlersBaseBaseStats.IsPlayer;

    public bool IsDead;
    private string _battlerNamePostFix;

    /// <summary>
    /// The battlers GUID, this is generated to decipher between the battlers in game for any reason.  It changes every battle.
    /// </summary>


    /// <summary>
    /// This is used to rename enemies due to multiples for their display names;
    /// </summary>
    public void AddBattlerNamePostFix(string thePostFix)
    {
        _battlerNamePostFix = thePostFix;

    }

    public void AddBattlerNumber(int battlerNum)
    {
        BattlerNumber = battlerNum;

    }

    /// <summary>
    /// Returns true if the battler is still alive
    /// </summary>
    /// <param name="damageToGive"></param>
    /// <returns></returns>
    public int ApplyDamage(int damageToGive)
    {
        return BattlerCurrentHp = Mathf.Clamp(BattlerCurrentHp - damageToGive ,0, BattlerMaxHp);
    }

}
