using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        Debug.Log("This is the player Turn state");
        _battleComponent.BattleGui.Player1Window.OpenPlayerWindow();
        var battlerNumber = _currentBattler.BattleStats.BattlerNumber;
        Debug.Log($"Your battler number is {battlerNumber}");


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
