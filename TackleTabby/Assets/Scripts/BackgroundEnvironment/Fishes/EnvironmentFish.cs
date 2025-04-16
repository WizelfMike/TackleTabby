using UnityEngine;

public class EnvironmentFish : MonoBehaviour
{
    [SerializeField]
    private float SteeringStrength = 0.4f;
    
    private RectTransform _rectTransform;
    private FishContainer _parentContainer;
    private Vector3 _currentDirection = Vector3.right;
    private Vector3 _targetDirection = Vector3.down;

    private DeltaTimer _turnTimeout = new DeltaTimer(1f);
    
    private float velocity = 1f;

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

    private void Start()
    {
        _parentContainer = transform.parent.GetComponent<FishContainer>();
    }

    private void Update()
    {
        if (_turnTimeout.IsRunning)
            _turnTimeout.Update(Time.deltaTime);
        
        HandleWalls();
        _currentDirection = Vector3.MoveTowards(_currentDirection, _targetDirection, SteeringStrength * Time.deltaTime);
        RectTransform.position += _currentDirection * (velocity * Time.deltaTime);
    }

    private Directions CheckWalls()
    {
        // TODO! Calculate position correct
        if (RectTransform.position.x - RectTransform.rect.width / 2  - _parentContainer.BoundaryBuffer < float.Epsilon)
            return Directions.Left;

        if (RectTransform.position.x + RectTransform.rect.width / 2 + _parentContainer.BoundaryBuffer >
            _parentContainer.RectTransform.rect.width)
            return Directions.Right;

        if (RectTransform.position.y - RectTransform.rect.height / 2 - _parentContainer.BoundaryBuffer < float.Epsilon)
            return Directions.Down;

        if (RectTransform.position.y + RectTransform.rect.height / 2 + _parentContainer.BoundaryBuffer >
            _parentContainer.RectTransform.rect.height)
            return Directions.Up;

        return Directions.None;
    }

    private void HandleWalls()
    {
        if (_turnTimeout.IsRunning)
            return;
        
        Directions hittingWall = CheckWalls();
        if (!hittingWall.HasFlag(Directions.None))
        {
            _turnTimeout.Reset();
            _targetDirection = -_targetDirection;
        }
    }
    
}