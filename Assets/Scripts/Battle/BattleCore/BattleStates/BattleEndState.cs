using System;

public class BattleEndState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        BattleMusicHandler.StopBattleMusic();
        BattleMusicHandler.PlayBattleWin();
        _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification("You Win!!!");
    }

    private void OnMouseClick()
    {
        if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum !=
            BattleStateMachine.BattleStates.BattleEndState) return;
        _battleComponent.BattleGui.StartFadeOut();
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }
}
