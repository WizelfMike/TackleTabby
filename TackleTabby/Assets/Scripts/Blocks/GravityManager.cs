using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class GravityManager : MonoBehaviour
{
    [SerializeField]
    [Description("Strength in gravity, positive value is downwards")]
    private float GravityStrength = 9.81f;
    [SerializeField]
    private float TerminalVelocity = 1f;
    [SerializeField]
    private Collider2D ControllingCollider;
    [SerializeField]
    private float GroundDetectionDistance = 1f;

    public UnityEvent OnLanded = new();

    private bool _isFalling = false;
    private Vector3 _fallingDirection = Vector3.down;
    private Vector3 _acceleration = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private GridPlayField _parentField;


    private void Start()
    {
        _parentField = transform.parent.GetComponent<GridPlayField>();
    }

    private void Update()
    {
        if (_isFalling)
            Fall(Time.deltaTime);
    }

    public void StartFalling()
    {
        if (_isFalling)
            return;
        
        _isFalling = true;
        ControllingCollider.enabled = false;
    }

    private void StopFalling()
    {
        _velocity = Vector3.zero;
        _acceleration = Vector3.zero;
        _isFalling = false;
        ControllingCollider.enabled = true;
        
        OnLanded.Invoke();
    }

    private void Fall(float deltaTime)
    {
        float ground = CheckGroundLevel();
        if (transform.localPosition.y <= ground)
        {
            StopFalling();
            return;
        }

        transform.position += _velocity * deltaTime;
        _acceleration += _fallingDirection * (GravityStrength * deltaTime);
        _velocity += _acceleration * deltaTime;
        if (_velocity.magnitude >= TerminalVelocity)
            _velocity = _velocity.normalized * TerminalVelocity;
    }

    private float CheckGroundLevel()
    {
        Transform tform = transform;
        RaycastHit2D ground = Physics2D.Raycast(tform.position, Vector2.down, GroundDetectionDistance);
        if (!ground)
            return 0f;

        if (!ground.transform.TryGetComponent(out FieldBlock groundBlock))
            return 0f;

        return _parentField.GetLocalisedCoordinateUnclamped(0, groundBlock.VerticalPosition + 1).y;
    }
}