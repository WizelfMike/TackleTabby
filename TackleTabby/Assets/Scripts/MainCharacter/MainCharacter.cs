using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainCharacter : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    private string OnFirstBaitMatchTriggerName;
    [SerializeField]
    private string OnCaughtTriggerName;
    [SerializeField]
    private string[] OnSleepTriggerNames;
    [SerializeField]
    private float SleepTimeoutSeconds = 2f;

    private Animator _animator;
    private int _onFirstBaitMatchTrigger = -1;
    private int _onCaughtTrigger = -1;
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
        _onCaughtTrigger = Animator.StringToHash(OnCaughtTriggerName);
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
        _animator.SetTrigger(_onCaughtTrigger);
        _sleepTimer.Reset();
    }

    public void OnCaughtTrash(TrashDefinition trashType)
    {
        if (!_hasFirstBait)
            return;

        _hasFirstBait = false;
        _catchDisplaySprite = trashType.TrashSprite;
        _animator.SetTrigger(_onCaughtTrigger);
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