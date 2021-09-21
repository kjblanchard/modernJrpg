using System;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
    /// <summary>
    /// If the interaction component is currently within an interaction.
    /// </summary>
    public bool CurrentlyInteracting { get; private set; }
    [SerializeField] private DialogController _dialogController;
    [SerializeField] private PlayerPickupController _playerPickupController;
    [SerializeField] private GameObject _interactionSprite;
    private InteractableComponent _currentInteractableComponent;


    /// <summary>
    /// This is called by a Remote interactable component.  It will turn on the interaction sprite, and also update your current interactable
    /// </summary>
    /// <param name="newInteractable"></param>
    public void UpdateInteraction(InteractableComponent newInteractable, bool isInteractable)
    {
        _interactionSprite.SetActive(isInteractable);
        _currentInteractableComponent = newInteractable;
    }

    /// <summary>
    /// This is called by the player to actually handle the interaction
    /// </summary>
    public void PerformInteraction()
    {
        if (!_currentInteractableComponent)
            return;
        switch (_currentInteractableComponent.InteractionType)
        {
            case TypeOfInteraction.Dialog:
                var dialogCasted = (DialogInteractionComponent)_currentInteractableComponent;
                HandleDialogInteraction(dialogCasted.DialogToDisplay);
                break;
            case TypeOfInteraction.Pickup:
                var pickupCasted = (PickupInteractionComponent)_currentInteractableComponent;
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
        CurrentlyInteracting = _playerPickupController.PickupItem(pickupComponent);
    }


}
