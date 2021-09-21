using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Require circle collider for overlapping
/// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
public abstract class InteractableComponent : MonoBehaviour
{
    [SerializeField]
    protected CircleCollider2D theInteractableArea;

    public TypeOfInteraction InteractionType { get; protected set; }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
            return;
        var playerInteractionComponent = collider.gameObject.GetComponent<PlayerInteractionComponent>();
        playerInteractionComponent.UpdateInteraction(this, true);

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var playerInteractionComponent = collider.gameObject.GetComponent<PlayerInteractionComponent>();
        playerInteractionComponent.UpdateInteraction(null, false);
    }
}
public enum TypeOfInteraction
{
    Dialog,
    Pickup
}

