using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
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
        //Debug.Log($"GetInstance 1 {typeof(T).Name}");
        if (instance == null || instance == default)
        {
            //Debug.Log($"Finding instance of type {typeof(T).Name}.");
            instance = UnityEngine.Object.FindObjectOfType<T>(true);
        }
        return instance;
    }
}
