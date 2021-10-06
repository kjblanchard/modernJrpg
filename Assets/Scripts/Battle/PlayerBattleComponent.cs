using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBattleComponent : MonoBehaviour
{
    public bool InBattle { get; private set; }

    [SerializeField] private Battler[] _playerParty;
    [SerializeField] private StudioEventEmitter _battleMusicEmitter;
    private BattleAreaComponent _currentBattleAreaComponent;



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
        Debug.Log("BattleShouldStartNow");
        InBattle = true;

        var bgmbus = FMODUnity.RuntimeManager.GetBus("bus:/bgm");
        bgmbus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        _battleMusicEmitter.Play();
    }

    /// <summary>
    /// Called by the battle area component, this updates the zone that should be referenced
    /// </summary>
    /// <param name="newBattleArea"></param>
    public void UpdateBattleArea(BattleAreaComponent newBattleArea)
    {
        _currentBattleAreaComponent = newBattleArea;
    }
}

