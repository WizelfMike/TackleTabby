using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MovingCloud : MonoBehaviour
{
    public CloudPool ObjectPool { get; set; }
    
    public RectTransform ParentRect { private get; set; }
    
    public float Speed { get; set; }
    
    public Sprite Sprite
    {
        set => GetComponent<Image>().sprite = value;
    }
    
    private RectTransform _ownRect;

    private void Awake()
    {
        _ownRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        transform.position += Vector3.right * (Speed * Time.deltaTime);
        PositionalCheck();
    }

    private void PositionalCheck()
    {
        if (_ownRect.anchoredPosition.x > ParentRect.rect.width + 1 * (transform.localScale.x * _ownRect.rect.width))
            ObjectPool.Store(this);
    }
}