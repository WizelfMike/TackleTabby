using UnityEngine;

public class EnvironmentFish : MonoBehaviour
{
    [SerializeField]
    private float SteeringStrength = 0.85f;
    [SerializeField]
    private float Velocity = 0.6f;
    
    private RectTransform _rectTransform;
    private FishContainer _parentContainer;
    private Vector3 _currentDirection = Vector3.right;
    private Vector3 _targetDirection = Vector3.right;

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
        SetDirection(new Vector3(1f, 1f, 0f));
    }

    private void Update()
    {
        HandleWalls();
        _currentDirection = Vector3.MoveTowards(_currentDirection, _targetDirection, SteeringStrength * Time.deltaTime);
        RectTransform.position += _currentDirection * (Velocity * Time.deltaTime);
    }

    private Directions CheckWalls()
    {
        if (RectTransform.anchoredPosition.x - RectTransform.rect.width / 2 < _parentContainer.BoundaryBuffer)
            return Directions.Left;

        if (RectTransform.anchoredPosition.x + RectTransform.rect.width / 2 >
            _parentContainer.RectTransform.rect.width - _parentContainer.BoundaryBuffer)
            return Directions.Right;

        if (RectTransform.anchoredPosition.y - RectTransform.rect.height / 2 < _parentContainer.BoundaryBuffer)
            return Directions.Down;

        if (RectTransform.anchoredPosition.y + RectTransform.rect.height / 2 >
            _parentContainer.RectTransform.rect.height - _parentContainer.BoundaryBuffer)
            return Directions.Up;

        return Directions.None;
    }

    private void HandleWalls()
    {
        Directions hittingWall = CheckWalls();
        switch (hittingWall)
        {
            case Directions.Left:
                if (Mathf.Sign(_targetDirection.x) < 0)
                    _targetDirection *= new Vector2(-1f, 1f);
                break;
            case Directions.Right:
                if (Mathf.Sign(_targetDirection.x) > 0)
                    _targetDirection *= new Vector2(-1f, 1f);
                break;
            case Directions.Up:
                if (Mathf.Sign(_targetDirection.y) > 0)
                    _targetDirection *= new Vector2(1f, -1f);
                break;
            case Directions.Down:
                if (Mathf.Sign(_targetDirection.y) < 0)
                    _targetDirection *= new Vector2(1f, -1f);
                break;
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        newDirection *= Vector2.one;
        newDirection.Normalize();
        _currentDirection = newDirection;
        _targetDirection = newDirection;
    }
}