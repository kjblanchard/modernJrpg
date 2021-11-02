using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardScreen : BattleState
{
    public event PlayerInputEventHandler PlayerClickEvent;
    public delegate void PlayerInputEventHandler(object obj, EventArgs e);


    private readonly List<PlayerResult> playerResults = new();
    public GameObject ResultGameObjectPrefab;
    public GameObject PrefabSpawnLocation;
    public BattlerBaseStats[] BattlersToLoad;
    public Canvas rewardScreenCanvas;
    private bool isLoaded = false;
    private int _expGained;


    public bool isCurrentlyPlayingGainingSound = false;
    public bool IsFinishedPlayingAnim => playerResults.All(playerResult => playerResult.isFinished);

    public override void StartState(params bool[] startupBools)
    {
        _expGained = _battleComponent.BattleData.EnemyBattlers.ToList()
            .Sum(battler => battler.BattleStats.BattlerExpReward);

        foreach (var _battlerBaseStats in BattlersToLoad)
        {
            if (_battlerBaseStats is null) continue;
            var spawnedPrefab = Instantiate(ResultGameObjectPrefab, PrefabSpawnLocation.transform);
            var spawnedPlayerResulg = spawnedPrefab.GetComponent<PlayerResult>();
            var battleStats = new BattleStats(_battlerBaseStats);
            spawnedPlayerResulg.LoadBattlerIntoPlayerResult(battleStats, _expGained);
            PlayerClickEvent += spawnedPlayerResulg.OnPlayerClickEvent;
            playerResults.Add(spawnedPlayerResulg);
        }

        rewardScreenCanvas.enabled = true;
        playerResults.ForEach(playerResult => playerResult.isLoaded = true);
        StartCoroutine(WaitForLoad());
    }


    void Update()
    {
        var isAnyBattlerGainingExp = false;
        playerResults.ForEach(playerResult =>
        {
            if (isAnyBattlerGainingExp)
                return;
            if (playerResult.IsCurrentlyGainingExp)
                isAnyBattlerGainingExp = true;
        });
        if (isAnyBattlerGainingExp)
        {
            if (!isCurrentlyPlayingGainingSound)
            {
                SoundController.Instance.PlayBgm(SoundController.Bgm.BattleRewardGaining);
                isCurrentlyPlayingGainingSound = true;
            }
        }
        else
        {
            if (isCurrentlyPlayingGainingSound)
            {
                SoundController.Instance.StopBgm(SoundController.Bgm.BattleRewardGaining, false);
                isCurrentlyPlayingGainingSound = false;
            }
        }

    }

    private void OnMouseClick()
    {
        if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.BattleRewardState)
            return;
        if (isLoaded)
            OnPlayerClick(this, EventArgs.Empty);
        if (IsFinishedPlayingAnim)
            _battleComponent.BattleGui.BattleTransitionComponent.StartFadeOut();



    }

    private void OnPlayerClick(object obj, EventArgs e)
    {
        PlayerClickEvent?.Invoke(obj, e);
    }
    public override void StateUpdate()
    {
        throw new NotImplementedException();
    }

    public override void EndState()
    {
    }

    public override void ResetState()
    {
        throw new NotImplementedException();
    }

    private IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(0.25f);
        isLoaded = true;
    }

}
