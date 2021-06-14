using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(HealthBarBehavior))]
public class NumberedHealthBarBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberText = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private HealthBarBehavior _healthBarBehavior = default;

    private void Awake()
    {
        _healthBarBehavior = GetComponent<HealthBarBehavior>();
        _healthBarBehavior.OnShowHealthActions.Add(ShowHealthChange);
    }

    private void ShowHealthChange()
    {
        var maxValue = _healthBarBehavior.Slider.maxValue;
        var currentValue = _healthBarBehavior.Slider.value;
        _numberText.text = $"{currentValue} / {maxValue}";
    }
}
