using System.Collections.Generic;

public class ObserverCollection
{
    private readonly List<IObserver> _observers = new();

    public void Add(IObserver observer)
    {
        if (observer != null && !_observers.Contains(observer))
            _observers.Add(observer);
    }
    public void Remove(IObserver observer) => _observers.Remove(observer);
    public void Notify(ISubject subject)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            var currentObserver = _observers[i];
            if (currentObserver == null)
            {
                _observers.RemoveAt(i);
                continue;
            }
            currentObserver.OnNotify(subject);
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
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            var currentObserver = _observers[i];
            if (currentObserver == null)
            {
                _observers.RemoveAt(i);
                continue;
            }
            currentObserver.OnNotify(subject, change);
        }
    }
}
