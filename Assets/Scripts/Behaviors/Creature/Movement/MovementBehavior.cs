using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovementBehavior : MonoBehaviour, IObserver, ISubject<MovementChangeData>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Number _speed;
    [SerializeField] private MoveType _moveType = default;
    [SerializeField] private Vector2 _direction = default;
    [SerializeField] private Vector2 _normalizedDirection = default;
    [SerializeField] private float _animationSpeed = 0f;
    [SerializeField] private float _startDelay = 0f;
    [SerializeField] private bool _dontStopWhenReach = default;
    [SerializeField] private float _acceptedCollisionDistance = default;
    [SerializeField] private Vector2 _targetPosition = default;
    [SerializeField] private Transform _targetTransform = default;
    [SerializeField] private static string _animationRatioName = "MovementSpeedRatio";
    private MovementStaticData _staticData = default;
    protected Level _level = default;
    private float _saveSpeedCurrentValue = default;
    private Rigidbody2D _rigidbody2d = default;
    private Animator _animator = default;
    private bool _isStopped = default;
    private readonly ObserverCollection<MovementChangeData> _observers = new();

    public Number Speed => _speed;
    public bool DontStopWhenReach { get => _dontStopWhenReach; set => _dontStopWhenReach = value; }

    public void FeedData(MovementStaticData staticData, Level level = null,
        float? acceptedCollisionDistance = null)
    {
        _staticData = staticData;
        _level = level;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _acceptedCollisionDistance = acceptedCollisionDistance ??
            GameManagerBehavior.Instance.Settings.AcceptedCollisionDistance;
        _animationSpeed = _staticData.AnimationSpeed;
        _startDelay = _staticData.StartDelay;
        _dontStopWhenReach = _staticData.DontStopWhenReach;

        _speed = new(_level, _staticData.SpeedLevelInfo,
            minPercentage: -40, maxPercentage: 40);
        _speed.Attach(this);

        _isStopped = false;
        StopMoving();
    }

    private void Start()
    {
        SetAnimationSpeed();
    }

    public void MoveWithDirection(Vector2 direction)
    {
        _moveType = MoveType.WITH_DIRECTION;
        _direction = direction;
        _normalizedDirection = _direction.normalized;
        StartMoving();
    }

    public void MoveToPosition(Vector2 position)
    {
        _moveType = MoveType.TO_POSITION;
        _targetPosition = position;
        _direction = _targetPosition - (Vector2)transform.position;
        _normalizedDirection = _direction.normalized;
        StartMoving();
    }

    public void MoveToTarget(Transform targetTransform)
    {
        _moveType = MoveType.TO_TARGET;
        _targetTransform = targetTransform;
        _direction = _targetTransform.position - transform.position;
        _normalizedDirection = _direction.normalized;
        StartMoving();
    }

    private void FixedUpdate()
    {
        if (_speed.Value == 0 || _direction == Vector2.zero)
            return;
        switch (_moveType)
        {
            case MoveType.WITH_DIRECTION:
                ContinueMovingWithDirection();
                break;
            case MoveType.TO_POSITION:
                ContinueMovingToPosition();
                break;
            case MoveType.TO_TARGET:
                ContinueMovingToTarget();
                break;
        }
    }

    private void ContinueMovingWithDirection()
    {
        MoveOneStepWithNormalizedDirection();
    }

    private void ContinueMovingToPosition()
    {
        var distance = Vector2.Distance(transform.position, _targetPosition);
        if (_dontStopWhenReach || distance > _acceptedCollisionDistance)
            MoveOneStepWithNormalizedDirection();
        else
        {
            _rigidbody2d.MovePosition(_targetPosition);
            StopMoving();
            Notify(new MovementChangeData(MovementChangeState.REACHED));
        }
    }

    private void ContinueMovingToTarget()
    {
        if (_targetTransform == null)
        {
            if (_targetPosition == Vector2.zero)
                Notify(new MovementChangeData(MovementChangeState.REACHED));
            else
                MoveToPosition(_targetPosition);
            return;
        }

        _targetPosition = _targetTransform.position;
        _direction = _targetTransform.position - transform.position;
        _normalizedDirection = _direction.normalized;

        var distance = Vector2.Distance(transform.position, _targetTransform.position);
        if (distance > _acceptedCollisionDistance)
            MoveOneStepWithNormalizedDirection();
        else
        {
            _rigidbody2d.MovePosition(_targetTransform.position);
            StopMoving();
            Notify(new MovementChangeData(MovementChangeState.REACHED));
        }
    }

    private void MoveOneStepWithNormalizedDirection()
    {
        var movement = _normalizedDirection * _speed.Value * Time.fixedDeltaTime;
        _rigidbody2d.MovePosition((Vector2)transform.position + movement);
    }

    public void StopMoving()
    {
        if (_isStopped)
            return;
        _isStopped = true;
        _speed.Fix(0);
        Notify(new MovementChangeData(MovementChangeState.STOPPED));
    }

    private void StartMoving()
    {
        if (_isStopped == false)
            return;
        _isStopped = false;
        _speed.Unfix();
        if (gameObject.activeInHierarchy)
            StartCoroutine(StartMovingSmouthly());
        Notify(new MovementChangeData(MovementChangeState.STARTED));
    }

    private IEnumerator StartMovingSmouthly()
    {
        if (_startDelay == 0)
            yield break;
        _saveSpeedCurrentValue = _speed.Value;

        var CountOfSteps = Mathf.FloorToInt(_startDelay / 0.5f) + 1;
        var SpeedOfEachSteps = (_saveSpeedCurrentValue / CountOfSteps).Round();
        var currentSpeed = 0f;
        _speed.Fix(currentSpeed);
        do
        {
            yield return new WaitForSeconds(0.5f);
            currentSpeed += SpeedOfEachSteps;
            if (currentSpeed > _saveSpeedCurrentValue)
                currentSpeed = _saveSpeedCurrentValue;
            _speed.Fix(currentSpeed);
        }
        while (currentSpeed != _saveSpeedCurrentValue);
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _speed)
        {
            SetAnimationSpeed();
        }
    }

    private void SetAnimationSpeed()
    {
        try
        {
            if (_animator == null || !_animator.parameters.Any(p => p.name == _animationRatioName))
                return;
            var movementSpeedRatio = _speed.Value / _animationSpeed;
            _animator.SetFloat(_animationRatioName, movementSpeedRatio);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void Attach(IObserver<MovementChangeData> observer) => _observers.Add(observer);
    public void Detach(IObserver<MovementChangeData> observer) => _observers.Remove(observer);
    public void Notify(MovementChangeData changeData) => _observers.Notify(this, changeData);
}
