using UnityEngine;
using UnityEngine.Events;

public class HungerTracking : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private int StomachSize;

    public UnityEvent OnStarving = new();
    public UnityEvent<int> OnSatiationChanged = new();

    private int _satiation;

    private void Start()
    {
        _satiation = StomachSize;
    }

    public void Feed(CaughtFish fish)
    {
        _satiation += fish.SatiationAmount;
        if (_satiation >= StomachSize)
            _satiation = StomachSize;

        OnSatiationChanged?.Invoke(_satiation);
    }

    public void ApplyHunger(int hunger = 1)
    {
        if (_satiation <= 0)
            return;
        
        _satiation -= hunger;
        OnSatiationChanged?.Invoke(_satiation);
        if (_satiation <= 0)
            IsStarving();
    }

    private void IsStarving()
    {
        OnStarving?.Invoke();
    }
}
