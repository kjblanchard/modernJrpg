using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerHudItem[] _playerHudItems;


    /// <summary>
    /// Adds the battler to the hud item so that it can be referenced.
    /// </summary>
    /// <param name="playerBattlers"></param>
    public void LoadBattlersIntoHud(Battler[] playerBattlers)
    {
        for (var i = 0; i < playerBattlers.Length; i++)
        {
            var currentBattler = playerBattlers[i];
            if (currentBattler is null)
                continue;
            _playerHudItems[i].BattlerAssigned = currentBattler;
        }
    }

    public void LoadInitialHudItems()
    {
        foreach (var _playerHudItem in _playerHudItems)
        {
            if (_playerHudItem.BattlerAssigned is null)
            {
                _playerHudItem.gameObject.SetActive(false);
                continue;
            }
            _playerHudItem.HpText.text = _playerHudItem.BattlerAssigned.BattleStats.BattlerMaxHp.ToString();
            _playerHudItem.NameText.text = _playerHudItem.BattlerAssigned.BattleStats.BattlerDisplayName;
            _playerHudItem.MpText.text = "N/a";
        }
    }

}
