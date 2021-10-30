

public class PlayerTurnState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {
        IsCurrentBattlerAttacking = false;
        IsCurrentBattlerDefending = false;
        //if (CheckToSeeIfWeAreComingBackIntoThisState(startupBools)) return;
        _battleComponent.BattleGui.Player1Window.OpenPlayerWindow();
        //Grey out magics that are unusable.
        var battlerNum = _currentBattler.BattleStats.BattlerNumber;
        var magicMenu = battlerNum switch

        {
            0 => _battleComponent.BattleGui.Player1MagicWindow,
            1 => _battleComponent.BattleGui.Player2MagicWindow,
            2 => _battleComponent.BattleGui.Player3MagicWindow,
            _ => null
        };
        if (magicMenu is null)
        {
            DebugLogger.SendDebugMessage($"The magic menu is null for some reason");
            return;
        }
        magicMenu.CheckForSufficientMana(_currentBattler.BattleStats.BattlerCurrentMp);
        _currentBattler.StatusEffectComponent.ApplyAllPlayerStartStateStatus();



    }

    private static bool CheckToSeeIfWeAreComingBackIntoThisState(bool[] startupBools)
    {
        return startupBools != null;
    }


    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }

}
