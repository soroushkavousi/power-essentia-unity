using UnityEngine;

public static class ColorUtils
{
    public static Color GetColorFromHex(string colorHex)
    {
        ColorUtility.TryParseHtmlString(colorHex, out Color color);
        return color;
    }
}
