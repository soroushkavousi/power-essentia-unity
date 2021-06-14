using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayBehavior : MonoBehaviour
{
    [SerializeField] private Text _textComponent = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private Func<string> _getTextFunction = default;

    void Update()
    {
        _textComponent.text = _getTextFunction();
    }

    public void FeedData(Func<string> getTextFunction)
    {
        _getTextFunction = getTextFunction;
    }
}
