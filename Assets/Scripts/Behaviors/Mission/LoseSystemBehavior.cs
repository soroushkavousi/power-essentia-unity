using System;
using UnityEngine;

public class LoseSystemBehavior : MonoBehaviour, IObserver
{
    private static LoseSystemBehavior _instance = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private bool _lose = default;
    private HealthBehavior _castleHealthBehavior = default;

    public static LoseSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public OrderedList<Action> OnLoseActions { get; } = new OrderedList<Action>();
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
        OnLoseActions.CallActionsSafely();
        DesicionCanvasManager.Instance.Show();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _castleHealthBehavior.Health)
        {
            CheckLoseCondition();
        }
    }
}
