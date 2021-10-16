using UnityEngine;

public abstract class BattleState : MonoBehaviour
{
    protected static Battle _battleComponent;
    public abstract void StartState(params bool[] startupBools);
    public abstract void StateUpdate();
    public abstract void EndState();
    public abstract void ResetState();

    private void Start()
    {
        if (_battleComponent == null)
            _battleComponent = FindObjectOfType<Battle>();
    }

}