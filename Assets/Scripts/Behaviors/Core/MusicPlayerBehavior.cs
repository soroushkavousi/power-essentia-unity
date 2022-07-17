using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayerBehavior : MonoBehaviour
{
    private static MusicPlayerBehavior _instance = default;

    [SerializeField] private AudioClip _clickSound = default;
    [SerializeField] private AudioClip _enemyDeathGoldRewardSound = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _mute = default;

    private AudioSource _audioSource = default;

    public static MusicPlayerBehavior Instance => Utils.GetInstance(ref _instance);
    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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

    public void StartAllSounds()
    {
        _mute = false;
        AudioListener.volume = 1f;
        AudioListener.pause = false;
    }

    public IEnumerator StopAllSounds(float delay)
    {
        _mute = true;
        if (delay != 0f)
        {
            yield return new WaitForSecondsRealtime(delay);
            var currentVolume = AudioListener.volume;
            while (currentVolume > 0)
            {
                if (_mute == false)
                    yield break;
                currentVolume = Mathf.Clamp(currentVolume - 0.2f, 0f, 1f);
                AudioListener.volume = currentVolume;
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
        AudioListener.volume = 0;
        AudioListener.pause = true;
    }
}
