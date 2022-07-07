using UnityEngine;

public class InteractionLayerPageBehavior : MonoBehaviour
{
    [SerializeField] private InteractionLayerBehavior _interactionLayerBehavior = default;

    public void Enable()
    {
        gameObject.SetActive(true);
        _interactionLayerBehavior.gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        _interactionLayerBehavior.gameObject.SetActive(false);
    }
}
