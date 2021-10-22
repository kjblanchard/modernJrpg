
using STOP_MODE = FMOD.Studio.STOP_MODE;
using FMODUnity;
using UnityEngine;

public class BattleEndState : BattleState
{
    // Start is called before the first frame update
    [SerializeField] private StudioEventEmitter battleMusic;
    public override void StartState(params bool[] startupBools)
    {

        var bgmbus = FMODUnity.RuntimeManager.GetBus("bus:/bgm");
        bgmbus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        battleMusic.Play();
        _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification("You Win!!!");


    }

    private void OnMouseClick()
    {
        if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum !=
            BattleStateMachine.BattleStates.BattleEndState) return;
        var bgmbus = FMODUnity.RuntimeManager.GetBus("bus:/bgm");
        bgmbus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        SceneController.ChangeGameScene(SceneController.GameScenesEnum.DebugRoom);
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
