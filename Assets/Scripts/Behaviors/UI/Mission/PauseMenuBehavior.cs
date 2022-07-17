using UnityEngine;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
public class PauseMenuBehavior : MonoBehaviour
{
    private static PauseMenuBehavior _instance = default;
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private InteractionLayerPageBehavior _interactionLayerComponentBehavior = default;

    public static PauseMenuBehavior Instance => Utils.GetInstance(ref _instance);

    private void Awake()
    {
        _interactionLayerComponentBehavior = GetComponent<InteractionLayerPageBehavior>();
    }

    public void Show()
    {
        _interactionLayerComponentBehavior.Enable();
        StartCoroutine(MusicPlayerBehavior.Instance.StopAllSounds(0f));
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
        MusicPlayerBehavior.Instance.StartAllSounds();
        Time.timeScale = 1f;
    }
}
