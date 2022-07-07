using UnityEngine;

public class PhysicalResistance : IObserver, ISubject
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected float _value;
    [SerializeField] protected float _startValue;
    [SerializeField] protected float _levelPercentage;
    [SerializeField] protected float _maxPercentage;
    protected readonly Observable<int> _level;
    private readonly ObserverCollection _observers = new();

    public float Value => _value;

    public PhysicalResistance(float startValue, Observable<int> level = null,
        float levelPercentage = 0f, float? maxPercentage = null)
    {
        _value = _startValue = startValue;
        _level = level;
        _level?.Attach(this);
        _levelPercentage = levelPercentage;
        _maxPercentage = maxPercentage ?? 99f; //Todo Global Constants
    }

    protected virtual void CalculateValue()
    {
        float percentage = 100f;
        if (_level != null)
        {
            var levelOverallPercentage = _level.Value * _levelPercentage;
            percentage += levelOverallPercentage;
        }
        _value = _startValue.MeasurePercentage(percentage);

        if (_value > _maxPercentage)
            _value = _maxPercentage;

        Notify();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _level)
            CalculateValue();
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}
