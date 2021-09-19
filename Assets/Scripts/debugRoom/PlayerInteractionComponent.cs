using System;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
    public bool CurrentlyInteracting { get; private set; }
    [SerializeField] private DialogController _dialogController;
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
                HandleDialogInteraction();
                break;
            case TypeOfInteraction.Pickup:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleDialogInteraction()
    {
        CurrentlyInteracting = _dialogController.TriggerInteractionDialog(thingToIntaractWith.Dialog);
    }


}
