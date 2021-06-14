using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelLoseStaticData", 
    menuName = "StaticData/Mission/LevelLoseStaticData", order = 1)]
public class LevelLoseStaticData : ScriptableObject
{
    [SerializeField] private AudioClip _loseAudioClip = default;

    public AudioClip LoseAudioClip { get => _loseAudioClip; }
}
