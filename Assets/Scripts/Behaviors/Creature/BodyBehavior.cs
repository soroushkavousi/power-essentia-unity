using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BodyBehavior : MonoBehaviour
{
    [SerializeField] private List<BodyAreaBehavior> _bodyAreaBehaviors = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private bool _isColliderDisabled = default;
    [SerializeField] private bool _isStarted = false;

    public bool IsColliderDisabled { get => _isColliderDisabled || !_isStarted; set => _isColliderDisabled = value; }
    public OrderedList<Action<Collider2D>> OnUnconstrainedEnterActions { get; } = new OrderedList<Action<Collider2D>>();
    public OrderedList<Action<Collider2D>> OnUnconstrainedExitActions { get; } = new OrderedList<Action<Collider2D>>();
    public OrderedList<Action<Collider2D>> OnEnterActions { get; } = new OrderedList<Action<Collider2D>>();
    public OrderedList<Action<Collider2D>> OnExitActions { get; } = new OrderedList<Action<Collider2D>>();

    public void FeedData()
    {
        _bodyAreaBehaviors.ForEach(bodyAreaBehavior 
            => bodyAreaBehavior.FeedStaticData(OnEnter, OnExit));
    }

    private void Start()
    {
        //Debug.Log($"BodyBehavior Start 1 _bodyBehavior.IsColliderDisabled: {IsColliderDisabled}");
        _isStarted = true;
    }

    private void OnEnter(Collider2D targetCollider)
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

    private void OnExit(Collider2D targetCollider)
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
