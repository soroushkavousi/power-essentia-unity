using System.Collections.Generic;
using UnityEngine;

public class LayerManagerBehavior : MonoBehaviour
{
    [SerializeField] private List<InteractionLayerBehavior> _interactionLayers = default;

    private void Awake()
    {
        _interactionLayers.ForEach(il => il.gameObject.SetActive(true));
    }
}
