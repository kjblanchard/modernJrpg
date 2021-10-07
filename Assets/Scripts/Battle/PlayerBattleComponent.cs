using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBattleComponent : MonoBehaviour
{
    public bool InBattle { get; private set; }

    [SerializeField] private Battler[] _playerParty;
    [SerializeField] private StudioEventEmitter _battleMusicEmitter;
    private BattleAreaComponent _currentBattleAreaComponent;
    [SerializeField] private AnimationCompleteComponent battleTransitionCompleteComponent;


    [SerializeField] private EnterBattleCamera _enterBattleCamera;


    void Start()
    {
        battleTransitionCompleteComponent.AnimationCompleteEvent += OnAnimationComplete;

    }

    /// <summary>
    /// Updates the step counter if there is a zone we are in, this is called by the player on movement
    /// </summary>
    public void UpdateCurrentBattleAreaStepCounter()
    {
        if (!_currentBattleAreaComponent) return;
        var shouldStartBattle = _currentBattleAreaComponent.UpdateBattleZoneOnStep();
        if (shouldStartBattle)
            StartBattle();

    }

    /// <summary>
    /// Starts the battle
    /// </summary>
    private void StartBattle()
    {
        InBattle = true;
        var bgmbus = FMODUnity.RuntimeManager.GetBus("bus:/bgm");
        bgmbus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        _battleMusicEmitter.Play();
        _enterBattleCamera.StartBattleTransition();
        PersistantData.instance.UpdateBattleData(new PersistantData.BattleData(_currentBattleAreaComponent.NextEncounter, _playerParty));
    }

    /// <summary>
    /// Called by the battle area component, this updates the zone that should be referenced
    /// </summary>
    /// <param name="newBattleArea"></param>
    public void UpdateBattleArea(BattleAreaComponent newBattleArea)
    {
        _currentBattleAreaComponent = newBattleArea;
    }

    public void OnAnimationComplete(object obj, EventArgs e)
    {
        SceneManager.LoadScene("battle1");

    }
}

