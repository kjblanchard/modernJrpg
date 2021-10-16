using UnityEngine;
using UnityEngine.UI;

public class BattleLoadingState :BattleState
{
    [SerializeField] private HorizontalLayoutGroup hlg;
    public override void StartState(params bool[] startupBools)
    {
        _battleComponent.BattleData.SetBattleData(PersistantData.instance.GetBattleData());
        _battleComponent.BattleData.SetBattlers();
        var allBattlers = _battleComponent.BattleData.AllBattlers;
        _battleComponent.BattleGui.LoadInitialPlayerHud(_battleComponent.BattleData.PlayerBattlers);
        CalculateInitialTurnsForBattlers(allBattlers);
        var next20Turns = BattlerClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers, true);
        //Update the gui and other elements with that turn data
        _battleComponent.BattleGui.StartFadeIn();
        //Move into between turn state at end of fade in

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

    /// <summary>
    /// Calculates the initial 20 turns for battle and then confirms them
    /// </summary>
    /// <param name="battlers"></param>
    private void CalculateInitialTurnsForBattlers(Battler[] battlers)
    {
        foreach (var _battler in battlers)
        {
            _battler.CalculatePotentialNext20Turns(1.0f,true);
            _battler.ConfirmTurn();
            
        }

    }
}
