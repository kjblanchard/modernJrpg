using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private PlayerInteractionComponent _playerInteractionComponent;

    public player(Rigidbody2D rigidBody)
    {
        this._rigidBody = rigidBody;
    }

    void OnMouseClick()
    {
        if (_playerInteractionComponent.CurrentlyInteracting)
            return;
        var mousePosition = Mouse.current.position.ReadValue();
        var screenPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        screenPosition.z = 0;
        _rigidBody.DOMove(new Vector2(screenPosition.x, screenPosition.y), 1.0f);
    }

    void OnMouseRightClick()
    {
        _playerInteractionComponent.PerformInteraction();

    }


}
