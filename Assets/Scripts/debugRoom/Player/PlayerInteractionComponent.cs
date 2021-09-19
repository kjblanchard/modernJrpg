using System;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
    public bool CurrentlyInteracting { get; private set; }
    [SerializeField] private DialogController _dialogController;
    [SerializeField] private PlayerPickupController _playerPickupController;
    [SerializeField] private GameObject canInteractSprite;
    private InteractableComponent thingToIntaractWith;

    void Start()
    {
    }

    public void UpdateInteraction(InteractableComponent newInteractable)
    {
        canInteractSprite.SetActive(true);
        thingToIntaractWith = newInteractable;
    }

    public void RemoveInteraction()
    {
        canInteractSprite.SetActive(false);
        thingToIntaractWith = null;
    }

    public void PerformInteraction()
    {
        if (!thingToIntaractWith)
            return;
        switch (thingToIntaractWith.InteractionType)
        {
            case TypeOfInteraction.Dialog:
                var dialogCasted = (DialogInteractionComponent) thingToIntaractWith;
                HandleDialogInteraction(dialogCasted.DialogToDisplay);
                break;
            case TypeOfInteraction.Pickup:
                var pickupCasted = (PickupInteractionComponent) thingToIntaractWith;
                HandlePickupInteraction(pickupCasted);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleDialogInteraction(Dialog dialogToDisplay)
    {
        CurrentlyInteracting = _dialogController.TriggerInteractionDialog(dialogToDisplay);
    }

    private void HandlePickupInteraction(PickupInteractionComponent pickupComponent)
    {
        _playerPickupController.PickupItem(pickupComponent);
    }


}
