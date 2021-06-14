using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarBehavior : MonoBehaviour
{
    [SerializeField] private Image _fill = default;
    [SerializeField] private HealthBehavior _healthBehavior = default;
    [SerializeField] private Gradient _gradient = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private Slider _slider = default;

    public Slider Slider => _slider;
    public OrderedList<Action> OnShowHealthActions { get; } = new OrderedList<Action>();

    public void Awake()
    {
        _slider = GetComponent<Slider>();
        _healthBehavior.Health.OnNewValueActions.Add(ShowHealthChange);
    }

    private void ShowHealthChange(NumberChangeCommand changeCommand)
    {
        _slider.maxValue = _healthBehavior.Health.Peak.Value;
        _slider.value = _healthBehavior.Health.Value;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        OnShowHealthActions.CallActionsSafely();
    }
}
