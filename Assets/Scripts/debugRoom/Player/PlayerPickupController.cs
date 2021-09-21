using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour

{
    [SerializeField] private PickupController _pickupController;
    public bool PickupItem(PickupInteractionComponent componentToPickup)
    {
        return _pickupController.TriggerInteractionDialog(componentToPickup);

    }
}
