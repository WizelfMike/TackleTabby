using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private string DisplayBoolName;
    [SerializeField]
    [Range(0f, 2f)]
    private float ResetUIDelay = 0.2f;
    [SerializeField]
    [Range(0f, 2f)]
    private float ClearProgressDelay = 0.2f;

    private Queue<Match> _progressQueue = new();
    private List<Match> _progress = new();
    private Mutex _syncMutex = new Mutex();

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(UpdateComboUI);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
    }

    private void UpdateComboUI(Match match)
    {
        _syncMutex.WaitOne();
        
        _progressQueue.Enqueue(match);
        HandleProgressQueue();
        
        _syncMutex.ReleaseMutex();
    }

    private void OnComboFinished(Combo combo)
    {
        StartCoroutine(OnComboFinishedDelayed());
    }

    private IEnumerator OnComboFinishedDelayed()
    {
        _syncMutex.WaitOne();
        
        yield return new WaitForSeconds(ResetUIDelay);
        ResetComboUI();

        yield return new WaitForSeconds(ClearProgressDelay);
        ClearProgress();
        
        while (_progressQueue.Count > ComboSlots.Count)
            for (int i = 0; i < ComboSlots.Count; i++)
                _ = _progressQueue.Dequeue();

        for (int i = 0; i < ComboSlots.Count; i++)
            if (!HandleProgressQueue())
                break;
        
        _syncMutex.ReleaseMutex();
    }

    private void ClearProgress()
    {
        _progress.Clear();
    }

    private void EnableSlot(int index, bool enable = true)
    {
        ComboSlots[index].BaitAnimator.SetBool(DisplayBoolName, enable);
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
