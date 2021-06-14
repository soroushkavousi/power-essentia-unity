using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuHeaderBehavior : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _alpha = default;
    [SerializeField] private List<ButtonBehavior> _headerButtons = default;
    [SerializeField] private List<GameObject> _subMenus= default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private void Start()
    {
        ShowSubMenu(0);
    }

    public void ShowSubMenu(int index)
    {
        ButtonBehavior currentHeaderButton;
        GameObject currentSubMenu;
        Color currentHeaderImageColor;
        for (int i = 0; i < _headerButtons.Count; i++)
        {
            currentHeaderButton = _headerButtons[i];
            currentSubMenu = _subMenus[i];
            currentHeaderImageColor = currentHeaderButton.TargetGraphic.color;
            if (i == index)
            {
                currentHeaderImageColor.a = 1f;
                currentSubMenu.SetActive(true);
            }
            else
            {
                currentHeaderImageColor.a = _alpha;
                currentSubMenu.SetActive(false);
            }

            currentHeaderButton.TargetGraphic.color = currentHeaderImageColor;
        }
    }
}
