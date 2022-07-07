using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class VisionAreaBehavior : MonoBehaviour
{
    [SerializeField] private VisionBehavior _visionBehavior = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private BoxCollider2D _boxCollider2D = default;

    public VisionBehavior VisionBehavior => _visionBehavior;

    private void Start()
    {
        //FixIllegalXSize();
    }

    public void Initialize(Vector2 centerPoint, Vector2 size)
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _boxCollider2D.offset = centerPoint;
        _boxCollider2D.size = size;
    }

    //private void FixIllegalXSize()
    //{
    //    var areaMaxXPosition = transform.position.x + _boxCollider2D.offset.x + _boxCollider2D.size.x / 2;
    //    var illegalXSize = areaMaxXPosition - MissionManagerBehavior.Instance.GameAreaMaxPosition.x;
    //    if (illegalXSize <= 0)
    //        return;
    //    _boxCollider2D.offset -= new Vector2(illegalXSize / 2, 0);
    //    _boxCollider2D.size -= new Vector2(illegalXSize, 0);
    //}

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        _visionBehavior.OnEnter(otherCollider);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        _visionBehavior.OnExit(otherCollider);
    }
}
