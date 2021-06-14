using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountryBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public void Fight()
    {
        SceneManagerBehavior.Instance.LoadMission();
    }
}
