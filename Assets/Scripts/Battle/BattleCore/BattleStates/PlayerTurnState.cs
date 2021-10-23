
public class PlayerTurnState : BattleState
{
    public override void StartState(params bool[] startupBools)
    {
        _battleComponent.BattleGui.Player1Window.OpenPlayerWindow();
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
