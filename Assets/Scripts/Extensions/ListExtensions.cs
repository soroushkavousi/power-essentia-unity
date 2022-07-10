using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ListExtensions
{
    public static GameObject FindCLosestGameObject(this List<GameObject> targets, GameObject source)
    {
        var distances = targets.Select(t => Vector3.Distance(source.transform.position, t.transform.position)).ToList();
        var minimumDistanceIndex = distances.IndexOfMin();
        return targets[minimumDistanceIndex];
    }

    public static int IndexOfMin(this IList<float> numbers)
    {
        if (numbers == null || numbers.Count == 0)
        {
            throw new ArgumentNullException(nameof(numbers));
        }

        var min = numbers[0];
        var minIndex = 0;

        for (int i = 1; i < numbers.Count; ++i)
        {
            if (numbers[i] < min)
            {
                min = numbers[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public static void CallActionsSafely(this OrderedList<Action> actionList)
    {
        foreach (var action in actionList)
        {
            try
            {
                action();
            }
            catch (MissingReferenceException)
            {
                Debug.Log($"Action removed because of MissingReferenceException.");
                actionList.Remove(action);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                actionList.Remove(action);
            }
        }
    }

    public static void CallActionsSafely<I1>(this OrderedList<Action<I1>> actionList, I1 input1)
    {
        foreach (var action in actionList)
        {
            try
            {
                action(input1);
            }
            catch (MissingReferenceException)
            {
                Debug.Log($"Action removed because of MissingReferenceException.");
                actionList.Remove(action);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                actionList.Remove(action);
            }
        }
    }

    public static void CallActionsSafely<I1, I2>(this OrderedList<Action<I1, I2>> actionList, I1 input1, I2 input2)
    {
        foreach (var action in actionList)
        {
            try
            {
                action(input1, input2);
            }
            catch (MissingReferenceException)
            {
                Debug.Log($"Action removed because of MissingReferenceException.");
                actionList.Remove(action);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                actionList.Remove(action);
            }
        }
    }
}

