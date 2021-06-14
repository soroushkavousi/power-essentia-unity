using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBehavior : MonoBehaviour
{
    [SerializeField] private VisionAreaBehavior _visionAreaBehavior = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private VisionStaticData _staticData = default;
    [SerializeField] private bool _isColliderDisabled = default;
    [SerializeField] private bool _isStarted = false;

    public bool IsColliderDisabled { get => _isColliderDisabled || !_isStarted; set => _isColliderDisabled = value; }
    public VisionAreaBehavior VisionAreaBehavior => _visionAreaBehavior;
    public OrderedList<Action<Collider2D>> OnUnconstrainedEnterActions { get; } = new OrderedList<Action<Collider2D>>();
    public OrderedList<Action<Collider2D>> OnUnconstrainedExitActions { get; } = new OrderedList<Action<Collider2D>>();
    public OrderedList<Action<Collider2D>> OnEnterActions { get; } = new OrderedList<Action<Collider2D>>();
    public OrderedList<Action<Collider2D>> OnExitActions { get; } = new OrderedList<Action<Collider2D>>();

    public void FeedData(VisionStaticData staticData)
    {
        _staticData = staticData;
        _visionAreaBehavior.Initialize(
            _staticData.CenterPoint, _staticData.Size);
    }

    private void Start()
    {
        _isStarted = true;
    }

    public void OnEnter(Collider2D targetCollider)
    {
        if (!_isStarted)
            return;

        OnUnconstrainedEnterActions.CallActionsSafely(targetCollider);

        if (IsColliderDisabled)
            return;

        if (targetCollider.IsTargetColliderDisabled())
            return;

        OnEnterActions.CallActionsSafely(targetCollider);
    }
    public void OnExit(Collider2D targetCollider)
    {
        if (!_isStarted)
            return;

        OnUnconstrainedExitActions.CallActionsSafely(targetCollider);

        if (IsColliderDisabled)
            return;

        if (targetCollider.IsTargetColliderDisabled())
            return;

        OnExitActions.CallActionsSafely(targetCollider);
    }
}
