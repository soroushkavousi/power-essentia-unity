using System.Collections;
using System.Collections.Generic;

public class ObserverCollection
{
    private readonly List<IObserver> _observers = new();

    public void Add(IObserver observer)
    {
        if (observer != null)
            _observers.Add(observer);
    }
    public void Remove(IObserver observer) => _observers.Remove(observer);
    public void Notify(ISubject subject)
    {
        GameManagerBehavior.Instance.StartCoroutine(NotifyWithCoroutine(subject));
    }
    private IEnumerator NotifyWithCoroutine(ISubject subject)
    {
        foreach (var observer in _observers)
        {
            if (observer == null)
            {
                Remove(observer);
                continue;
            }
            observer.OnNotify(subject);
            yield return null;
        }
    }
}

public class ObserverCollection<TChange>
{
    private readonly List<IObserver<TChange>> _observers = new();

    public void Add(IObserver<TChange> observer)
    {
        if (observer != null)
            _observers.Add(observer);
    }
    public void Remove(IObserver<TChange> observer) => _observers.Remove(observer);
    public void Notify(ISubject<TChange> subject, TChange change)
    {
        GameManagerBehavior.Instance.StartCoroutine(NotifyWithCoroutine(subject, change));
    }
    private IEnumerator NotifyWithCoroutine(ISubject<TChange> subject, TChange change)
    {
        foreach (var observer in _observers)
        {
            if (observer == null)
            {
                Remove(observer);
                continue;
            }
            observer.OnNotify(subject, change);
            yield return null;
        }
    }
}
