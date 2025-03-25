using System;
using UnityEngine;

[Serializable]
public class combo
{
    [SerializeField]
    private string ComboSlot1;

    [SerializeField]
    private string ComboSlot2;

    public void FillSlots(string slot1, string slot2)
    {
        ComboSlot1 = slot1;
        ComboSlot2 = slot2;
    }

    public void PrintSlots()
    {
        Debug.Log(ComboSlot1);
        Debug.Log(ComboSlot2);
    }
}
