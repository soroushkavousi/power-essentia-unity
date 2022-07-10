﻿using System.Collections;
using UnityEngine;

public class MissionSceneBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip _startSound = default;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_startSound, 0.15f);
    }
}