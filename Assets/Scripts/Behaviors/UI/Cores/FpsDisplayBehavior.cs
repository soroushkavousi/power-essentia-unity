using TMPro;
using UnityEngine;

public class FpsDisplayBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent = default;
    private int _passedTime = 0;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    void Update()
    {
        var deltaTime = Time.unscaledDeltaTime;
        var deltaTimeInMs = Mathf.CeilToInt(deltaTime * Mathf.Pow(10, 3));
        _passedTime += deltaTimeInMs;
        if (_passedTime < 500)
            return;
        _passedTime = 0;
        var fps = Mathf.CeilToInt(1f / deltaTime);
        _textComponent.text = $"{fps} FPS ({deltaTimeInMs}ms)\n" +
            $"[{Screen.currentResolution.refreshRate} ({Screen.currentResolution.width}, {Screen.currentResolution.height})]";
    }
}
