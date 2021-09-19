using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractionComponent : InteractableComponent
{
    [SerializeField] public Dialog DialogToDisplay;

    void Awake()
    {
        InteractionType = TypeOfInteraction.Dialog;
    }



}
