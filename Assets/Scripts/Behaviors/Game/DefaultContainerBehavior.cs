using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefaultContainerBehavior : MonoBehaviour
{
    private static DefaultContainerBehavior _instance = default;
    [SerializeField] private Sprite _diamondImage = default;
    [SerializeField] private string _diamondName = default;

    public Sprite DiamondImage => _diamondImage;
    public string DiamondName => _diamondName;

    public static DefaultContainerBehavior Instance => Utils.GetInstance(ref _instance);

    public void FeedData()
    {

    }
}
