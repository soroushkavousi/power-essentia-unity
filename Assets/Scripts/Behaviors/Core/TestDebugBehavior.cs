using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebugBehavior : MonoBehaviour
{
    [SerializeField] private ButtonBehavior _buttonBehaviour = default;

    private void Awake()
    {

    }

    public void TestButtonBehavior()
    {
        Debug.Log($"[TestButtonBehavior]");
        var boxCollider = _buttonBehaviour.GetComponent<BoxCollider2D>();
        Debug.Log($"IsColliderDisabled: {_buttonBehaviour.IsColliderDisabled} | {boxCollider.offset} {boxCollider.size}");
        Debug.Log($"boxCollider.bounds.center: {boxCollider.bounds.center}");
        Debug.DrawLine(new Vector3(100, 200, 0), new Vector3(200, 300, 0), Color.red, 2, false);
        _buttonBehaviour.transform.position = _buttonBehaviour.transform.position + new Vector3(0, 10, 0);
    }

    void OnGUI()
    {
        var boxCollider = _buttonBehaviour.GetComponent<BoxCollider2D>();
        Rect rect = new Rect(boxCollider.offset, boxCollider.size);
        //UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, Color.yellow, Color.red);
        var style = new GUIStyle();
        style.fontSize = 40;
        GUI.Box(rect, "collider region", style);
    }
}
