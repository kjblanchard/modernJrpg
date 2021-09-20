using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractionComponent : InteractableComponent
{
    [SerializeField] public int ItemNum;
    [SerializeField] public int QuestLogId;
    private bool _hasBeenPickedUp;

    void Awake()
    {
        InteractionType = TypeOfInteraction.Pickup;
    }
    public void Start()
    {
        LoadItem();

    }
    public int PickupItem()
    {
        return ItemNum;
    }

    private void LoadItem()
    {
        //If this item is inside the quest log as picked up, turn the item off and mark it picked up.
    }

    public void DisableItem()
    {
        //Send notification to questlog that the item was picked up for saving
        _hasBeenPickedUp = true;
        //this.gameObject.SetActive(false);

    }
}
