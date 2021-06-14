using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
public class PauseMenuBehavior : MonoBehaviour
{
    private static PauseMenuBehavior _instance = default;
    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private InteractionLayerPageBehavior _interactionLayerComponentBehavior = default;

    public static PauseMenuBehavior Instance => Utils.GetInstance(ref _instance);

    private void Awake()
    {
        _interactionLayerComponentBehavior = GetComponent<InteractionLayerPageBehavior>();
    }

    public void Show()
    {
        _interactionLayerComponentBehavior.Enable();
    }

    public void ExitFromMission()
    {
        SceneManagerBehavior.Instance.LoadCountry();
    }

    public void ResetMission()
    {
        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void ResumeMission()
    {
        _interactionLayerComponentBehavior.Disable();
        Time.timeScale = 1f;
    }
}
