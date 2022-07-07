using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NavigationBarBehavior : MonoBehaviour
{
    private List<GameObject> _pages = default;
    private List<Button> _buttons = default;

    private void Awake()
    {
        FindPages();
        FindButtons();
    }

    private void Start()
    {
        InitializeButtons();
    }

    private void FindPages()
    {
        _pages = new List<GameObject>();
        foreach (Transform child in transform.parent.Find("Pages"))
        {
            if (child.name == "Background")
                continue;
            _pages.Add(child.gameObject);
        }
    }

    private void FindButtons()
    {
        _buttons = GetComponentsInChildren<Button>().ToList();
    }

    private void InitializeButtons()
    {
        //_buttons[0].gameObject.SetActive(true);
        _buttons[0].Select();
        for (int i = 0; i < _buttons.Count; i++)
        {
            var index = i;
            _buttons[i].onClick.AddListener(() => ShowPage(index));
        }
    }

    public void ShowPage(int index)
    {
        MusicPlayerBehavior.Instance.PlayClickSound();
        for (int i = 0; i < _pages.Count; i++)
        {
            if (i == index)
                _pages[i].SetActive(true);
            else
                _pages[i].SetActive(false);
        }
    }
}
