using System;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayBehavior : MonoBehaviour
{
    [SerializeField] private Text _textComponent = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

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
