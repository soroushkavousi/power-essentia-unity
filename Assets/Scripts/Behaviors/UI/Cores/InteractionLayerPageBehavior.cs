using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
