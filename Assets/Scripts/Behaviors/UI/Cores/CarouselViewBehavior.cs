using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class CarouselViewBehavior : MonoBehaviour
{
    [SerializeField] private Transform _centerPoint = default;
    [SerializeField] private Transform _contentPanel = default;
    [SerializeField] private float _distanceBetweenItems = default;
    [SerializeField] private int _pivotItemIndex = default;
    [SerializeField] private float _lerpSpeed = default;
    

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private List<Transform> _items = default;
    [SerializeField] private int _closestItemIndex = default;
    [SerializeField] private float _targetContentPanelXOffset = default;
    [SerializeField] private float _firstItemLocalX = default;

    private List<float> _itemDistances = default;
    private bool _isDragging = default;
    private List<Transform> _itemsWithoutLast = default;

    public void Initialize()
    {
        FindItems();
        SetSizeOfContentPanel();
        SetPositionOfItems();
        AdjustPivotItem();
        _contentPanel.localPosition = new Vector2(_targetContentPanelXOffset, _contentPanel.localPosition.y);
    }

    private void Update()
    {
        LerpClosestItemToCenter();
    }

    private void FindItems()
    {
        _items = new List<Transform>();
        foreach (Transform item in _contentPanel)
        {
            _items.Add(item);
        }
        _itemsWithoutLast = _items.GetRange(0, _items.Count - 1);
        _pivotItemIndex = Mathf.Clamp(_pivotItemIndex, 0, _items.Count - 2);
    }

    private void SetSizeOfContentPanel()
    {
        var contentPanelWidth = _items.Count * _distanceBetweenItems + 23;
        var contentPanelHeight = _contentPanel.GetComponent<RectTransform>().sizeDelta.y;
        _contentPanel.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(contentPanelWidth, contentPanelHeight);
    }

    private void SetPositionOfItems()
    {
        //_contentPanel.GetComponent<GridLayoutGroup>().spacing = new Vector2(_distanceBetweenItems, 0);
        _firstItemLocalX = -_distanceBetweenItems * (_items.Count - 1) / 2;
        for (int i = 0; i < _items.Count; i++)
        {
            var xPosition = _firstItemLocalX + i * _distanceBetweenItems;
            _items[i].localPosition = new Vector2(xPosition, 0);
        }
    }

    private void AdjustPivotItem()
    {
        var contentPanelLocalX = - (_firstItemLocalX + _distanceBetweenItems / 2) 
            - _pivotItemIndex * _distanceBetweenItems;
        _targetContentPanelXOffset = contentPanelLocalX;
    }

    private void LerpClosestItemToCenter()
    {
        if (_isDragging)
            return;
        if (_contentPanel.localPosition.x == _targetContentPanelXOffset)
            return;
        var newContentPanelLocalX = Mathf.Lerp(_contentPanel.localPosition.x,
            _targetContentPanelXOffset, Time.deltaTime * _lerpSpeed);
        var newContentPanelLocalPosition = new Vector2(newContentPanelLocalX, _contentPanel.localPosition.y);
        _contentPanel.localPosition = newContentPanelLocalPosition;
    }

    public void OnDraggingStarted()
    {
        _isDragging = true;
    }

    public void OnDraggingStopped()
    {
        FindClosestItem();
        _pivotItemIndex = _closestItemIndex;
        AdjustPivotItem();
        _isDragging = false;
    }

    private void FindClosestItem()
    {
        //_itemDistances = _items.Select(item => _centerPoint - item.position.x)
        var minimumDistance = float.MaxValue;
        var currentItemDistance = 0f;
        Transform currentItem;
        for (int i = 0; i < _itemsWithoutLast.Count; i++)
        {
            currentItem = _itemsWithoutLast[i];
            currentItemDistance = Mathf.Abs(currentItem.position.x - _centerPoint.position.x);
            if (currentItemDistance < minimumDistance)
            {
                minimumDistance = currentItemDistance;
                _closestItemIndex = i;
            }
        }
        //foreach (var item in _itemsWithoutLast)
        //{
        //    currentItemDistance = Mathf.Abs(item.position.x - _centerPoint.position.x);
        //    if (currentItemDistance < minimumDistance)
        //    {
        //        minimumDistance = currentItemDistance;
        //        _closestItem = item;
        //    }
        //}
        //Debug.Log($"[Closest Item 1] _closestItem.position.x: {_closestItem.position.x}, _centerPoint.position.x: {_centerPoint.position.x}");
        //currentItemDistance = _closestItem.position.x - _centerPoint.position.x;
        //Debug.Log($"[Closest Item 2] currentItemDistance: {currentItemDistance}, _contentPanel.localPosition.x: {_contentPanel.localPosition.x}");
        //_targetContentPanelXOffset = _contentPanel.localPosition.x - currentItemDistance;
    }
}
