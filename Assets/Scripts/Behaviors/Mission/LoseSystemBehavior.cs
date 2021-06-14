using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoseSystemBehavior : MonoBehaviour
{
    private static LoseSystemBehavior _instance = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private bool _lose = default;
    private HealthBehavior _castleHealthBehavior = default;

    public static LoseSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public OrderedList<Action> OnLoseActions { get; } = new OrderedList<Action>();
    public bool Lose => _lose;

    public void FeedData()
    {
        _castleHealthBehavior = FindObjectOfType<WallBehavior>(true).GetComponent<HealthBehavior>();
        _castleHealthBehavior.Health.Current.OnNewValueActions.Add(CheckLoseCondition);
    }

    public void Restart()
    {
        _lose = false;
    }

    public void CheckLoseCondition(NumberChangeCommand changeCommand)
    {
        if (_castleHealthBehavior.Health.Value > 0)
            return;

        if (_lose == true)
            return;

        _lose = true;
        OnLoseActions.CallActionsSafely();
        DesicionCanvasManager.Instance.Show();
    }
}
