using UnityEngine;

[CreateAssetMenu(fileName = "GameStaticData",
    menuName = "StaticData/Game/GameStaticData", order = 1)]
public class GameStaticData : ScriptableObject
{
    public SettingsStaticData Settings = default;
    public DefaultStaticData Defaults = default;
    public PrefabStaticData Prefabs = default;
}
