using System;

[Serializable]
public class StatsDynamicDataTO : IObserver
{
    public int Level = default;
    private StatsDynamicData _statsDynamicData = default;

    public StatsDynamicData GetStatsDynamicData()
    {
        _statsDynamicData = new StatsDynamicData(Level);
        _statsDynamicData.Level.Attach(this);
        return _statsDynamicData;
    }

    private void OnLevelChanged()
    {
        Level = _statsDynamicData.Level.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _statsDynamicData.Level)
        {
            OnLevelChanged();
        }
    }
}
