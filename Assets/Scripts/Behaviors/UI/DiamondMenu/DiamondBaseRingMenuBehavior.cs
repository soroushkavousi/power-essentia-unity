using Assets.Scripts.Enums;
using UnityEngine;

public class DiamondBaseRingMenuBehavior : MonoBehaviour
{
    private static DiamondBaseRingMenuBehavior _instance = default;
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public static DiamondBaseRingMenuBehavior Instance => Utils.GetInstance(ref _instance);
    public DiamondUpgradeMenuBehavior DiamondUpgradeMenuBehavior => _diamondUpgradeMenuBehavior;

    private void Awake()
    {
        _diamondUpgradeMenuBehavior.FeedData(PlayerBehavior.MainPlayer.DynamicData.SelectedItems.
                MenuDiamondName[RingName.BASE]);
    }
}
