
public class PlayerTurnState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {
        IsCurrentBattlerAttacking = false;
        IsCurrentBattlerDefending = false;
        var battleWindow = _battleComponent.BattleGui.GetPlayerWindow(_currentBattler);
        var magicMenu = _battleComponent.BattleGui.GetMagicWindow(_currentBattler);
        magicMenu.CheckForSufficientMana(_currentBattler.BattleStats.BattlerCurrentMp);
        _currentBattler.StatusEffectComponent.ApplyAllPlayerStartStateStatus();
        battleWindow.OpenPlayerWindow();
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
