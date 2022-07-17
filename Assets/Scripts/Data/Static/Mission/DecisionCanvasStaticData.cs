using UnityEngine;

[CreateAssetMenu(fileName = "DecisionCanvasStaticData",
    menuName = "StaticData/Mission/DecisionCanvasStaticData", order = 1)]
public class DecisionCanvasStaticData : ScriptableObject
{
    public AudioClip WinAudioClip = default;
    public AudioClip LoseAudioClip = default;
    public AudioClip VictorySoundClip = default;
}
