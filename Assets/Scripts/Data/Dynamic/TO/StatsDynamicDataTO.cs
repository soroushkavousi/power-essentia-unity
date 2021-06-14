using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class StatsDynamicDataTO
{
    public int Level = default;
    private StatsDynamicData _statsDynamicData = default;

    public StatsDynamicData GetStatsDynamicData()
    {
        _statsDynamicData = new StatsDynamicData(Level);
        _statsDynamicData.Level.Current.OnNewValueActions.Add(OnLevelChanged);
        return _statsDynamicData;
    }

    private void OnLevelChanged(NumberChangeCommand changeCommand)
    {
        Level = _statsDynamicData.Level.Current.IntValue;
        PlayerDynamicDataTO.Instance.Save();
    }
}
