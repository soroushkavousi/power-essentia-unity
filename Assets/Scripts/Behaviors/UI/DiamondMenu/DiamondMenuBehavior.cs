using UnityEngine;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
[RequireComponent(typeof(MenuHeaderBehavior))]
public class DiamondMenuBehavior : MonoBehaviour
{
    private static DiamondMenuBehavior _instance = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private InteractionLayerPageBehavior _interactionLayerPageBehavior = default;

    public static DiamondMenuBehavior Instance => Utils.GetInstance(ref _instance);

    private void Awake()
    {
        _interactionLayerPageBehavior = GetComponent<InteractionLayerPageBehavior>();
    }

    public void Show()
    {
        _interactionLayerPageBehavior.Enable();
    }

    public void Hide()
    {
        _interactionLayerPageBehavior.Disable();
    }
}
