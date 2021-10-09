using System;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBattleComponent : MonoBehaviour
{
    public bool InBattle { get; private set; }

    [SerializeField] private Battler[] _playerParty;
    [SerializeField] private StudioEventEmitter _battleMusicEmitter;
    [SerializeField] private AnimationCompleteComponent battleTransitionCompleteComponent;
    [SerializeField] private EnterBattleCamera _enterBattleCamera;

    private BattleAreaComponent _currentBattleAreaComponent;

    /// <summary>
    /// Subscribes to the battle transition complete component, so that it is notified correctly when it ends so that we can change the scene
    /// </summary>
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
        SceneController.ChangeGameScene(PersistantData.instance.GetBattleData().BattleEncounter.LocationForBattle);

    }
}

