using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private PlayerInteractionComponent _playerInteractionComponent;
    [SerializeField]
    private PlayerBattleComponent _playerBattleComponent;

    /// <summary>
    /// Handles the mouseclick using unitys new input system.
    /// </summary>
    private void OnMouseClick()
    {
        if (_playerInteractionComponent.CurrentlyInteracting || _playerBattleComponent.InBattle)
            return;
        var mousePosition = CalculateMousePosition();
        _rigidBody.DOMove(mousePosition, 1.0f);
        _playerBattleComponent.UpdateCurrentBattleAreaStepCounter();
    }

    /// <summary>
    /// Calculates the mouse position from the screen to world
    /// </summary>
    /// <returns>Returns the vector of the mouse position</returns>
    private Vector2 CalculateMousePosition()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var screenPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        screenPosition.z = 0;
        return new Vector2(screenPosition.x, screenPosition.y);
    }

    /// <summary>
    /// Handles the Right mouseclick using unitys new input system.
    /// </summary>
    private void OnMouseRightClick()
    {
        _playerInteractionComponent.PerformInteraction();
    }

}
