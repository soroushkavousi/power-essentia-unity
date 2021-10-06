using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _owner = default;
    [SerializeField] private TextMeshProUGUI _text = default;
    [SerializeField] private Graphic _targetGraphic = default;
    [SerializeField] private Color _pressedColor = default;
    [SerializeField] private Color _lockColor = default;
    [SerializeField] private AudioClip _clickSound = default;
    [SerializeField] private UnityEvent _clickEvent;
    private Color _normalColor = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private bool _isColliderDisabled = default;
    [SerializeField] private Collider2D _collider2D = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public GameObject Owner => _owner;
    public TextMeshProUGUI Text => _text;
    public Graphic TargetGraphic => _targetGraphic;
    public bool IsColliderDisabled { get => _isColliderDisabled; set => _isColliderDisabled = value; }
    public OrderedList<Action> OnClickDownActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnClickUpActions { get; } = new OrderedList<Action>();

    private void Awake()
    {
        if (_normalColor == default)
            _normalColor = _targetGraphic.color;

        _collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckMultiTouch();
    }

    private void CheckMultiTouch()
    {
        if (Input.touchCount <= 1)
            return;
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (_collider2D.bounds.Contains(touchPosition))
                    Debug.Log($"!!!!!!!!!!! Method 1");

                touchPosition = Camera.main.ScreenToWorldPoint(touch.rawPosition);
                if (_collider2D.bounds.Contains(touchPosition))
                    Debug.Log($"!!!!!!!!!!! Method 2");

                if (_collider2D.bounds.Contains(touch.rawPosition))
                    Debug.Log($"!!!!!!!!!!! Method 3");

                if (_collider2D.bounds.Contains(touch.position))
                {
                    Debug.Log($"!!!!!!!!!!! Method 4");
                    OnMouseDown();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (_isColliderDisabled)
            return;

        Debug.Log($"Button [{name} -> {transform.parent.name} -> {transform.parent.parent.name}] clicked.");
        SetPressedColor();
        PlayClickSound();
        _clickEvent.Invoke();
        OnClickDownActions.CallActionsSafely();
    }

    private void OnMouseUp()
    {
        if (_isColliderDisabled)
            return;

        OnClickUpActions.CallActionsSafely();
    }

    private void SetPressedColor()
    {
        if (_pressedColor != default)
            _targetGraphic.color = _pressedColor;
    }

    private void PlayClickSound()
    {
        if (_clickSound != default)
            MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_clickSound, 1);
        else
            MusicPlayerBehavior.Instance.PlayClickSound();
    }

    public void Lock()
    {
        if (_normalColor == default)
            _normalColor = _targetGraphic.color;
        IsColliderDisabled = true;
        _targetGraphic.color = _lockColor;
    }

    public void Unlock()
    {
        if (_normalColor == default)
            _normalColor = _targetGraphic.color;
        IsColliderDisabled = false;
        _targetGraphic.color = _normalColor;
    }
}
