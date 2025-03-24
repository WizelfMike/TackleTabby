using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BaitMovement : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> buttons = new();

    public void OnPushedButton()
    {
        print("swipe");
    }
}
