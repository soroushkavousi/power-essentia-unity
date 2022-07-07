public interface ISubject
{
    public void Attach(IObserver observer);
    public void Detach(IObserver observer);
    public void Notify();
}

public interface ISubject<TData>
{
    public void Attach(IObserver<TData> observer);
    public void Detach(IObserver<TData> observer);
    public void Notify(TData data);
}
