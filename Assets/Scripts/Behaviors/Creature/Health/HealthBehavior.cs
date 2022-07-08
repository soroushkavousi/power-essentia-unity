using UnityEngine;

public class HealthBehavior : MonoBehaviour, IObserver, ISubject
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected Health _health;
    [SerializeField] private bool _destroyOnDeath = default;
    [SerializeField] private GameObject _deathVfxPrefab = default;
    [SerializeField] protected bool _isDead = default;
    private HealthStaticData _healthStaticData = default;
    protected readonly ObserverCollection _observers = new();

    public Health Health => _health;
    public bool IsDead => _isDead;

    public void FeedData(HealthStaticData healthStaticData,
        Observable<int> level = null,
        bool destroyOnDeath = true)
    {
        _healthStaticData = healthStaticData;
        _health = new(_healthStaticData.Health, level, _healthStaticData.HealthLevelPercentage,
            _healthStaticData.PhysicalResistance, _healthStaticData.PhysicalResistanceLevelPercentage,
            _healthStaticData.MagicResistance, _healthStaticData.MagicResistanceLevelPercentage);
        _health.Attach(this);
        _deathVfxPrefab = _healthStaticData.DeathVfxPrefab;
        _destroyOnDeath = destroyOnDeath;
        _isDead = false;
    }

    protected virtual void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        Notify();
        TriggerDeathVfx();
        if (_destroyOnDeath)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }

    protected virtual void TriggerDeathVfx()
    {
        if (_deathVfxPrefab == null)
            return;

        var deathVfx = Instantiate(_deathVfxPrefab,
            transform.position, transform.rotation);
        Destroy(deathVfx, 2f);
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _health)
        {
            if (_health.Value <= 0)
                Die();
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}
