using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    private BattleState _currentBattleState;

    private BattleState _previousBattleState;

    [SerializeField]
    private SerializableDictionaryBase<BattleStates, BattleState> _battleStateDictionary;

    public void ChangeBattleState(BattleStates battleStateToChangeTo)
    {
        if (!_battleStateDictionary.TryGetValue(battleStateToChangeTo, out var newBattleState)) return;
        _currentBattleState = newBattleState;
        _currentBattleState.StartState();
    }

    public enum BattleStates
    {
        Default,
        LoadingState
    }
}
