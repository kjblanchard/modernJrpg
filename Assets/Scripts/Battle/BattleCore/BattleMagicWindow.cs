using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleMagicWindow : MonoBehaviour
{

    [SerializeField]
    private DOTweenAnimation openScaleTween;
    [SerializeField]
    private DOTweenAnimation openMoveTween;
    [SerializeField]
    private DOTweenAnimation openRotateTween;

    private static readonly Color32 blueColor = new Color32(169, 193, 243, 255);
    private static readonly Color32 grayColor = new Color32(166, 166, 166, 100);

    /// <summary>
    /// This is for knowing when the tweens have finished playing for opening and closing the windows
    /// </summary>
    [SerializeField] private DotweenBroadcasterComponent _playerMagicWindowOpenBroadcaster;

    private List<MagicButtonController> _magicBattleButtons = new List<MagicButtonController>();
    public GameObject whereToAttachButtons;
    public GameObject prefabToSpawn;

    private bool _isOpen;

    private BattleStateMachine _battleStateMachine;


    private void Awake()
    {
        _playerMagicWindowOpenBroadcaster.DotweenCompleteEvent += OnPlayerWindowComplete;
        _playerMagicWindowOpenBroadcaster.DotweenRewindCompleteEvent += OnPlayerWindowCloseComplete;

    }

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
            spawnedBattleButton.MagicMpText.text = ability.MpCost.ToString();
            spawnedBattleButton.BattleButton.BattleButtonBroadcaster.ButtonPressedEvent += (obj, e) =>
            {
                var currentBattleState = _battleStateMachine.CurrentBattleStateEnum;
                if (currentBattleState != BattleStateMachine.BattleStates.PlayerTargetingState && currentBattleState != BattleStateMachine.BattleStates.PlayerTurnState)
                    return;
                if (battler.BattleStats.BattlerCurrentMp < ability.MpCost || !_isOpen || BattleGui.IsAnimationPlaying)
                    return;
                DeselectAllBattleButtons();
                SelectBattleButton(spawnedBattleButton);
                BattleState.SetAbility(ability);
                _battleStateMachine.ChangeBattleState(BattleStateMachine.BattleStates.PlayerTargetingState);
            };
            _magicBattleButtons.Add(spawnedBattleButton);
        }
    }

    public void CheckForSufficientMana(int manaAmount)
    {
        _magicBattleButtons.ForEach((battleButton) =>
        {
            if (manaAmount < battleButton.Ability.MpCost)
            {
                if(battleButton.MagicNameText.color == grayColor)
                   return; 
                battleButton.MagicNameText.color = grayColor;
                battleButton.MagicMpText.color = grayColor;
            }
            else
            {
                if(battleButton.MagicMpText.color == Color.blue)
                   return; 
                battleButton.MagicNameText.color = Color.white;
                battleButton.MagicMpText.color = blueColor;
            }

        });
    }
    /// <summary>
    /// Starts all the tweens to open the window with an animation
    /// </summary>
    public void OpenPlayerWindow()
    {
        if(_isOpen)
            return;
        DeselectAllBattleButtons();
        openMoveTween.DORestart();
        openScaleTween.DORestart();
        openRotateTween.DORestart();
        BattleGui.IsAnimationPlaying = true;
    }

    public void OnPlayerWindowComplete(object obj, EventArgs e)
    {
        _isOpen = true;
        BattleGui.IsAnimationPlaying = false;

    }

    public void OnPlayerWindowCloseComplete(object obj, EventArgs e)
    {
        _isOpen = false;
        BattleGui.IsAnimationPlaying = false;

    }

    public void DeselectAllBattleButtons()
    {
        _magicBattleButtons.ForEach(button =>
        {
            button.SelectedText.enabled = false;
        });
    }

    public void SelectBattleButton(MagicButtonController buttonToEnable)
    {
        buttonToEnable.SelectedText.enabled = true;
    }

    /// <summary>
    /// Starts all the tweens in reverse to close the window with an animation
    /// </summary>
    public void ClosePlayerWindow()
    {
        if(!_isOpen)
            return;
        openMoveTween.DOPlayBackwards();
        openScaleTween.DOPlayBackwards();
        openRotateTween.DOPlayBackwards();
        BattleGui.IsAnimationPlaying = true;
    }
}
