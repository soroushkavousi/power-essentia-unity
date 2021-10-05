using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(RotationBehavior))]
public class TestBehavior : MonoBehaviour
{
    private MovementBehavior _movementBehavior = default;
    private RotationBehavior _rotationBehavior = default;
    private Vector2 _mousePosition = default;

    private void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _rotationBehavior = GetComponent<RotationBehavior>();

        _movementBehavior.FeedData(new MovementStaticData(500, Vector2.right, 0, 1, false));
        _movementBehavior.Direction = Vector2.right;
        _rotationBehavior.FeedData();
    }

    public void DoTest()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _rotationBehavior.RotateToTarget(_mousePosition);
        _movementBehavior.StartMoving();
    }

    public void WriteASample()
    {
        Debug.Log($"A Sample!");
    }
}
