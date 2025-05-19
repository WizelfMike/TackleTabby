using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bucket : MonoBehaviour
{
    [SerializeField]
    [Range(5f, 240f)]
    private float WiggleTimeoutSeconds;
    [SerializeField]
    private string OnWiggleTriggerName;

    private DeltaTimer _WiggleTimer;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _WiggleTimer = new DeltaTimer(WiggleTimeoutSeconds)
        {
            OnTimerRanOut = OnWiggleTimerRanOut,
        };
    }

    private void Update()
    {
        if (_WiggleTimer.IsRunning)
            _WiggleTimer.Update(Time.deltaTime);
    }

    private void OnWiggleTimerRanOut()
    {
        _animator.SetTrigger(OnWiggleTriggerName);
        _WiggleTimer.Reset();
    }
}