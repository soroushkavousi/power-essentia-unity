using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LayerManagerBehavior : MonoBehaviour
{
    [SerializeField] private List<InteractionLayerBehavior> _interactionLayers = default;

    private void Awake()
    {
        _interactionLayers.ForEach(il => il.gameObject.SetActive(true));
    }
}
