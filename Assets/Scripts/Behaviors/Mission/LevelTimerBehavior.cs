using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerBehavior : MonoBehaviour
{
    private static LevelTimerBehavior _instance = default;
    [SerializeField] private Slider _slider = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private float _levelTime = default;
    [SerializeField] private bool _timeFinished = default;

    public static LevelTimerBehavior Instance => Utils.GetInstance(ref _instance);
    public Slider Slider => _slider;

    public void FeedStaticData(float levelTime)
    {
        _levelTime = levelTime;
        _slider.maxValue = _levelTime;
    }

    void Update()
    {
        if (_slider.value < _slider.maxValue)
        {
            _slider.value = Time.timeSinceLevelLoad;
            return;
        }

        if (_timeFinished == true)
            return;

        _timeFinished = true;
        Debug.Log("Time ended!");
    }
}
