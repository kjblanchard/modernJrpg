using UnityEngine;

/// <summary>
/// This is the battle stats in battle, and this should be modified with stat changes, etc
/// </summary>
public class BattleStats
{
    public BattleStats(BattlerBaseStats battlersStats)
    {
        _theBattlersBaseBaseStats = battlersStats;
    }

    private BattlerBaseStats _theBattlersBaseBaseStats;

    /// <summary>
    /// The battlers name postfixed with the postfix if it exists
    /// </summary>
    public string BattlerDisplayName =>
        (string.IsNullOrWhiteSpace(_battlerNamePostFix)
            ? _theBattlersBaseBaseStats.BattlerName
            : $"{_theBattlersBaseBaseStats.BattlerName} {_battlerNamePostFix}");

    public int BattlerCurrentHp;
    public int BattlerMaxHp => _theBattlersBaseBaseStats.BattlerHp;
    public int BattlerStr => _theBattlersBaseBaseStats.BattlerStr;
    public int BattlerSpd => _theBattlersBaseBaseStats.BattlerSpd;
    public int BattlerLvl => _theBattlersBaseBaseStats.BattlerLvl;
    public BattlerNames BattlerNameEnum => _theBattlersBaseBaseStats.BattlerNameEnum;
    public bool IsPlayer => _theBattlersBaseBaseStats.IsPlayer;

    private string _battlerNamePostFix;


    /// <summary>
    /// This is used to rename enemies due to multiples for their display names;
    /// </summary>
    public void AddBattlerNamePostFix(string thePostFix)
    {
        _battlerNamePostFix = thePostFix;

    }

}
