using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour
{
    private BoxCollider2D theInteractableArea;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interacting");
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        var playerComp = collider.gameObject.GetComponent<player>();
        Debug.Log("Just got here");

        if (playerComp)
        {
            playerComp.whatToInteractWith = this;
            
            Debug.Log("Playerjust collided with me");
        }
    }
}
