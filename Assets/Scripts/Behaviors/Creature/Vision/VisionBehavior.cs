using UnityEngine;

public class VisionBehavior : MonoBehaviour, ISubject<CollideData>
{
    [SerializeField] private VisionAreaBehavior _visionAreaBehavior = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private VisionStaticData _staticData = default;
    [SerializeField] private bool _isCollidingDisabled = default;
    [SerializeField] private bool _isStarted = false;
    private readonly ObserverCollection<CollideData> _observerCollection = new();

    public bool IsCollidingDisabled { get => _isCollidingDisabled || !_isStarted; set => _isCollidingDisabled = value; }
    public VisionAreaBehavior VisionAreaBehavior => _visionAreaBehavior;

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

        var collideDate = new CollideData(CollideType.ENTER, targetCollider, IsCollidingDisabled);
        Notify(collideDate);
    }
    public void OnExit(Collider2D targetCollider)
    {
        if (!_isStarted)
            return;

        var collideDate = new CollideData(CollideType.EXIT, targetCollider, IsCollidingDisabled);
        Notify(collideDate);
    }

    public void Attach(IObserver<CollideData> observer) => _observerCollection.Add(observer);
    public void Detach(IObserver<CollideData> observer) => _observerCollection.Remove(observer);
    public void Notify(CollideData data) => _observerCollection.Notify(this, data);

}
