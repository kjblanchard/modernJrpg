using TMPro;
using UnityEngine;

public class BattleNotificationsGui : MonoBehaviour
{

    [SerializeField] private TMP_Text pleaseSelectATargetTmp;


    public void EnableSelectATarget(bool isEnabled)
    {
        pleaseSelectATargetTmp.enabled = isEnabled;
    }


}
