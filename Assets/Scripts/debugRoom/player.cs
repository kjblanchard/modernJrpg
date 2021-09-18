using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public interactable whatToInteractWith;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMove()
    {
        Debug.Log("Moving");
    }

    void OnMouseClick()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var screenPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        screenPosition.z = 0;
        this.gameObject.transform.position = screenPosition;
    }

    void OnMouseRightClick()
    {
        if (whatToInteractWith)
        {
            whatToInteractWith.Interact();
        }
    }

}
