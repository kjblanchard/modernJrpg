using System;
using UnityEngine;

/// <summary>
/// This is the battle stats in battle, and this should be modified with stat changes, etc
/// </summary>
[System.Serializable]
public class BattleStats
{
    public BattleStats(BattlerBaseStats battlersStats, StatusEffectComponent statusEffectsToReference)
    {
        _theBattlersBaseBaseStats = battlersStats;
        BattlerGuid = Guid.NewGuid();
        BattlerCurrentHp = BattlerMaxHp;
        BattlerCurrentMp = BattlerMaxMp;
        _statusEffectComponent = statusEffectsToReference;
    }
    public BattleStats(BattlerBaseStats battlersStats)
    {
        _theBattlersBaseBaseStats = battlersStats;
        BattlerGuid = Guid.NewGuid();
        BattlerCurrentHp = BattlerMaxHp;
        BattlerCurrentMp = BattlerMaxMp;
    }

    private BattlerBaseStats _theBattlersBaseBaseStats;

    /// <summary>
    /// The battlers name postfixed with the postfix if it exists
    /// </summary>
    public string BattlerDisplayName =>
        (_battlerNamePostFix == 0)
            ? _theBattlersBaseBaseStats.BattlerName
            : $"{_theBattlersBaseBaseStats.BattlerName} {_battlerNamePostFix}";


    public Guid BattlerGuid { get; }
    public int BattlerNumber { get; private set; }

    public int BattlerCurrentHp { get; private set; }
    public int BattlerCurrentMp { get; private set; }
    public int BattlerMaxMp => _theBattlersBaseBaseStats.BattlerMp;
    public int BattlerMaxHp => _theBattlersBaseBaseStats.BattlerHp;
    public int BattlerStr => _theBattlersBaseBaseStats.BattlerStr + _statusEffectComponent.StrModifier;
    public int BattlerSpd => _theBattlersBaseBaseStats.BattlerSpd;
    public int BattlerLvl { get; set; } = 1;
    public int BattlerTotalExp { get; set; } = 0;
    public int BattlerExpToNextLevel => ExpRequiredForNextLevel - BattlerTotalExp;
    public int BattlerCurrentExpThisLevel => BattlerTotalExp - _theBattlersBaseBaseStats.ExpRequired[BattlerLvl - 1];
    public BattlerNames BattlerNameEnum => _theBattlersBaseBaseStats.BattlerNameEnum;
    public bool IsPlayer => _theBattlersBaseBaseStats.IsPlayer;
    public int BattlerMp => 999;
    public Color32 PortraitColor => _theBattlersBaseBaseStats.CharColor;
    public Sprite BattlerPortrait => _theBattlersBaseBaseStats.CharPortrait;
    public Ability[] Abilities => _theBattlersBaseBaseStats.Abilities;
    public int BattlerExpReward => _theBattlersBaseBaseStats.Exp;

    public int ExpRequiredForNextLevel => _theBattlersBaseBaseStats.ExpRequired[BattlerLvl];

    public bool IsDead;
    private char _battlerNamePostFix;
    private StatusEffectComponent _statusEffectComponent;

    /// <summary>
    /// The battlers GUID, this is generated to decipher between the battlers in game for any reason.  It changes every battle.
    /// </summary>


    /// <summary>
    /// This is used to rename enemies due to multiples for their display names;
    /// </summary>
    public void AddBattlerNamePostFix(char thePostFix)
    {
        _battlerNamePostFix += thePostFix;

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
        return BattlerCurrentHp = Mathf.Clamp(BattlerCurrentHp - damageToGive, 0, BattlerMaxHp);
    }

    public int ApplyMpDamage(int damageToGive)
    {

        return BattlerCurrentMp = Mathf.Clamp(BattlerCurrentMp - damageToGive, 0, BattlerMaxMp);
    }

}
