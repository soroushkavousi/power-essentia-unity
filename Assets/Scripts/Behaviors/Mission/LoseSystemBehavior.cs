using UnityEngine;

public class LoseSystemBehavior : MonoBehaviour, ISubject, IObserver
{
    private static LoseSystemBehavior _instance = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private bool _lose = default;
    private HealthBehavior _castleHealthBehavior = default;
    private readonly ObserverCollection _observers = new();

    public static LoseSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public bool Lose => _lose;

    public void FeedData()
    {
        _lose = false;
        _castleHealthBehavior = WallBehavior.Instance.GetComponent<HealthBehavior>();
        _castleHealthBehavior.Health.Attach(this);
    }

    public void CheckLoseCondition()
    {
        if (_castleHealthBehavior.Health.Value > 0)
            return;

        if (_lose == true)
            return;

        _lose = true;
        Notify();
        DecisionCanvasManager.Instance.Show();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _castleHealthBehavior.Health)
        {
            CheckLoseCondition();
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}
