using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutBoxBehavior : MonoBehaviour
{
    private static OutBoxBehavior _instance = default;
    [SerializeField] private Transform _location1 = default;
    [SerializeField] private Transform _location2 = default;
    [SerializeField] private Transform _location3 = default;
    [SerializeField] private Transform _location4 = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]


    public static OutBoxBehavior Instance => Utils.GetInstance(ref _instance);
    public Transform Location1 => _location1;
    public Transform Location2 => _location2;
    public Transform Location3 => _location3;
    public Transform Location4 => _location4;
}
