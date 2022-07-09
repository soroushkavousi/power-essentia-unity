using UnityEngine;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
public class AboutDialogBehavior : MonoBehaviour
{
    private InteractionLayerPageBehavior _interactionLayerComponentBehavior = default;

    private void Awake()
    {
        _interactionLayerComponentBehavior = GetComponent<InteractionLayerPageBehavior>();
    }

    public void Show()
    {
        _interactionLayerComponentBehavior.Enable();
    }

    public void Exit()
    {
        _interactionLayerComponentBehavior.Disable();
    }
}
