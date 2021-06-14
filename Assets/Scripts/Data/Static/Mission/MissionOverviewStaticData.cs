using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelWinStaticData", 
    menuName = "StaticData/Mission/LevelWinStaticData", order = 1)]
public class MissionOverviewStaticData : ScriptableObject
{
    [SerializeField] private AudioClip _winAudioClip = default;

    public AudioClip WinAudioClip { get => _winAudioClip; }
}
