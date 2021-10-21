using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{

    public BattleStates CurrentBattleStateEnum { get; private set; }
    private BattleState _currentBattleState;

    private BattleState _previousBattleState;

    [SerializeField]
    private SerializableDictionaryBase<BattleStates, BattleState> _battleStateDictionary;

    /// <summary>
    /// Changes the battle state to the referenced enum of the battlestates
    /// </summary>
    /// <param name="battleStateToChangeTo">The enum for battle state to change to</param>
    public void ChangeBattleState(BattleStates battleStateToChangeTo)
    {
        if (!_battleStateDictionary.TryGetValue(battleStateToChangeTo, out var newBattleState)) return;
        _currentBattleState = newBattleState;
        CurrentBattleStateEnum = battleStateToChangeTo;
        _currentBattleState.StartState();
    }

    public enum BattleStates
    {
        Default,
        LoadingState,
        BetweenTurnState,
        PlayerTurnState,
        EnemyTurnState,
        PlayerTargetingState,
        ActionPerformState,
        BattleEndState,


    }
}
