using UnityEngine;

[CreateAssetMenu(fileName = "GameStaticData",
    menuName = "StaticData/Game/GameStaticData", order = 1)]
public class GameStaticData : ScriptableObject
{
    public DefaultStaticData Defaults = default;
    public PrefabStaticData Prefabs = default;
    public SettingsStaticData Settings = default;
}
