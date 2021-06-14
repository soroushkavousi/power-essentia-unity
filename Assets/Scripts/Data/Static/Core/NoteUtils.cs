using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class NoteUtils
{
    public static string MakeBold(string content) => $"<b>{content}</b>";
    public static string MakeBold(int content) => MakeBold($"{content}");
    public static string MakeBold(float content) => MakeBold($"{content:N2}");

    public static string AddColor(string content, string color)
    {
        if (!color.StartsWith("#"))
            color = $"\"{color}\"";
        return $"<color={color}>{content}</color>";
    }
    public static string AddColor(int content, string color) => AddColor($"{content}", color);
    public static string AddColor(float content, string color) => AddColor($"{content:N2}", color);
}
