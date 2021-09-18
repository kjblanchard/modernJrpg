using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class interactable : MonoBehaviour
{
    private BoxCollider2D theInteractableArea;
    [SerializeField] public TypeOfInteraction interactionType;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(player playerChar)
    {
        playerChar.DialogController.currentText = "This is the new text for me to put on the screen";
        playerChar.DialogController.UpdateDialog();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
            return;
        var playerComp = collider.gameObject.GetComponent<player>();
        if (!playerComp) return;
        playerComp.whatToInteractWith = this;
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        var playerComp = collider.gameObject.GetComponent<player>();
        if (!playerComp) return;
        playerComp.whatToInteractWith = null;
        playerComp.DialogController.currentText = "";
        playerComp.DialogController.UpdateDialog();
    }
}

public enum TypeOfInteraction
{
    Dialog,
}
