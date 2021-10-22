using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattleData BattleData => _battleData;
    public BattleGui BattleGui => _battleGui;

    public BattleStateMachine BattleStateMachine => _battleStateMachine;

    [SerializeField] private BattleGui _battleGui;
    [SerializeField] private BattleCamera _battleCamera;
    [SerializeField] private BattleData _battleData;
    [SerializeField] private BattleStateMachine _battleStateMachine;

    void Start()
    {
        ChangeBattleState(BattleStateMachine.BattleStates.LoadingState);
    }

    public void ChangeBattleState(BattleStateMachine.BattleStates stateToChangeTo, bool[] startupBools = null)
    {
        _battleStateMachine.ChangeBattleState(stateToChangeTo, startupBools);
    }

}
