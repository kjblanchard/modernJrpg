using UnityEngine;

public class BetweenTurnState : BattleState
{
    // Start is called before the first frame update
    public override void StartState(params bool[] startupBools)
    {
        Debug.Log("In the between turn state");
        _battleComponent.BattleGui.Player1Window.OpenPlayer1Window();
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
