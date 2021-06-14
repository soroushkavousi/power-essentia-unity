using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinSystemBehavior : MonoBehaviour
{
    private static WinSystemBehavior _instance = default;
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private bool _win = default;

    public static WinSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public OrderedList<Action> OnWinActions { get; } = new OrderedList<Action>();
    public bool Win => _win;

    public void FeedData()
    {
        LevelManagerBehavior.Instance.Finished.OnNewValueActions.Add(CheckWinCondition);
    }

    public void Restart()
    {
        _win = false;
    }

    public void CheckWinCondition(BooleanChangeCommand changeCommand)
    {
        if (LevelManagerBehavior.Instance.Finished.Value == false)
            return;

        if (_win)
            return;

        _win = true;
        OnWinActions.CallActionsSafely();
        DesicionCanvasManager.Instance.Show();
    }
}
