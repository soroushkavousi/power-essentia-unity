using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class NumberExtensions
{
    public static int RemoveNegative(this int number)
    {
        number = number >= 0 ? number : 0;
        return number;
    }

    public static float RemoveNegative(this float number)
    {
        number = number >= 0f ? number : 0f;
        return number;
    }

    public static float Round(this float number, int decimalCount = 4)
    {
        var decimalWeight = Mathf.Pow(10, decimalCount);
        number = Mathf.Round(number * decimalWeight) / decimalWeight;
        return number;
    }
}

