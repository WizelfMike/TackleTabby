using UnityEngine;

public class FishContainer : MonoBehaviour
{
    [SerializeField]
    private float BoundaryBufferSize = 100f;
    
    private RectTransform _rectTransform;

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform)
                return _rectTransform;

            _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    public float BoundaryBuffer => BoundaryBufferSize;
}