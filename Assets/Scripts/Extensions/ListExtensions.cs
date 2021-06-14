using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ListExtensions
{
    public static GameObject FindCLosestGameObject(this List<GameObject> targets, GameObject source)
    {
        float dist;
        GameObject currentGameObject;
        var minDist = Mathf.Infinity;
        var nearestGameObject = targets[0];
        for (int i = 0; i < targets.Count; i++)
        {
            currentGameObject = targets[i];
            dist = Vector3.Distance(source.transform.position, currentGameObject.transform.position);
            if (dist < minDist)
            {
                nearestGameObject = currentGameObject;
                minDist = dist;
            }
        }
        return nearestGameObject;
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

