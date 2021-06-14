using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovementBehavior : MonoBehaviour
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private MovementStaticData _staticData = default;
    [SerializeField] private ThreePartAdvancedNumber _speed = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private MoveType _moveType = default;
    [SerializeField] private Vector2 _direction = default;
    [SerializeField] private Vector2 _normalizedDirection = default;
    [SerializeField] private float _startDelay = 0f;
    [SerializeField] private bool _dontStopWhenReach = default;
    [SerializeField] private float _acceptedTargetDistance = default;
    [SerializeField] private Vector2 _targetPosition = default;
    [SerializeField] private Transform _targetTransform = default;
    private float _saveSpeedCurrentValue = default;
    private Rigidbody2D _rigidbody2d = default;
    private Animator _animator = default;
    private bool _isStopped = default;

    public ThreePartAdvancedNumber Speed => _speed;
    public bool DontStopWhenReach { get => _dontStopWhenReach; set => _dontStopWhenReach = value; }
    public Vector2 Direction 
    { 
        get => _direction; 
        set 
        {
            _moveType = MoveType.WITH_DIRECTION;
            _direction = value; 
            _normalizedDirection = _direction.normalized; 
        } 
    }
    public Vector2 TargetPosition
    {
        get => _targetPosition;
        set
        {
            _moveType = MoveType.TO_POSITION;
            _targetPosition = value;
            _direction = _targetPosition - (Vector2)transform.position;
            _normalizedDirection = _direction.normalized;
        }
    }
    public Transform TargetTransform
    {
        get => _targetTransform;
        set
        {
            _moveType = MoveType.TO_TARGET;
            _targetTransform = value;
            _direction = _targetTransform.position - transform.position;
            _normalizedDirection = _direction.normalized;
        }
    }
    public OrderedList<Action> OnStartMovingActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnStopMovingActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnReachActions { get; } = new OrderedList<Action>();

    public void FeedData(MovementStaticData staticData)
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _acceptedTargetDistance = 30f;

        _staticData = staticData;
        if (_animator != null && _animator.parameters.Any(p => p.name == "MovementSpeedRatio"))
        {
            _speed.Current.OnNewValueActions.Add(SetAnimationSpeed);
            SetAnimationSpeed(null);
        }
        _speed.FeedData(_staticData.Speed);
        _startDelay = _staticData.StartDelay;
        if (_staticData.StartDirection != Vector2.zero)
            Direction = _staticData.StartDirection;
        _isStopped = false;
        _dontStopWhenReach = _staticData.DontStopWhenReach;
        StopMoving();
    }

    private void Start()
    {
        //foreach (var parameter in _animator.parameters)
        //{
        //    Debug.Log($"{name} | Animator Parameter: {parameter.name} {parameter.type}");
        //}
    }

    private void FixedUpdate()
    {
        switch (_moveType)
        {
            case MoveType.WITH_DIRECTION:
                MoveWithDirection();
                break;
            case MoveType.TO_POSITION:
                MoveToPosition();
                break;
            case MoveType.TO_TARGET:
                MoveToTarget();
                break;
        }
    }

    private void SetAnimationSpeed(NumberChangeCommand changeCommand)
    {
        var movementSpeedRatio = _speed.Value / _staticData.AnimationSpeed;
        _animator.SetFloat("MovementSpeedRatio", movementSpeedRatio);
    }

    private void MoveWithDirection()
    {
        var speedValue = _speed.Value;
        if (speedValue != 0 && _direction != Vector2.zero)
        {
            var movement = _normalizedDirection * speedValue * Time.fixedDeltaTime;
            _rigidbody2d.MovePosition((Vector2)transform.position + movement);
        }
    }

    private void MoveToPosition()
    {
        var speedValue = _speed.Value;
        if (speedValue != 0 && _direction != Vector2.zero)
        {
            var distance = Vector2.Distance(transform.position, _targetPosition);
            if (_dontStopWhenReach || distance > _acceptedTargetDistance)
            {
                var movement = _normalizedDirection * speedValue * Time.fixedDeltaTime;
                _rigidbody2d.MovePosition((Vector2)transform.position + movement);
            }
            else
            {
                _rigidbody2d.MovePosition(_targetPosition);
                StopMoving();
                OnReachActions.CallActionsSafely();
            }
        }
    }

    private void MoveToTarget()
    {
        var speedValue = _speed.Value;
        if (_targetTransform == null)
        {
            if (_targetPosition == default)
                OnReachActions.CallActionsSafely();
            else
                TargetPosition = _targetPosition;
            return;
        }

        _targetPosition = _targetTransform.position;
        _direction = _targetTransform.position - transform.position;
        _normalizedDirection = _direction.normalized;
        if (speedValue != 0 && _direction != Vector2.zero)
        {
            var distance = Vector2.Distance(transform.position, _targetTransform.position);
            if (distance > _acceptedTargetDistance)
            {
                var movement = _normalizedDirection * speedValue * Time.fixedDeltaTime;
                _rigidbody2d.MovePosition((Vector2)transform.position + movement);
            }
            else
            {
                _rigidbody2d.MovePosition(_targetTransform.position);
                StopMoving();
                OnReachActions.CallActionsSafely();
            }
        }
    }

    public void StopMoving()
    {
        if (_isStopped)
            return;
        _isStopped = true;
        _speed.Current.Fix(0);
        OnStopMovingActions.CallActionsSafely();
    }

    public void StartMoving()
    {
        if (_isStopped == false)
            return;
        _isStopped = false;
        //if (_speed.CurrentValue.Result != 0)
        //    return;
        _speed.Current.UnFix();
        StartCoroutine(StartMovingSmouthly());
        OnStartMovingActions.CallActionsSafely();
    }

    private IEnumerator StartMovingSmouthly()
    {
        if (_startDelay == 0)
            yield break;
        _saveSpeedCurrentValue = _speed.Value;
        _speed.Current.Change(-_saveSpeedCurrentValue, name, MovementChangeType.MOVING_SMOUTHLY, "Reset moving to zero");

        var startStepsCount = Mathf.FloorToInt(_startDelay / 0.5f) + 1;
        var startStepsVelocity = (_saveSpeedCurrentValue / startStepsCount).Round();
        for (int i = 0; i < startStepsCount - 1; i++)
        {
            _speed.Current.Change(startStepsVelocity,
                name, MovementChangeType.MOVING_SMOUTHLY, "Start moving smouthly");
            yield return new WaitForSeconds(0.5f);
        }
        _speed.Current.Change(_saveSpeedCurrentValue - startStepsVelocity * (startStepsCount - 1),
            name, MovementChangeType.MOVING_SMOUTHLY, "Start moving smouthly");
    }


    //private void FixedUpdate()
    //{
    //    var speedValue = _speed.CurrentValue;
    //    if (speedValue != 0 && _direction != Vector2.zero)
    //    {
    //        var movement = transform.rotation * (_normalizedDirection * speedValue * Time.fixedDeltaTime);
    //        _rigidbody2d.MovePosition(transform.position + movement);
    //    }
    //}

    //private void Update()
    //{
    //    var speedValue = _speed.CurrentValue;
    //    if (speedValue != 0 && _direction != Vector2.zero)
    //    {
    //        var movement = transform.rotation * (_normalizedDirection * speedValue);
    //        _rigidbody2d.velocity = movement;
    //        //_rigidbody2d.velocity = _normalizedDirection * speedValue * Time.deltaTime;
    //    }
    //}

    //private void Update()
    //{
    //    var speedValue = _speed.CurrentValue;
    //    if (speedValue != 0 && _direction != Vector2.zero)
    //    {
    //        transform.Translate(_normalizedDirection * speedValue * Time.deltaTime, Space.Self);
    //    }
    //}
}
