using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleMagicWindow : MonoBehaviour
{
    //Tween names
    private const string _playerWindowOpenScale = "player1MagicWindowOpenScaleTween";
    private const string _playerWindowOpenMove = "player1MagicWindowOpenMoveTween";
    private const string _playerWindowOpenRotate = "player1MagicWindowOpenRotateTween";
    /// <summary>
    /// This is for knowing when the tweens have finished playing for opening and closing the windows
    /// </summary>
    [SerializeField] private DotweenBroadcasterComponent _playerMagicWindowOpenBroadcaster;

    private List<MagicButtonController> _magicBattleButtons = new List<MagicButtonController>();
    public GameObject whereToAttachButtons;
    public GameObject prefabToSpawn;

    private BattleStateMachine _battleStateMachine;

    private void Start()
    {
        _battleStateMachine = FindObjectOfType<BattleStateMachine>();
    }

    public void LoadAbilitiesIntoButtons(Ability[] abilities, Battler battler)
    {
        foreach (var ability in abilities)
        {
            var spawnedAbilityPrefab = Instantiate(prefabToSpawn, whereToAttachButtons.transform);
            if (!spawnedAbilityPrefab.TryGetComponent<MagicButtonController>(out var spawnedBattleButton))
            {
                Destroy(spawnedBattleButton);
                DebugLogger.SendDebugMessage($"The spawned ability {ability.Name} with type {ability.Type} didn't have a magic controller");
                continue;
            }

            spawnedBattleButton.Ability = ability;
            spawnedBattleButton.MagicNameText.text = ability.Name;
            spawnedBattleButton.BattleButton.BattleButtonBroadcaster.ButtonPressedEvent += (object obj, EventArgs e) =>
            {
                if (_battleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTurnState || battler.BattleStats.BattlerMp < ability.MpCost)
                    return;
                BattleState.SetAbility(ability);
                _battleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.PlayerTargetingState);
            };
            _magicBattleButtons.Add(spawnedBattleButton);
        }
    }
    /// <summary>
    /// Starts all the tweens to open the window with an animation
    /// </summary>
    public void OpenPlayerWindow()
    {
        DOTween.Restart(_playerWindowOpenMove);
        DOTween.Restart(_playerWindowOpenScale);
        DOTween.Restart(_playerWindowOpenRotate);
    }

    /// <summary>
    /// Starts all the tweens in reverse to close the window with an animation
    /// </summary>
    public void ClosePlayerWindow()
    {
        DOTween.PlayBackwards(_playerWindowOpenRotate);
        DOTween.PlayBackwards(_playerWindowOpenScale);
        DOTween.PlayBackwards(_playerWindowOpenMove);
    }
}
