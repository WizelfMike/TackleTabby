using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> ComboSlots = new();
    [SerializeField]
    private ComboTracker ComboTracker;

    private List<ComboEntry> _progress = new();

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(UpdateComboUI);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
    }

    private void UpdateComboUI(ComboEntry match)
    {
        if (_progress.Count == 0)
            ResetComboUI();

        _progress.Add(match);
        ComboSlots[_progress.Count - 1].SetText(match.BaitType.DisplayName);
    }

    private void OnComboFinished(Combo combo)
    {
        ClearProgress();
    }

    private void ClearProgress()
    {
        _progress.Clear();
    }

    public void ResetComboUI()
    {
        ComboSlots[0].SetText("");
        ComboSlots[1].SetText("");
    }
}
