using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class MainCharacter : MonoBehaviour
{
    [Header("ChildObjects")]
    [SerializeField]
    private Image CaughtFishDisplayImage;
    
    [Header("Animation")]
    [SerializeField]
    private string OnFirstBaitMatchTriggerName;
    [SerializeField]
    private string OnCaughtFishTriggerName;
    [SerializeField]
    private string OnCaughtTrashTriggerName;
    [SerializeField]
    private string[] OnSleepTriggerNames;
    [SerializeField]
    private float SleepTimeoutSeconds = 2f;

    private Animator _animator;
    private int _onFirstBaitMatchTrigger = -1;
    private int _onCaughtFishTrigger = -1;
    private int _onCaughtTrashTrigger = -1;
    private bool _hasFirstBait = false;
    private Sprite _catchDisplaySprite = null;
    private DeltaTimer _sleepTimer;

    private void Start()
    {
        _sleepTimer = new DeltaTimer(SleepTimeoutSeconds)
        {
            OnTimerReset = OnSleepTimerReset,
            OnTimerRanOut = OnSleepTimerRanOut
        };
            
        _animator = GetComponent<Animator>();

        _onFirstBaitMatchTrigger = Animator.StringToHash(OnFirstBaitMatchTriggerName);
        _onCaughtFishTrigger = Animator.StringToHash(OnCaughtFishTriggerName);
        _onCaughtTrashTrigger = Animator.StringToHash(OnCaughtTrashTriggerName);
    }

    private void Update()
    {
        if (_sleepTimer.IsRunning)
            _sleepTimer.Update(Time.deltaTime);
    }

    public void OnCreatedMatch()
    {
        if (_hasFirstBait)
            return;
        
        _hasFirstBait = true;
        _animator.SetTrigger(_onFirstBaitMatchTrigger);
        _sleepTimer.Reset();
    }
    
    public void OnCaughtFish(CaughtFish fish)
    {
        if (!_hasFirstBait)
            return;

        _hasFirstBait = false;
        _catchDisplaySprite = fish.FishType.Expand().FishSprite;
        CaughtFishDisplayImage.sprite = _catchDisplaySprite;
        CaughtFishDisplayImage.rectTransform.pivot = fish.FishType.Expand().MouthPivot;
        _animator.SetTrigger(_onCaughtFishTrigger);
        _sleepTimer.Reset();
    }

    public void OnCaughtTrash(TrashDefinition trashType)
    {
        if (!_hasFirstBait)
            return;

        _hasFirstBait = false;
        _catchDisplaySprite = trashType.TrashSprite;
        CaughtFishDisplayImage.sprite = _catchDisplaySprite;
        CaughtFishDisplayImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _animator.SetTrigger(_onCaughtTrashTrigger);
        _sleepTimer.Reset();
    }

    private void OnSleepTimerReset()
    { 
        foreach (string onSleepTriggerName in OnSleepTriggerNames) 
            _animator.ResetTrigger(onSleepTriggerName);       
    }

    private void OnSleepTimerRanOut()
    {
        foreach (string onSleepTriggerName in OnSleepTriggerNames)
            _animator.ResetTrigger(onSleepTriggerName);
        
        _animator.SetTrigger(OnSleepTriggerNames[Random.Range(0, OnSleepTriggerNames.Length)]);
    }
}