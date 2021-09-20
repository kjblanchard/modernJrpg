using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupItemController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogTextbox;
    [SerializeField] private TMP_Text _textBoxToUpdate;
    public bool inDialog;
    private int _currentLocationInDialog;



    /// <summary>
    /// Triggers the interaction dialog for pickups
    /// </summary>
    /// <param name="dialogToGoThrough">The Scriptable object dialog that will be gone though</param>
    /// <returns>Returns true if the dialog should continue, and false if the dialog is ending</returns>
    public bool TriggerInteractionDialog(PickupInteractionComponent pickupItem)
    {
        if (inDialog)
        {
            return AdvanceDialog(pickupItem);
        }
        inDialog = true;
        _currentLocationInDialog = 0;
        DisplayDialog(pickupItem);
        return true;
    }


    public bool AdvanceDialog(PickupInteractionComponent dialogToGoThrough)
    {
        if (_currentLocationInDialog == 0)
        {
            EndDialog(dialogToGoThrough);
            return false;
        }
        _currentLocationInDialog++;
        DisplayDialog(dialogToGoThrough);
        return true;
    }

    private void EndDialog(PickupInteractionComponent dialog)
    {
        _textBoxToUpdate.text = "";
        inDialog = false;
        _currentLocationInDialog = 0;
        dialog.gameObject.SetActive(false);
        

    }

    private void DisplayDialog(PickupInteractionComponent dialogToGoThrough)
    {
        var whattoSay = "You Just found " + ItemLookupDictionary[dialogToGoThrough.ItemNum] + "!!";
        _textBoxToUpdate.text = whattoSay;
    }

    private Dictionary<int, string> ItemLookupDictionary = new Dictionary<int, string>
    {
        {0, "Big Sword"},
        {1, "Small Sword"}
    };

}
