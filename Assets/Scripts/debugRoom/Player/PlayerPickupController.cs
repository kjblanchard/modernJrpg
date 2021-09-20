using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour

{
    [SerializeField] private PickupItemController _pickupItemController;
    public bool PickupItem(PickupInteractionComponent componentToPickup)
    {
        var item = componentToPickup.PickupItem();
        componentToPickup.DisableItem();
        return _pickupItemController.TriggerInteractionDialog(componentToPickup);

    }
}
