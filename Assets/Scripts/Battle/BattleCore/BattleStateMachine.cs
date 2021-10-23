using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{

    /// <summary>
    /// The battle state that we are in currently
    /// </summary>
    public BattleStates CurrentBattleStateEnum { get; private set; }

    private BattleState _currentBattleState;
    private BattleState _previousBattleState;

    /// <summary>
    /// This dictionary is a lookup for the state to change to.  It is set in the editor when new battle states are added.
    /// </summary>
    [SerializeField]
    private SerializableDictionaryBase<BattleStates, BattleState> _battleStateDictionary;

    /// <summary>
    /// Changes the battle state to the referenced enum of the battlestates
    /// </summary>
    /// <param name="battleStateToChangeTo">The enum for battle state to change to</param>
    public void ChangeBattleState(BattleStates battleStateToChangeTo, bool[] startupBools = null)
    {
        if (!_battleStateDictionary.TryGetValue(battleStateToChangeTo, out var newBattleState)) return;
        _previousBattleState = _currentBattleState;
        _currentBattleState = newBattleState;
        CurrentBattleStateEnum = battleStateToChangeTo;
        _currentBattleState.StartState(startupBools);
    }

    /// <summary>
    /// The different states that the battle can be in.
    /// </summary>
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
        BattleRewardState,
        BattleTransitionState,


    }
}
