using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class BaitMovement : MonoBehaviour
{
    [SerializeField] 
    private int MinSwipeDistance;
    
    private Vector2 _startPosition;

    public void OnPushedButton()
    {
        TouchPhase started = TouchPhase.Began;
        TouchPhase lastInput = Touchscreen.current.primaryTouch.phase.value;
        if (lastInput == started)
        {
            _startPosition = Touchscreen.current.position.value;
        }
    }

    public void OnPressEnd()
    {
        TouchPhase ended = TouchPhase.Ended;
        TouchPhase lastInput = Touchscreen.current.primaryTouch.phase.value;
        if (lastInput == ended)
        {
            checkSwipe(Touchscreen.current.position.value);
        }
    }

    private void checkSwipe(Vector2 endPosition)
    {
        if (_startPosition == Vector2.zero)
            return;

        float distance = Vector2.Distance(_startPosition, endPosition);
        if (distance >= MinSwipeDistance)
        {
            print("good swipe" + distance);
        }
        else
        {
            print("too close" + distance);
        }
    }
}
