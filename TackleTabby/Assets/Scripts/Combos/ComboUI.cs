using System.Collections.Generic;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField]
    private List<ComboSlot> ComboSlots = new();
    [SerializeField]
    private ComboTracker ComboTracker;

    private List<Match> _progress = new();

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(UpdateComboUI);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
        
        ResetComboUI();
    }

    private void UpdateComboUI(Match match)
    {
        if (_progress.Count == 0)
            ResetComboUI();

        _progress.Add(match);
        EnableSlot(_progress.Count - 1, true);
        ComboSlots[_progress.Count - 1].BaitMatchImage.sprite = match.BaitType.BaitSprite;
        ComboSlots[_progress.Count - 1].BaitMatchSizeText.SetText($"{match.MatchSize}x");
    }

    private void OnComboFinished(Combo combo)
    {
        ClearProgress();
    }

    private void ClearProgress()
    {
        _progress.Clear();
    }

    private void EnableSlot(int index, bool enable = true)
    {
        ComboSlots[index].BaitMatchImage.enabled = enable;
        ComboSlots[index].BaitMatchSizeText.enabled = enable;
    }

    public void ResetComboUI()
    {
        int slotCount = ComboSlots.Count;
        for (int i = 0; i < slotCount; i++)
        {
            EnableSlot(i, false);
        }
    }
}
