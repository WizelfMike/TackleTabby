using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> ComboSlots = new();
    [SerializeField]
    private ComboTracker ComboTracker;

    private List<Match> _progress = new();

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(UpdateComboUI);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
    }

    private void UpdateComboUI(Match match)
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
