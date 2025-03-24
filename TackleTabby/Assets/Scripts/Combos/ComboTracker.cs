using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ComboTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ComboSlotA;

    [SerializeField]
    private TextMeshProUGUI ComboSlotB;

    [SerializeField]
    private List<combo> BaitComboParts;

    private int comboIndex = 1;

    public void SetNextCombo(string combo)
    {
        if (comboIndex == 1)
            ComboSlotA.text = combo; comboIndex++;
        if (comboIndex == 2)
            ComboSlotB.text = combo; comboIndex++;
        if (comboIndex > 2)
            ComboSlotA.text = null; ComboSlotB.text = null; comboIndex = 0;
    }
}
