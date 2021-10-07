using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
[RequireComponent(typeof(AudioSource))]
public class DesicionCanvasManager : MonoBehaviour
{
    private static DesicionCanvasManager _instance = default;
    [SerializeField] private MissionOverviewStaticData _baseData = default;
    [SerializeField] private GameObject _winCanvas = default;
    [SerializeField] private TextMeshProUGUI _winTitle = default;
    [SerializeField] private GameObject _loseCanvas = default;
    [SerializeField] private TextMeshProUGUI _loseTitle = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private InteractionLayerPageBehavior _interactionLayerPageBehavior = default;
    private AudioSource _audioSource = default;
    private OnePartAdvancedNumber _selectedDemonLevel = default;

    public static DesicionCanvasManager Instance => Utils.GetInstance(ref _instance);

    private void Awake()
    {
        _interactionLayerPageBehavior = GetComponent<InteractionLayerPageBehavior>();
        _audioSource = GetComponent<AudioSource>();
        _selectedDemonLevel = PlayerBehavior.Main.DynamicData.SelectedItems.DemonLevel;
    }

    public void GoNextLevel()
    {
        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void GoPreviousLevel()
    {
        _selectedDemonLevel.Change(-1, name, "PREVIOUS");
        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void RepeatOrTryAgainLevel()
    {
        if (WinSystemBehavior.Instance.Win)
            _selectedDemonLevel.Change(-1, name, "REPEAT");

        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void EndMission()
    {
        SceneManagerBehavior.Instance.LoadCountry();
    }

    public void Show()
    {
        if(WinSystemBehavior.Instance.Win)
        {
            _winCanvas.SetActive(true);
            _loseCanvas.SetActive(false);
            _winTitle.text = $"Congragulations! You have completed level {_selectedDemonLevel.Value}.";
            _selectedDemonLevel.Change(1, name, "VICTORY");
        }
        else if(LoseSystemBehavior.Instance.Lose)
        {
            _winCanvas.SetActive(false);
            _loseCanvas.SetActive(true);
            _loseTitle.text = $"You have failed on level {_selectedDemonLevel.Value}.";
        }
        _interactionLayerPageBehavior.Enable();
        Time.timeScale = 0f;
        _audioSource.PlayOneShot(_baseData.WinAudioClip, 0.3f);
    }

    public void Hide()
    {
        _interactionLayerPageBehavior.Disable();
    }
}
