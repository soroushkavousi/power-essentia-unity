using System;

[Serializable]
public class LevelInfo
{
    public float StartValue = default;
    public float OneLevelPercentage = 0f;

    public LevelInfo(float startValue, float oneLevelPercentage)
    {
        StartValue = startValue;
        OneLevelPercentage = oneLevelPercentage;
    }
}