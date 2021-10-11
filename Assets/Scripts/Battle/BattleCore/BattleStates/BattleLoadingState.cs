using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoadingState :BattleState
{
    public override void StartState(params bool[] startupBools)
    {

        var data = PersistantData.instance.GetBattleData();
        BattleComponent.BattleData.SetBattleData(data);
        BattleComponent.BattleGui.StartFadeIn();
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
