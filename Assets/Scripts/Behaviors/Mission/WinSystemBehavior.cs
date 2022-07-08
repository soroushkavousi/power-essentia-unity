using System;
using UnityEngine;

public class WinSystemBehavior : MonoBehaviour, IObserver
{
    private static WinSystemBehavior _instance = default;
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private bool _win = default;

    public static WinSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public OrderedList<Action> OnWinActions { get; } = new OrderedList<Action>();
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
        OnWinActions.CallActionsSafely();
        DesicionCanvasManager.Instance.Show();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == LevelManagerBehavior.Instance.Finished)
        {
            CheckWinCondition();
        }
    }
}
