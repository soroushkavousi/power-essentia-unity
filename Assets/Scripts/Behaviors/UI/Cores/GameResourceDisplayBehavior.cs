using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ResourceDisplayBehavior))]
public class GameResourceDisplayBehavior : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType = default;

    private ResourceDisplayBehavior _resourceDisplayBehavior = default;
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private OnePartAdvancedNumber _resourceAmount = default;

    void Awake()
    {
        _resourceDisplayBehavior = GetComponent<ResourceDisplayBehavior>();    
        _resourceDisplayBehavior.AmountText.text = "0";
        _resourceAmount = PlayerBehavior.Main.DynamicData.ResourceBox
            .ResourceBunches[_resourceType];
        _resourceAmount.OnNewValueActions.Add(ShowResourceChange);
        _resourceDisplayBehavior.AmountText.text = _resourceAmount.IntValue.ToString();
    }

    private void ShowResourceChange(NumberChangeCommand changeCommand)
    {
        _resourceDisplayBehavior.AmountText.text = _resourceAmount.IntValue.ToString();
    }
}
