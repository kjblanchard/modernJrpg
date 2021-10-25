using System;

public class PlayerTargetingState : BattleState
{

    private BattlerClickHandler[] _playerClicks;
    private BattlerClickHandler[] _enemyClicks;

    public override void StartState(params bool[] startupBools)
    {
        _targetBattler = null;
        _battleComponent.BattleGui.BattleNotifications.EnableSelectATarget(true);
    }

    /// <summary>
    /// Loads the players and enemies clicks to handle what happens when you click on them in this state. Should be ran from the battle loading state or in awake/start
    /// </summary>
    public void LoadBattleClicks()
    {
        if (_playerClicks != null) return;
        _playerClicks = new BattlerClickHandler[_battleComponent.BattleData.PlayerBattlers.Length];
        _enemyClicks = new BattlerClickHandler[_battleComponent.BattleData.EnemyBattlers.Length];

        for (var i = 0; i < _battleComponent.BattleData.PlayerBattlers.Length; i++)
        {
            var battler = _battleComponent.BattleData.PlayerBattlers[i];
            if (battler == null) continue;
            _playerClicks[i] = battler.BattlerClickHandler;
            _playerClicks[i]._battleButtonBroadcaster.ButtonPressedEvent += (object obj, EventArgs e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battler.BattleStats.IsDead)
                    return;
                _targetBattler = battler;
                _battleComponent.BattleGui.Player1Window.ClosePlayerWindow();
                _battleComponent.BattleGui.Player1MagicWindow.ClosePlayerWindow();

            };
            _playerClicks[i]._battleButtonBroadcaster.ButtonHoveredEvent += (object obj, EventArgs e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battler.BattleStats.IsDead)
                    return;
                _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification($"{battler.BattleStats.BattlerDisplayName}");
            };
            _playerClicks[i]._battleButtonBroadcaster.ButtonHoveredLeaveEvent += (object obj, EventArgs e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battler.BattleStats.IsDead)
                    return;
                _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();
            };
        }
        for (var i = 0; i < _battleComponent.BattleData.EnemyBattlers.Length; i++)
        {
            var battler = _battleComponent.BattleData.EnemyBattlers[i];
            if (battler == null) continue;
            _enemyClicks[i] = battler.BattlerClickHandler;
            _enemyClicks[i]._battleButtonBroadcaster.ButtonPressedEvent += (object obj, EventArgs e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battler.BattleStats.IsDead)
                    return;
                _targetBattler = battler;
                _battleComponent.BattleGui.Player1Window.ClosePlayerWindow();
                _battleComponent.BattleGui.Player1MagicWindow.ClosePlayerWindow();
            };
            _enemyClicks[i]._battleButtonBroadcaster.ButtonHoveredEvent += (object obj, EventArgs e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battler.BattleStats.IsDead)
                    return;
                _battleComponent.BattleGui.BattleNotifications.DisplayBattleNotification($"{battler.BattleStats.BattlerDisplayName}");
            };
            _enemyClicks[i]._battleButtonBroadcaster.ButtonHoveredLeaveEvent += (object obj, EventArgs e) =>
            {
                if (_battleComponent.BattleStateMachine.CurrentBattleStateEnum != BattleStateMachine.BattleStates.PlayerTargetingState || battler.BattleStats.IsDead)
                    return;
                _battleComponent.BattleGui.BattleNotifications.DisableBattleNotification();
            };
        }


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


}
