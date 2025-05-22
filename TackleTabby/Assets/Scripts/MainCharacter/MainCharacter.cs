using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class MainCharacter : MonoBehaviour
{
    [Header("ChildObjects")]
    [SerializeField]
    private Image CaughtFishDisplayImage;
    
    [Header("Animation")]
    [SerializeField]
    private string OnFirstBaitMatchBoolName;
    [SerializeField]
    private string OnCaughtFishTriggerName;
    [SerializeField]
    private string OnCaughtTrashTriggerName;
    [SerializeField]
    private string OnCaughtSpecificFishIntName;
    [SerializeField]
    private FishDefinition[] SpecificFishAnimations;
    [SerializeField]
    private string[] OnSleepTriggerNames;
    [SerializeField]
    private float SleepTimeoutSeconds = 2f;

    public UnityEvent OnLost = new();

    private Animator _animator;
    private int _onFirstBaitMatchBool = -1;
    private int _onCaughtFishTrigger = -1;
    private int _onCaughtTrashTrigger = -1;
    private bool _hasFirstBait = false;
    private bool _mayWalkAway;
    private Sprite _catchDisplaySprite = null;
    private DeltaTimer _sleepTimer;
    private int _createdFirstMatchCount = 0;


    private int CreatedFirstMatchCount
    {
        get => _createdFirstMatchCount;
        set
        {
            if (value < 0)
                return;

            _createdFirstMatchCount = value;
            _animator.SetBool(_onFirstBaitMatchBool, CreatedFirstMatchCount > 0);
        }
    }

    private void Start()
    {
        _sleepTimer = new DeltaTimer(SleepTimeoutSeconds)
        {
            OnTimerReset = OnSleepTimerReset,
            OnTimerRanOut = OnSleepTimerRanOut
        };

        _animator = GetComponent<Animator>();

        _onFirstBaitMatchBool = Animator.StringToHash(OnFirstBaitMatchBoolName);
        _onCaughtFishTrigger = Animator.StringToHash(OnCaughtFishTriggerName);
        _onCaughtTrashTrigger = Animator.StringToHash(OnCaughtTrashTriggerName);
    }

    private void Update()
    {
        if (_sleepTimer.IsRunning)
            _sleepTimer.Update(Time.deltaTime);
        
        if (_mayWalkAway)
            OnWalkAway(Time.deltaTime);
    }

    public void OnCreatedMatch()
    {
        if (_hasFirstBait)
            return;
        
        _hasFirstBait = true;
        CreatedFirstMatchCount++;
        _sleepTimer.Reset();
    }
    
    public void OnCaughtFish(CaughtFish fish)
    {
        if (!_hasFirstBait)
            return;

        _animator.SetInteger(
            OnCaughtSpecificFishIntName,
            Array.IndexOf(SpecificFishAnimations, fish.FishType.Expand())
        );

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

    private void OnWalkAway(float time)
    {
        float xPosition = transform.position.x - time * 1.5f;
        transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
    }

    public void PlayLoseAnimation()
    {
        _animator.SetTrigger("OnLoseGame");
    }
    
    public void EnableWalkAway()
    {
        OnLost.Invoke();
        _mayWalkAway = true;
    }
    
    private void DisableBaitBool()
    {
        CreatedFirstMatchCount--;
    }
}