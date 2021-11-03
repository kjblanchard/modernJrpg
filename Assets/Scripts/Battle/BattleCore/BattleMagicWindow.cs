using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleMagicWindow : MonoBehaviour, IGuiLoadingEvent
{
    public void DisplayCanvas(bool isEnabled) => _canvas.enabled = isEnabled;
    [SerializeField] private Canvas _canvas;
    private static readonly Color32 blueColor = new(169, 193, 243, 255);
    private static readonly Color32 grayColor = new(166, 166, 166, 100);
    [SerializeField]
    private GameObject _whereToAttachButtons;
    [SerializeField]
    private GameObject _prefabToSpawn;

    [SerializeField]
    private DOTweenAnimation _openScaleTween;
    [SerializeField]
    private DOTweenAnimation _openMoveTween;
    [SerializeField]
    private DOTweenAnimation _openRotateTween;
    [SerializeField] private DotweenBroadcasterComponent _playerMagicWindowOpenBroadcaster;
    private readonly List<MagicButtonController> _magicBattleButtons = new List<MagicButtonController>();
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
            var spawnedAbilityPrefab = Instantiate(_prefabToSpawn, _whereToAttachButtons.transform);
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
                if (battleButton.MagicNameText.color == grayColor)
                    return;
                battleButton.MagicNameText.color = grayColor;
                battleButton.MagicMpText.color = grayColor;
            }
            else
            {
                if (battleButton.MagicMpText.color == Color.blue)
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
        if (_isOpen)
            return;
        DeselectAllBattleButtons();
        _openMoveTween.DORestart();
        _openScaleTween.DORestart();
        _openRotateTween.DORestart();
        OnGuiLoadingEvent(this, new GuiLoadingEventArgs(GuiLoadingId,true));
    }

    public void OnPlayerWindowComplete(object obj, EventArgs e)
    {
        _isOpen = true;
        OnGuiLoadingEvent(this, new GuiLoadingEventArgs(GuiLoadingId,false));
    }

    public void OnPlayerWindowCloseComplete(object obj, EventArgs e)
    {
        _isOpen = false;
        OnGuiLoadingEvent(this, new GuiLoadingEventArgs(GuiLoadingId,false));
    }

    public void DeselectAllBattleButtons()
    {
        _magicBattleButtons.ForEach(button =>
        {
            if (button.SelectedText.enabled)
                button.SelectedText.enabled = false;
        });
    }

    public void SelectBattleButton(MagicButtonController buttonToEnable)
    {
        if (!buttonToEnable.SelectedText.enabled)
            buttonToEnable.SelectedText.enabled = true;
    }

    /// <summary>
    /// Starts all the tweens in reverse to close the window with an animation
    /// </summary>
    public void ClosePlayerWindow()
    {
        if (!_isOpen)
            return;
        _openMoveTween.DOPlayBackwards();
        _openScaleTween.DOPlayBackwards();
        _openRotateTween.DOPlayBackwards();
        OnGuiLoadingEvent(this, new GuiLoadingEventArgs(GuiLoadingId,true));
    }

    public event IGuiLoadingEvent.GuiLoadingEventHandler GuiLoadingEvent;
    public void OnGuiLoadingEvent(object obj, GuiLoadingEventArgs loadingArgs)
    {
        GuiLoadingEvent?.Invoke(obj, loadingArgs);
    }

    public Guid GuiLoadingId { get; set; } = Guid.NewGuid();
}
