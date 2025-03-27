using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CheckPressStatus : MonoBehaviour
{
    [Description("Passes the start position of the swipe and its direction")]
    public UnityEvent<Vector2, Vector2> OnSwipeEnded = new();

    private Vector2 _startPosition;
    
    public void OnPressStart(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        TouchState touch = context.ReadValue<TouchState>();
        if (touch.phase != TouchPhase.Began)
            return;
        
        _startPosition = touch.position;
    }

    public void OnPressEnd(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        TouchState touch = context.ReadValue<TouchState>();
        if (touch.phase != TouchPhase.Ended)
            return;

        Vector2 direction = (touch.position - _startPosition);
        OnSwipeEnded.Invoke(_startPosition, direction);
    }
}
