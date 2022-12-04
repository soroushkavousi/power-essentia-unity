using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _owner = default;
    [SerializeField] private TextMeshProUGUI _text = default;
    [SerializeField] private Graphic _targetGraphic = default;
    [SerializeField] private Color _pressedColor = Color.clear;
    [SerializeField] private Color _lockColor = Color.clear;
    [SerializeField] private Color _lockTextColor = Color.clear;
    [SerializeField] private AudioClip _clickSound = default;
    [SerializeField] private UnityEvent _clickEvent;
    private Color _normalColor = Color.clear;
    private Color _normalTextColor = Color.clear;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _isColliderDisabled = default;
    [SerializeField] private Collider2D _collider2D = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public GameObject Owner => _owner;
    public TextMeshProUGUI Text => _text;
    public Graphic TargetGraphic => _targetGraphic;
    public bool IsColliderDisabled { get => _isColliderDisabled; set => _isColliderDisabled = value; }
    public OrderedList<Action> OnClickDownActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnClickUpActions { get; } = new OrderedList<Action>();

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();

        if (_normalColor == Color.clear)
            _normalColor = _targetGraphic.color;

        if (_text != default)
            _normalTextColor = _text.color;
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
                touchPosition.z = 0;
                if (_collider2D.bounds.Contains(touchPosition))
                    OnMouseDown();
            }
        }
    }

    private void OnMouseDown()
    {
        if (_isColliderDisabled)
            return;

        //Debug.Log($"Button [{name} -> {transform.parent.name} -> {transform.parent.parent.name}] clicked.");
        StartCoroutine(SetPressedColor());
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

    private IEnumerator SetPressedColor()
    {
        if (_pressedColor == Color.clear)
            yield break;
        var originColor = _targetGraphic.color;
        _targetGraphic.color = _pressedColor;
        yield return new WaitForSeconds(0.1f);
        _targetGraphic.color = originColor;
    }

    private void PlayClickSound()
    {
        if (_clickSound != default)
            MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_clickSound, 0.4f);
        else
            MusicPlayerBehavior.Instance.PlayClickSound();
    }

    public void Lock()
    {
        IsColliderDisabled = true;

        if (_lockColor != Color.clear)
            _targetGraphic.color = _lockColor;
        if (_lockTextColor != Color.clear)
            _text.color = _lockTextColor;
    }

    public void Unlock()
    {
        IsColliderDisabled = false;
        if (_normalColor != Color.clear)
            _targetGraphic.color = _normalColor;
        if (_normalTextColor != Color.clear)
            _text.color = _normalTextColor;
    }
}
