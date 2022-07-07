public interface IObserver
{
    void OnNotify(ISubject subject);
}

public interface IObserver<TChange>
{
    void OnNotify(ISubject<TChange> subject, TChange change);
}
