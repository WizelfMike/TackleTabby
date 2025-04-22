using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [Header("Combo Value Setting")]
    [SerializeField]
    private List<ComboSlot> ComboSlots = new();
    [SerializeField]
    private ComboTracker ComboTracker;
    [Header("Animation")]
    [SerializeField]
    private string OpenTriggerName;
    [SerializeField]
    private string CloseTriggerName;

    private Queue<Match> _progressQueue = new();
    private List<Match> _progress = new();

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(UpdateComboUI);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
        
        ResetComboUI();
    }

    private void UpdateComboUI(Match match)
    {
        // if (_progress.Count == 0)
        //     ResetComboUI();
        
        _progressQueue.Enqueue(match);
        HandleProgressQueue();
    }

    private void OnComboFinished(Combo combo)
    {
        // Invoke(nameof(OnComboFinishedDelayed), 0.5f);
        StartCoroutine(OnComboFinishedDelayed());
    }

    private IEnumerator OnComboFinishedDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        ResetComboUI();

        yield return new WaitForSeconds(0.5f);
        ClearProgress();
        while (true)
        {
            if (!HandleProgressQueue())
                break;
        }
    }

    private void ClearProgress()
    {
        _progress.Clear();
    }

    private void EnableSlot(int index, bool enable = true)
    {
        ComboSlots[index].BaitAnimator.SetTrigger(enable ? OpenTriggerName : CloseTriggerName);
    }

    private bool HandleProgressQueue()
    {
        if (_progress.Count >= ComboSlots.Count)
            return false;
        
        Match match = _progressQueue.Dequeue();
        _progress.Add(match);
        EnableSlot(_progress.Count - 1, true);
        ComboSlots[_progress.Count - 1].BaitMatchImage.sprite = match.BaitType.BaitSprite;
        ComboSlots[_progress.Count - 1].BaitMatchSizeText.SetText($"{match.MatchSize}x");
        return true;
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
