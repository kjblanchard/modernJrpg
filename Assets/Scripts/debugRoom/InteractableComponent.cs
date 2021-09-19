using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableComponent : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D theInteractableArea;
    [SerializeField]
    public TypeOfInteraction InteractionType;

    [SerializeField] public Dialog Dialog;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
            return;
        var playerInteractionComponent = collider.gameObject.GetComponent<PlayerInteractionComponent>();
        playerInteractionComponent.UpdateInteraction(this);

    }
    void OnTriggerExit2D(Collider2D collider)
    {
        var playerInteractionComponent = collider.gameObject.GetComponent<PlayerInteractionComponent>();
        playerInteractionComponent.RemoveInteraction();
    }
}
public enum TypeOfInteraction
{
    Dialog,
    Pickup,
}
