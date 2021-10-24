using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleNotificationsGui : MonoBehaviour
{

    [SerializeField] private TMP_Text pleaseSelectATargetTmp;
    [SerializeField] private TMP_Text battleNotificationTmp;

    [SerializeField] private GameObject _damageTextToSpawn;
    [SerializeField] private GameObject _damageTextParent;


    private Queue<TMP_Text> _damageTextQueue;


    public void SpawnDamageTexts()
    {
        _damageTextQueue = new Queue<TMP_Text>();
        for (int i = 0; i < 10; i++)
        {
            var instantiatedDamageText = Instantiate(_damageTextToSpawn, _damageTextParent.transform);
            var instantiatedTmpText = instantiatedDamageText.GetComponent<TMP_Text>();
            instantiatedTmpText.enabled = false;
            _damageTextQueue.Enqueue(instantiatedTmpText);
        }
    }

    public TMP_Text GetTmpTextFromQueue()
    {
        return _damageTextQueue.Dequeue();
    }

    public void ReturnDamageTextToQueue(TMP_Text textToReturn)
    {
        _damageTextQueue.Enqueue(textToReturn);
    }

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
