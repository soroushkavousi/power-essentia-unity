using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCanvasBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip _startSound = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_startSound, 0.15f);
    }
}
