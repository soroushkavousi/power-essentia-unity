using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class AttackAreaBehavior : MonoBehaviour
{
    private static AttackAreaBehavior _instance = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _mouseIsDown = default;
    private readonly PlayerBehavior _mainPlayerBehavior = default;

    public static AttackAreaBehavior Instance => Utils.GetInstance(ref _instance);
    public bool MouseIsDown => _mouseIsDown;

    private void OnMouseDown()
    {
        PlayerBehavior.MainPlayer.MouseIsDown = _mouseIsDown = true;
    }

    private void OnMouseUp()
    {
        PlayerBehavior.MainPlayer.MouseIsDown = _mouseIsDown = false;
    }
}
