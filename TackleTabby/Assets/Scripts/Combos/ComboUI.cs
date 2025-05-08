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
    private bool _updatingCombo = false;

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(UpdateComboUI);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
    }

    private void UpdateComboUI(Match match)
    {
        StartCoroutine(UpdateComboUICoroutine(match));
    }

    private void OnComboFinished(Combo combo)
    {
        StartCoroutine(OnComboFinishedDelayed());
    }

    private IEnumerator UpdateComboUICoroutine(Match match)
    {
        Match value = match;
        
        yield return new WaitUntil(() => !_updatingCombo);
        _updatingCombo = true;
        
        _progressQueue.Enqueue(value);
        HandleProgressQueue();
        
        _updatingCombo = false;
    }

    private IEnumerator OnComboFinishedDelayed()
    {
        yield return new WaitUntil(() => !_updatingCombo);
        _updatingCombo = true;
        
        yield return new WaitForSeconds(ResetUIDelay);
        ResetComboUI();
        ClearProgress();

        yield return new WaitForSeconds(ClearProgressDelay);
        for (int i = 0; i < ComboSlots.Count; i++)
            if (!HandleProgressQueue())
                break;

        _updatingCombo = false;
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
