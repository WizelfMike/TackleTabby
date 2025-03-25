using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class ComboTracker : MonoBehaviour
{
    [SerializeField]
    private int ComboLength = 2;
    [SerializeField]
    private Combo _endingCombo;

    public UnityEvent<Combo> OnComboFinished;
    public UnityEvent<ComboEntry> OnComboUpdated;

    private List<BaitDefinition> _baitList = new();

    public void UpdateCombo(BaitDefinition match)
    {
        _baitList.Add(match);
        OnComboUpdated?.Invoke(new ComboEntry { BaitType = match });

        if (_baitList.Count >=  ComboLength)
        {
            SetGivenCombo();
            ResetCombo();
        }
    }

    public void ResetCombo()
    {
        _baitList.Clear();
    }

    public void SetGivenCombo()
    {
        _endingCombo = new Combo()
        {
            Entries = _baitList.Select(match => new ComboEntry() { BaitType = match }).ToArray()
        };

        OnComboFinished?.Invoke(_endingCombo);
    }
}