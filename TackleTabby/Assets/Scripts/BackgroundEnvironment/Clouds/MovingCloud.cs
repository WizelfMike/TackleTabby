using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MovingCloud : MonoBehaviour
{
    public CloudPool ObjectPool { get; set; }
    public float Speed { get; set; }

    public float Lifetime
    {
        get => _lifetime;
        set
        {
            _lifetime = value;
            _lifeTimeTimer = new DeltaTimer(_lifetime)
            {
                OnTimerRanOut = OnLifeRanOut
            };
        }
    }

    public Sprite Sprite
    {
        set => GetComponent<Image>().sprite = value;
    }
    
    private float _lifetime;
    private DeltaTimer _lifeTimeTimer;

    private void Update()
    {
        transform.position += Vector3.right * (Speed * Time.deltaTime);
        
        if (_lifeTimeTimer is not null &&  _lifeTimeTimer.IsRunning)
            _lifeTimeTimer.Update(Time.deltaTime);
    }

    private void OnLifeRanOut()
    {
        ObjectPool.Store(this);
    }
}