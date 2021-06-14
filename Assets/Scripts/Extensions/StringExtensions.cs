using System;
using System.Linq;
using UnityEngine;

public static class StringExtensions
{
    public static T ToEnum<T>(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return default;
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static Enum ToEnum(this string value, Type enumType)
    {
        if (string.IsNullOrWhiteSpace(value))
            return default;
        return (Enum)Enum.Parse(enumType, value, true);
    }

    public static T FromJson<T>(this string value)
    {
        return JsonUtility.FromJson<T>(value);
    }

    public static int ToInt(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return default;
        return int.Parse(value);
    }

    public static string CapitalizeFirstCharacter(this string input) =>
        input switch
        {
            null => null,
            "" => "",
            _ => input.First().ToString().ToUpper() + input.Substring(1)
        };
}

