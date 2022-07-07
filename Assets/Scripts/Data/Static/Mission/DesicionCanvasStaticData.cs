using UnityEngine;

[CreateAssetMenu(fileName = "MissionOverviewStaticData",
    menuName = "StaticData/Mission/MissionOverviewStaticData", order = 1)]
public class DesicionCanvasStaticData : ScriptableObject
{
    public AudioClip WinAudioClip = default;
    public AudioClip LoseAudioClip = default;
}
