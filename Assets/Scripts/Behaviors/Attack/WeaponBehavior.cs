using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]
    public OrderedList<Action> OnHitActions { get; } = new OrderedList<Action>();

    
    private void HandleHitEvent()
    {
        OnHitActions.CallActionsSafely();
    }
}
