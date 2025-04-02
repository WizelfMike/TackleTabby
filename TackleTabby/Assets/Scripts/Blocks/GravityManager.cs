using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class GravityManager : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField]
    [Description("Strength in gravity, positive value is downwards")]
    private float GravityStrength = 9.81f;
    [SerializeField]
    private float TerminalVelocity = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    private float MaxLossyScaleDistance = 0.5f;
    [Header("Detection")]
    [SerializeField]
    private float GroundDetectionDistance = 1f;
    [Header("Collision")]
    [SerializeField]
    private Collider2D ControllingCollider;

    public UnityEvent<GravityManager> OnLanded = new();

    private bool _isFalling = false;
    private Vector3 _fallingDirection = Vector3.down;
    private Vector3 _acceleration = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private GridPlayField _parentField;

    private void Start()
    {
        _parentField = transform.parent.GetComponent<GridPlayField>();
    }

    private void FixedUpdate()
    {
        if (_isFalling)
        {
            TestGround();
            Fall(Time.fixedDeltaTime);
        }
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
        OnLanded.Invoke(this);
        _velocity = Vector3.zero;
        _acceleration = Vector3.zero;
        _isFalling = false;
        ControllingCollider.enabled = true;
    }

    private void TestGround()
    {
        float ground = CheckGroundLevel();
        if (transform.localPosition.y <= ground)
            StopFalling();
    }

    private void Fall(float deltaTime)
    {
        if (!_isFalling)
            return;

        UpdateVelocityPosition(deltaTime);
    }

    private void UpdateVelocityPosition(float deltaTime)
    {
        transform.position += SafeguardVelocity(_velocity * deltaTime);
         
        _acceleration += _fallingDirection * (GravityStrength * deltaTime);
        _velocity += _acceleration * deltaTime;
        if (_velocity.magnitude >= TerminalVelocity)
            _velocity = _velocity.normalized * TerminalVelocity;       
    }

    private Vector3 SafeguardVelocity(Vector3 velocity)
    {
        if (velocity.magnitude >= transform.lossyScale.y * MaxLossyScaleDistance)
            return velocity.normalized * (transform.lossyScale.y * MaxLossyScaleDistance);
        return velocity;
    }

    private float CheckGroundLevel()
    {
        RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, GroundDetectionDistance);
        if (!ground)
            return 0f;

        if (!ground.transform.TryGetComponent(out FieldBlock groundBlock))
            return 0f;

        return _parentField.GetLocalisedCoordinateUnclamped(0, groundBlock.VerticalPosition + 1).y;
    }
}