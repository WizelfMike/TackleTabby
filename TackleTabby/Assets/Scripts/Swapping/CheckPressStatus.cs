using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CheckPressStatus : MonoBehaviour
{
    [Description("Passes the start position of the swipe and its end-position")]
    public UnityEvent<Vector2, Vector2> OnSwipeEnded = new();

    private Vector2 _startPosition;
    

    public void OnPressStart(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        if (Touchscreen.current.primaryTouch.phase.value != TouchPhase.Began)
            return;
        
        _startPosition = context.ReadValue<Vector2>();
    }

    public void OnPressEnd(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        if (Touchscreen.current.primaryTouch.phase.value != TouchPhase.Ended)
            return;
        
        Vector2 endPosition = context.ReadValue<Vector2>();
        OnSwipeEnded.Invoke(_startPosition, endPosition);
    }
}
