using System;

public class DeltaTimer
{
    public Action OnTimerRanOut;

    private float _currentTimeout = 0f;
    private bool _hasInvoked = false;
    
    public float Timeout { get; }
    public bool IsRunning => !_hasInvoked;

    public DeltaTimer(float timeout)
    {
        Timeout = timeout;
    }

    public void Reset()
    {
        _currentTimeout = Timeout;
        _hasInvoked = false;
    }

    public void Prolong()
    {
        _currentTimeout += Timeout;
        _hasInvoked = false;
    }

    public void Update(float delta)
    {
        if (_hasInvoked)
            return;
        
        _currentTimeout -= delta;
        if (_currentTimeout <= 0)
        {
            _hasInvoked = true;
            _currentTimeout = 0;
            OnTimerRanOut?.Invoke();
        }
    }
}