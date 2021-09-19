using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour
{
    public void PickupItem(PickupInteractionComponent componentToPickup)
    {
        var item = componentToPickup.PickupItem();
        componentToPickup.DisableItem();

    }
}
