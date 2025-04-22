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
    [SerializeField]
    [Range(0f, 2f)]
    private float ResetUIDelay = 0.2f;
    [SerializeField]
    [Range(0f, 2f)]
    private float ClearProgressDelay = 0.2f;

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
        _progressQueue.Enqueue(match);
        HandleProgressQueue();
    }

    private void OnComboFinished(Combo combo)
    {
        StartCoroutine(OnComboFinishedDelayed());
    }

    private IEnumerator OnComboFinishedDelayed()
    {
        yield return new WaitForSeconds(ResetUIDelay);
        ResetComboUI();

        yield return new WaitForSeconds(ClearProgressDelay);
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
        if (_progress.Count >= ComboSlots.Count || _progressQueue.Count <= 0)
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
