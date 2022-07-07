using System.Collections.Generic;
using UnityEngine;

public class BodyBehavior : MonoBehaviour, ISubject<CollideData>
{
    [SerializeField] private List<BodyAreaBehavior> _bodyAreaBehaviors = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _isCollidingDisabled = default;
    [SerializeField] private bool _isStarted = false;
    private readonly ObserverCollection<CollideData> _observers = new();
    private readonly ObserverCollection<CollideData> _superObservers = new();

    public bool IsCollidingDisabled { get => _isCollidingDisabled || !_isStarted; set => _isCollidingDisabled = value; }

    public void FeedData()
    {
        _bodyAreaBehaviors.ForEach(bodyAreaBehavior
            => bodyAreaBehavior.FeedData(OnEnter, OnExit));
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

        var collideDate = new CollideData(CollideType.ENTER, targetCollider, IsCollidingDisabled);
        Notify(collideDate);
    }

    private void OnExit(Collider2D targetCollider)
    {
        if (!_isStarted)
            return;

        var collideDate = new CollideData(CollideType.EXIT, targetCollider, IsCollidingDisabled);
        Notify(collideDate);
    }

    public void Attach(IObserver<CollideData> observer) => _observers.Add(observer);
    public void Detach(IObserver<CollideData> observer) => _observers.Remove(observer);
    public void Notify(CollideData data) => _observers.Notify(this, data);
}
