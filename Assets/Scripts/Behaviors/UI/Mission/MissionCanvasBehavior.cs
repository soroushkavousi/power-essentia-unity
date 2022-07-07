using System.Collections;
using UnityEngine;

public class MissionCanvasBehavior : MonoBehaviour
{
    [SerializeField] private AudioClip _startSound = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_startSound, 0.15f);
    }
}
