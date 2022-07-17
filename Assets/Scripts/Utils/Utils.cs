using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    private static readonly System.Random _random = new();

    public static bool AreTwoListEqual<T>(List<T> l1, List<T> l2)
    {
        if (l1 == null && l2 == null)
            return true;

        if (l1 == null || l2 == null)
            return false;

        var firstNotSecond = l1.Except(l2).ToList();
        var secondNotFirst = l2.Except(l1).ToList();

        return !firstNotSecond.Any() && !secondNotFirst.Any();
    }

    public static T GetInstance<T>(ref T instance) where T : MonoBehaviour
    {
        if (instance == null || instance == default)
            instance = UnityEngine.Object.FindObjectOfType<T>(true);
        return instance;
    }

    public static string GenerateRandomString(int length)
    {
        var chars = "abcdefghijklmnopqrstuvwxyz0123456789".Shuffle();
        var randomString = "";
        for (int i = 0; i < length; i++)
            randomString += chars[_random.Next(chars.Length)];
        return randomString;
    }

    public static string Shuffle(this string str)
    {
        char[] array = str.ToCharArray();
        var rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
        return new string(array);
    }
}
