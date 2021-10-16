using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattleData BattleData => _battleData;
    public BattleGui BattleGui => _battleGui;

    [SerializeField] private BattleGui _battleGui;
    [SerializeField] private BattleCamera _battleCamera;
    [SerializeField] private BattleData _battleData;
    [SerializeField] private BattleStateMachine _battleStateMachine;

    void Start()
    {
        _battleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.LoadingState);
    }

}
