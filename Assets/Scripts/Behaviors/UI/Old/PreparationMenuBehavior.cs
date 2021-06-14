using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PreparationMenuBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public void StartMission()
    {
        SceneManagerBehavior.Instance.LoadMission();
    }

    public void LoadLevel()
    {
        MusicPlayerBehavior.Instance.PlayClickSound();
        SceneManagerBehavior.Instance.LoadMission();
    }

    public void LoadDiamondMenu()
    {
        DiamondMenuBehavior.Instance.Show();
    }
}
