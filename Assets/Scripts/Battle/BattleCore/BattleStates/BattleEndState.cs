using System.Collections;
using UnityEngine;

public class BattleEndState : BattleState
{

    public override void StartState(params bool[] startupBools)
    {
        StartCoroutine(WaitForSecond());
    }

    private IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(1);
        BattleMusicHandler.StopBattleMusic();
        BattleMusicHandler.PlayBattleWin();
        _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification("You are the win!");
    }

    private void OnMouseClick()
    {
        if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum !=
            BattleStateMachine.BattleStates.BattleEndState) return;
        _battleComponent.BattleGui.BattleTransitionComponent.StartFadeOut();
    }

    public override void StateUpdate()
    {
    }

    public override void EndState()
    {
    }

    public override void ResetState()
    {
    }

}
