using System;
using UnityEngine;

public static class NumberExtensions
{
    public static float Randomize(this float value, float percentage = 20f)
    {
        var min = value.RemovePercentage(percentage);
        var max = value.AddPercentage(percentage);
        return UnityEngine.Random.Range(min, max);
    }

    public static float WithMin(this float value, float min)
    {
        if (value < min)
            return min;
        return value;
    }

    public static float WithMax(this float value, float max)
    {
        if (value < max)
            return max;
        return value;
    }

    public static int ToInt(this float value)
    {
        return Mathf.FloorToInt(value);
    }

    public static long ToLong(this float value)
    {
        var fractionalPart = value % 1;
        if (fractionalPart >= 0.5)
            return Convert.ToInt64(Mathf.Ceil(value));
        else
            return Convert.ToInt64(Mathf.Floor(value));
    }

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

    public static float Round(this float number, int decimalCount = 2)
    {
        var decimalWeight = Mathf.Pow(10, decimalCount);
        number = (number * decimalWeight).ToLong() / decimalWeight;
        return number;
    }

    public static float AddPercentage(this float number, float percentage)
        => MeasurePercentage(number, 100f + percentage);

    public static float RemovePercentage(this float number, float percentage)
        => MeasurePercentage(number, 100f - percentage);

    public static float MeasurePercentage(this float number, float percentage)
    {
        if (percentage == 100f)
            return number;
        number = number * percentage / 100f;
        return number;
    }
}

