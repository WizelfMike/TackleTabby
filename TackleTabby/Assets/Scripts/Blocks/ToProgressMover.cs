using System;
using UnityEngine;
using UnityEngine.Events;

public class ToProgressMover : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField]
    private RectTransform Target;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float Duration;

    [Header("Precision")]
    [SerializeField]
    [Range(0f, 25f)]
    private float MinStopDistance;

    [Header("Conversion")]
    [SerializeField]
    private Camera Camera;

    [Header("Other")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    public UnityEvent<ToProgressMover> OnDoneMoving = new();

    private Canvas _targetCanvas;
    private Vector3 _targetPosition;
    private bool _isMoving;
    private SpriteMaskInteraction _defaultMaskInteraction;

    private void Start()
    {
        _defaultMaskInteraction = SpriteRenderer.maskInteraction;
        Transform obj = Target;
        while (obj != null)
        {
            if (obj.TryGetComponent(out Canvas can))
            {
                _targetCanvas = can;
                break;
            }

            obj = obj.parent;
        }
    }

    private void OnDisable()
    {
        ForceStopMoving();
    }

    private void Update()
    {
        if (_isMoving)
            MoveToTarget(Time.deltaTime);
    }

    private void MoveToTarget(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, deltaTime * (1f / Duration));
        if (Vector3.Distance(transform.position, _targetPosition) <= MinStopDistance)
            ForceStopMoving();
    }

    public void StartMoving()
    {
        if (Target == null || _isMoving)
            return;
        
        SpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform) _targetCanvas.transform,
            Target.position, Camera, out _targetPosition);
        
        _isMoving = true;
    }

    public void ForceStopMoving()
    {
        if (!_isMoving)
            return;
        
        SpriteRenderer.maskInteraction = _defaultMaskInteraction;
        _isMoving = false;
        OnDoneMoving.Invoke(this);
    }
}