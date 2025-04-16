using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainCharacter : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    private string OnFirstBaitMatchTriggerName;
    [SerializeField]
    private string OnCaughtTriggerName;


    private Animator _animator;
    private int _onFirstBaitMatchTrigger = -1;
    private int _onCaughtTrigger = -1;
    private bool _hasFirstBait = false;
    private Sprite _catchDisplaySprite = null;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _onFirstBaitMatchTrigger = Animator.StringToHash(OnFirstBaitMatchTriggerName);
        _onCaughtTrigger = Animator.StringToHash(OnCaughtTriggerName);
    }

    public void OnCreatedMatch()
    {
        if (_hasFirstBait)
            return;
        
        _hasFirstBait = true;
        _animator.SetTrigger(_onFirstBaitMatchTrigger);
    }
    
    public void OnCaughtFish(CaughtFish fish)
    {
        if (!_hasFirstBait)
            return;

        _hasFirstBait = false;
        _catchDisplaySprite = fish.FishType.Expand().FishSprite;
        _animator.SetTrigger(_onCaughtTrigger);
    }

    public void OnCaughtTrash(TrashDefinition trashType)
    {
        if (!_hasFirstBait)
            return;

        _hasFirstBait = false;
        _catchDisplaySprite = trashType.TrashSprite;
        _animator.SetTrigger(_onCaughtTrigger);
    }
}