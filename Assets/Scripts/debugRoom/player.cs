using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public interactable whatToInteractWith;
    public BoxCollider2D boxCollider;
    public Rigidbody2D rigidBody;

    public DialogController DialogController;
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
        rigidBody.DOMove(new Vector2(screenPosition.x, screenPosition.y), 1.0f);
    }

    void OnMouseRightClick()
    {
        if (whatToInteractWith)
        {
            whatToInteractWith.Interact(this);
        }
    }

}
