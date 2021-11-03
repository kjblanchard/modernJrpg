using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The notifications gui that will show notifications in the GUI for the player such as damage numbers or helping text
/// </summary>
public class BattleNotificationsGui : MonoBehaviour
{
    public void DisplayCanvas(bool isEnabled) => _canvas.enabled = isEnabled;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private TMP_Text _pleaseSelectATargetTmp;
    [SerializeField] private TMP_Text _battleNotificationTmp;

    [SerializeField] private GameObject _damageTextToSpawn;
    [SerializeField] private GameObject _damageTextParent;


    private Queue<BattleDamageText> _damageTextQueue;


    /// <summary>
    /// This is used to spawn the texts that will be used in battle for displaying damage numbers.
    /// </summary>
    public void SpawnDamageTexts()
    {
        _damageTextQueue = new Queue<BattleDamageText>();
        for (int i = 0; i < 10; i++)
        {
            var instantiatedDamageText = Instantiate(_damageTextToSpawn, _damageTextParent.transform);
            var instantiatedTmpText = instantiatedDamageText.GetComponent<BattleDamageText>();
            instantiatedTmpText.enabled = false;
            _damageTextQueue.Enqueue(instantiatedTmpText);
        }
    }

    /// <summary>
    /// Gets a text from the tmp text queue for pooling
    /// </summary>
    /// <returns>A tmp text that can be used for displaying damage.</returns>
    public BattleDamageText GetTmpTextFromQueue()
    {
        return _damageTextQueue.Dequeue();
    }

    /// <summary>
    /// Puts the tmp text back into the queue
    /// </summary>
    /// <param name="textToReturn"></param>
    public void ReturnDamageTextToQueue(BattleDamageText textToReturn)
    {
        _damageTextQueue.Enqueue(textToReturn);
    }

    /// <summary>
    /// Enables the select a target text on screen
    /// </summary>
    /// <param name="isEnabled"></param>
    public void EnableSelectATarget(bool isEnabled)
    {
        if (_pleaseSelectATargetTmp.enabled == isEnabled)
            return;
        _pleaseSelectATargetTmp.enabled = isEnabled;
    }

    /// <summary>
    /// Shows a text on the screen
    /// </summary>
    /// <param name="textToDisplay"></param>
    public void DisplayBattleNotification(string textToDisplay)
    {
        _battleNotificationTmp.text = textToDisplay;
        _battleNotificationTmp.enabled = true;

    }

    /// <summary>
    /// Disables a text on the screen;
    /// </summary>
    public void DisableBattleNotification()
    {
        _battleNotificationTmp.enabled = false;
    }

}

