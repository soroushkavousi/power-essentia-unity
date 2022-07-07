using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class AttackAreaBehavior : MonoBehaviour
{
    private static AttackAreaBehavior _instance = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _mouseIsDown = default;
    private PlayerBehavior _mainPlayerBehavior = default;

    public static AttackAreaBehavior Instance => Utils.GetInstance(ref _instance);
    public bool MouseIsDown => _mouseIsDown;

    private void Start()
    {
        _mainPlayerBehavior = PlayerBehavior.Main;
    }

    private void OnMouseDown()
    {
        _mainPlayerBehavior.MouseIsDown = _mouseIsDown = true;
    }

    private void OnMouseUp()
    {
        _mainPlayerBehavior.MouseIsDown = _mouseIsDown = false;
    }
}
