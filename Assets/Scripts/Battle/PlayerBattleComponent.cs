using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class PlayerBattleComponent : MonoBehaviour
{
    [SerializeField] private Battler[] _playerParty;
    private BattleAreaComponent _currentBattleAreaComponent;
    public bool InBattle { get; private set; }
    [SerializeField] private StudioEventEmitter _battleMusicEmitter;


    private void StartBattle()
    {
        Debug.Log("BattleShouldStartNow");
        InBattle = true;

        var bgmbus = FMODUnity.RuntimeManager.GetBus("bus:/bgm");
        bgmbus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        _battleMusicEmitter.Play();
    }

    public void UpdateCurrentBattleAreaStepCounter()
    {
        if (!_currentBattleAreaComponent) return;
        var shouldStartBattle = _currentBattleAreaComponent.UpdateSteps();
        if (shouldStartBattle)
            StartBattle();

    }

    public void UpdateBattleArea(BattleAreaComponent newBattleArea)
    {
        _currentBattleAreaComponent = newBattleArea;
    }
}

