using TMPro;
using UnityEngine;

public class BattleNotificationsGui : MonoBehaviour
{

    [SerializeField] private TMP_Text pleaseSelectATargetTmp;
    [SerializeField] private TMP_Text battleNotificationTmp;


    public void EnableSelectATarget(bool isEnabled)
    {
        pleaseSelectATargetTmp.enabled = isEnabled;
    }

    public void DisplayBattleNotification(string textToDisplay)
    {
        battleNotificationTmp.text = textToDisplay;
        battleNotificationTmp.enabled = true;

    }

    public void DisableBattleNotification()
    {
        battleNotificationTmp.enabled = false;
    }


}
