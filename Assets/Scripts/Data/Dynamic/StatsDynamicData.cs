using System;
using UnityEngine;

[Serializable]
public class StatsDynamicData
{
    [SerializeField] private Observable<int> _level = new();

    public Observable<int> Level => _level;

    private StatsDynamicData() { }

    public StatsDynamicData(int level)
    {
        _level.Value = level;
    }

    public StatsDynamicData Copy() =>
        new(_level.Value);
}
