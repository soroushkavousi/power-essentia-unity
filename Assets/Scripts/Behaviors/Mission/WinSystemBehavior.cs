using UnityEngine;

public class WinSystemBehavior : MonoBehaviour, ISubject, IObserver
{
    private static WinSystemBehavior _instance = default;
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private bool _win = default;
    private readonly ObserverCollection _observers = new();

    public static WinSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public bool Win => _win;

    public void FeedData()
    {
        _win = false;
        LevelManagerBehavior.Instance.Finished.Attach(this);
    }

    public void CheckWinCondition()
    {
        if (LevelManagerBehavior.Instance.Finished.Value == false)
            return;

        if (_win)
            return;

        _win = true;
        Notify();
        DecisionCanvasManager.Instance.Show();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == LevelManagerBehavior.Instance.Finished)
        {
            CheckWinCondition();
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}
