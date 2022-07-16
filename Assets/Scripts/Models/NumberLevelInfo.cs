using System;

[Serializable]
public class NumberLevelInfo
{
    public float StartValue = default;
    public float OneLevelPercentage = 0f;

    public NumberLevelInfo(float startValue, float oneLevelPercentage)
    {
        StartValue = startValue;
        OneLevelPercentage = oneLevelPercentage;
    }
}