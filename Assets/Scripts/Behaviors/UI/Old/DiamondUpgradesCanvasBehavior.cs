using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondUpgradesCanvasBehavior : MonoBehaviour
{
    private CarouselViewBehavior _carouselViewBehavior = default;

    private void Awake()
    {
        _carouselViewBehavior = transform
            .GetComponentInChildren<CarouselViewBehavior>();
        _carouselViewBehavior.Initialize();
    }
}
