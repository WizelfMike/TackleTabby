using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ComboTracker : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> ComboSlots = new();

    [SerializeField]
    private combo _endingCombo = new combo();

    private int _comboIndex = 1;

    public void SetNextComboText(string combo)
    {
        if (_comboIndex == 3)
            ResetComboText();

        ComboSlots[_comboIndex-1].SetText(combo);
        _comboIndex++;

        if (_comboIndex == 3)
            SetGivenCombo();
    }

    public void ResetComboText()
    {
        ComboSlots[0].text = null;
        ComboSlots[1].text = null;
        _comboIndex = 1;
    }

    public void SetGivenCombo()
    {
        _endingCombo.FillSlots(ComboSlots[0].text.ToString(), ComboSlots[1].text.ToString());
    }
}