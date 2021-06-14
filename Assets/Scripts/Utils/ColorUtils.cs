using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ColorUtils
{
    public static Color GetColorFromHex(string colorHex)
    {
        ColorUtility.TryParseHtmlString(colorHex, out Color color);
        return color;
    }
}
