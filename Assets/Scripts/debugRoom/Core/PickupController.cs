using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The controller used for handling pickups, it is referenced by the player interaction handler
/// </summary>
public class PickupController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogTextbox;
    [SerializeField] private TMP_Text _textBoxToUpdate;
    public bool InDialog { get; private set; }
    private int _currentLocationInDialog;



    /// <summary>
    /// Triggers the interaction dialog for pickups
    /// </summary>
    /// <param name="pickupItem">The Scriptable object dialog that will be gone though</param>
    /// <returns>Returns true if the dialog should continue, and false if the dialog is ending</returns>
    public bool TriggerInteractionDialog(PickupInteractionComponent pickupItem)
    {
        return InDialog ? AdvanceDialog(pickupItem) : InitializePickupDialog(pickupItem);
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
        _currentLocationInDialog = 0;
        dialog.gameObject.SetActive(false);
        InDialog = false;
        _dialogTextbox.SetActive(false);


    }
    private bool InitializePickupDialog(PickupInteractionComponent pickupItem)
    {
        InDialog = true;
        _dialogTextbox.SetActive(true);
        _currentLocationInDialog = 0;
        DisplayDialog(pickupItem);
        return true;
    }
    private void DisplayDialog(PickupInteractionComponent dialogToGoThrough)
    {
        var whatToSay = "You Just found " + ItemLookupDictionary[dialogToGoThrough.ItemForPickup.ItemNumber] + "!!";
        _textBoxToUpdate.text = whatToSay;
    }

    //TODO This should be removed and an actual Item database should be made that can be looked up
    private Dictionary<int, string> ItemLookupDictionary = new Dictionary<int, string>
    {
        {0, "Big Sword"},
        {1, "Small Sword"}
    };

}
