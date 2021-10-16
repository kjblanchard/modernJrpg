using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerHudItem[] _playerHudItems;


    public void LoadPlayerBattlers(Battler[] playerBattlers)
    {
        for (int i = 0; i < playerBattlers.Length; i++)
        {
            var currentBattler = playerBattlers[i];
            if (!currentBattler)
            {
                //_playerHudItems[i].gameObject.SetActive(false);
                continue;
            }
            _playerHudItems[i].BattlerAssigned = currentBattler;
        }
    }

    public void LoadInitialPlayerHudItems()
    {
        foreach (var _playerHudItem in _playerHudItems)
        {

            if (_playerHudItem.BattlerAssigned is null)
            {
                _playerHudItem.gameObject.SetActive(false);
                continue;
            }
            _playerHudItem.HpText.text = _playerHudItem.BattlerAssigned.BattlerStats.BattlerHp.ToString();
            _playerHudItem.NameText.text = _playerHudItem.BattlerAssigned.BattlerStats.BattlerName.ToString();
            _playerHudItem.MpText.text = "N/a";
        }

    }

}
