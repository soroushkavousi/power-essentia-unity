public static class NoteUtils
{
    public const string UpgradeColor = "#542402";
    public const int NextNumberSizeRatio = 80;
    public const int NumberSizeRatio = 95;

    public static string ChangeSize(string content, int size) => $"<size={size}%>{content}</size>";
    public static string MakeBold(string content) => $"<b>{content}</b>";
    public static string MakeBold(int content) => MakeBold($"{content}");
    public static string MakeBold(long content) => MakeBold($"{content}");
    public static string MakeBold(float content) => MakeBold($"{content:N2}");

    public static string AddColor(string content, string color)
    {
        if (!color.StartsWith("#"))
            color = $"\"{color}\"";
        return $"<color={color}>{content}</color>";
    }
    public static string AddColor(int content, string color) => AddColor($"{content}", color);
    public static string AddColor(long content, string color) => AddColor($"{content}", color);
    public static string AddColor(float content, string color) => AddColor($"{content:N2}", color);

    public static string ConvertCriticalDamageToXFormat(long criticalDamagePercentage)
    {
        var number = criticalDamagePercentage / 100f + 1;
        number = number.Round();
        return $"{number}X";
    }
}
