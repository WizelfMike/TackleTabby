using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class ComboTracker : MonoBehaviour
{
    [SerializeField]
    private int ComboLength = 2;

    public UnityEvent<Combo> OnComboFinished;
    public UnityEvent<ComboEntry> OnComboUpdated;

    private Combo _endingCombo;
    private List<BaitDefinition> _baitList = new();

    public void UpdateCombo(BaitDefinition match)
    {
        _baitList.Add(match);
        OnComboUpdated?.Invoke(new ComboEntry { BaitType = match });

        if (_baitList.Count >=  ComboLength)
        {
            FinishCombo();
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        _baitList.Clear();
    }

    public void FinishCombo()
    {
        _endingCombo = new Combo()
        {
            Entries = _baitList.Select(match => new ComboEntry() { BaitType = match }).ToArray()
        };

        OnComboFinished?.Invoke(_endingCombo);
    }
}