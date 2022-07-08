﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayerBehavior : MonoBehaviour
{
    private static MusicPlayerBehavior _instance = default;

    [SerializeField] private AudioClip _clickSound = default;
    [SerializeField] private AudioClip _enemyDeathGoldRewardSound = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private AudioSource _audioSource = default;

    public static MusicPlayerBehavior Instance => Utils.GetInstance(ref _instance);
    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Instance == null)
            Debug.Log($"MusicPlayerBehavior Instance is null");
    }

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(_clickSound);
    }

    public void PlayEnemyDeathGoldRewardSound()
    {
        _audioSource.PlayOneShot(_enemyDeathGoldRewardSound, 0.5f);
    }

    private void OnMasterVolumeChange()
    {
        //_audioSource.volume = Options.Instance.MasterVolume;
    }

    private void PlayClipAtPosition()
    {

    }
}
