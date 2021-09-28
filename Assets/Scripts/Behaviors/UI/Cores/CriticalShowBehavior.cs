using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CriticalShowBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberText = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private float _number = default;

    public void FeedData(float number)
    {
        _number = Mathf.Abs(number);
        _numberText.text = _number.ToString();
    }

    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
}
