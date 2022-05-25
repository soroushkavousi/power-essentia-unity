using Assets.Scripts.Enums;
using System;
using UnityEngine;

public static class TypeExtensions
{
    public static bool IsSameOrSubclass(this Type potentialBase, Type potentialDescendant)
    {
        return potentialDescendant.IsSubclassOf(potentialBase)
               || potentialDescendant == potentialBase;
    }
}

