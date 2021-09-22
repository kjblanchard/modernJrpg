using System;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
    /// <summary>
    /// If the interaction component is currently within an interaction.
    /// looks at the other controllers status each frame and updates itself
    /// </summary>
    public bool CurrentlyInteracting { get; private set; }
    [SerializeField] private DialogController _dialogController;
    [SerializeField] private PickupController _playerPickupController;
    [SerializeField] private GameObject _interactionSprite;
    private InteractableComponent _currentInteractableComponent;

    private void Update()
    {
        var interacting = _dialogController.InDialog || _playerPickupController.InDialog;
        CurrentlyInteracting = interacting;
    }

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
            //TODO think of a way to make this not have to cast on every interaction check.
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
         _dialogController.HandlePlayerRightClick(dialogToDisplay);
    }

    private void HandlePickupInteraction(PickupInteractionComponent pickupComponent)
    {
        CurrentlyInteracting = _playerPickupController.TriggerInteractionDialog(pickupComponent);
    }


}
