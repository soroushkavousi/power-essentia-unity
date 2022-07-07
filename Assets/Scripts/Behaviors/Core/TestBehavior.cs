using UnityEngine;

[RequireComponent(typeof(MovementBehavior))]
public class TestBehavior : MonoBehaviour
{
    private MovementBehavior _movementBehavior = default;
    private RotationUtils RotationUtils = default;
    private Vector2 _mousePosition = default;

    private void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();

        var movementStaticData = new MovementStaticData { Speed = 10f, AnimationSpeed = 10f };
        _movementBehavior.FeedData(movementStaticData);
        RotationUtils = new RotationUtils(this);
    }

    public void DoTest()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotationUtils.RotateToTarget(_mousePosition);
        _movementBehavior.MoveWithDirection(Vector2.right);
    }

    public void WriteASample()
    {
        Debug.Log($"A Sample!");
    }
}
