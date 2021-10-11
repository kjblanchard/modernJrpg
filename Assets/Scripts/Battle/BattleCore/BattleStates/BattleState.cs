using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BattleState : MonoBehaviour
{
    public static Battle BattleComponent;
    public abstract void StartState(params bool[] startupBools);
    public abstract void StateUpdate();
    public abstract void EndState();
    public abstract void ResetState();

    private void Start()
    {
        if (BattleComponent == null)
            BattleComponent = FindObjectOfType<Battle>();
    }

}
