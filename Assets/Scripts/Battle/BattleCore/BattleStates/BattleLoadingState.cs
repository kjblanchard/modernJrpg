
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleLoadingState :BattleState
{
    [SerializeField] private HorizontalLayoutGroup hlg;
    public override void StartState(params bool[] startupBools)
    {

        var data = PersistantData.instance.GetBattleData();
        _battleComponent.BattleData.SetBattleData(data);
        _battleComponent.BattleGui.StartFadeIn();
        BattleClock.GenerateTurnList(_battleComponent.BattleData.AllBattlers,hlg);
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
