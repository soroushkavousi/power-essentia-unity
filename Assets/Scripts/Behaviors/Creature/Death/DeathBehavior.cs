using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Death : IObserver, ISubject
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private bool _isDead = default;
    [SerializeField] private bool _dontDestroyOnDeath = default;
    [SerializeField] private GameObject _deathVfxPrefab = default;

    private HealthBehavior _healthBehavior;
    private CurrentHealth _currentHealth;
    private readonly List<IObserver> _observers = new();

    public bool IsDead => _isDead;

    public Death(HealthBehavior healthBehavior, GameObject deathVfxPrefab)
    {
        _healthBehavior = healthBehavior;
        _currentHealth = _healthBehavior.CurrentHealth;
        _currentHealth.Attach(this);
        _deathVfxPrefab = deathVfxPrefab;
    }

    public void Update(ISubject subject)
    {
        if (subject.GetType().IsSubclassOf(typeof(CurrentHealth)) == false)
            return;
        if (_currentHealth.Value <= 0)
            Die();
    }

    public void Die()
    {
        _isDead = true;
        Notify();
        TriggerDeathVfx();
        if (_dontDestroyOnDeath == false)
            UnityEngine.Object.Destroy(_healthBehavior.gameObject);
    }

    private void TriggerDeathVfx()
    {
        if (_deathVfxPrefab == null)
            return;

        var deathVfx = UnityEngine.Object.Instantiate(_deathVfxPrefab, 
            _healthBehavior.transform.position, _healthBehavior.transform.rotation);
        UnityEngine.Object.Destroy(deathVfx, 2f);
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.ForEach(observer => observer.Update(this));
}
