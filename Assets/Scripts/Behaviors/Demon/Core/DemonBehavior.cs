using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(DemonHealthBehavior))]
[RequireComponent(typeof(StatusOwnerBehavior))]
public abstract class DemonBehavior : MonoBehaviour, ISubject,
    IObserver<CollideData>, IObserver
{
    [Header(Constants.HeaderStart + nameof(DemonBehavior) + Constants.HeaderEnd)]
    [SerializeField] private DemonName _name = default;
    //[SerializeField] private UnityEvent _InitializeEvent;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private DemonState _state = default;
    [SerializeField] protected Observable<int> _level;
    [SerializeField] protected bool _isInAttackArea = default;
    private DemonStaticData _demonStaticData = default;

    protected BodyBehavior _bodyBehavior = default;
    protected DemonHealthBehavior _healthBehavior = default;
    protected StatusOwnerBehavior _statusOwnerBehavior = default;
    protected static int _temp = 0;
    protected readonly ObserverCollection _observers = new();

    public DemonName Name => _name;
    public DemonState State => _state;
    public bool IsInAttackArea => _isInAttackArea;
    public Observable<int> Level => _level;
    public DemonHealthBehavior HealthBehavior => _healthBehavior;

    public virtual void Initialize(int level)
    {
        _level = new(level);
        _temp++;
    }

    public void FeedData(DemonStaticData demonStaticData)
    {
        _demonStaticData = demonStaticData;
        _healthBehavior = GetComponent<DemonHealthBehavior>();
        _healthBehavior.FeedData(_demonStaticData.HealthData, _level);
        _healthBehavior.Attach(this);

        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _bodyBehavior.IsCollidingDisabled = true;
        _bodyBehavior.Attach(this);

        _statusOwnerBehavior = GetComponent<StatusOwnerBehavior>();
        _statusOwnerBehavior.FeedData();
        _statusOwnerBehavior.BurnStatusBehavior.FeedData();
        _statusOwnerBehavior.StunStatusBehavior.FeedData();
        StartCoroutine(OnAfterInitialization());
    }

    private IEnumerator OnAfterInitialization()
    {
        yield return null;
        _state = DemonState.ALIVE;
        Notify();
    }

    protected virtual void FlipXRandomly()
    {
        if (Random.value >= 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.RotateAround(transform.position, transform.up, 180f);
            //_movementBehavior.Direction = 1;
        }
    }

    private void OnDeath()
    {
        _state = DemonState.DEAD;
        Notify();
        MusicPlayerBehavior.Instance?.PlayEnemyDeathGoldRewardSound();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"2222 Teeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeest");
    }

    public void OnNotify(ISubject<CollideData> subject, CollideData collideData)
    {
        if (collideData.Type == CollideType.ENTER
            && collideData.TargetCollider2D.gameObject.name == "DemonsAttackArea")
        {
            _isInAttackArea = true;
            _bodyBehavior.IsCollidingDisabled = false;
        }
    }

    protected GameObject IsTargetEnemy(GameObject target)
    {
        var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior != null)
        {
            target = bodyAreaBehavior.BodyBehavior.gameObject;
        }

        var castleBehavior = target.GetComponent<WallBehavior>();
        if (castleBehavior != null)
            return target;

        return null;
    }

    public void OnNotify(ISubject subject)
    {
        if (ReferenceEquals(subject, _healthBehavior))
        {
            if (_healthBehavior.IsDead)
                OnDeath();
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}
