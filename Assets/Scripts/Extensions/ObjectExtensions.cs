using UnityEngine;


public static class ObjectExtensions
{
    public static DestinationType To<DestinationType>(this object obj)
    {
        if (obj == null)
            return default;
        return (DestinationType)obj;
    }

    public static string ToJson(this object obj, bool pretty = true)
    {
        return JsonUtility.ToJson(obj, pretty);
    }
}

