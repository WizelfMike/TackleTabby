using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class ComboTracker : MonoBehaviour
{
    [SerializeField]
    private int ComboLength = 2;

    public UnityEvent<Combo> OnComboFinished;
    public UnityEvent<Match> OnComboUpdated;

    private Combo _endingCombo;
    private List<Match> _comboList = new();

    public void UpdateCombo(Match match)
    {
        _comboList.Add(match);
        OnComboUpdated?.Invoke(match);

        if (_comboList.Count >=  ComboLength)
        {
            FinishCombo();
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        _comboList.Clear();
    }

    private void FinishCombo()
    {
        _endingCombo = new Combo
        {
            Entries = _comboList.ToArray()
        };

        OnComboFinished?.Invoke(_endingCombo);
    }
}