using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class StatsDynamicData
{
    [SerializeField] private ThreePartAdvancedNumber _level = new ThreePartAdvancedNumber(currentDummyMin: 0f);

    public ThreePartAdvancedNumber Level => _level;

    private StatsDynamicData() { }

    public StatsDynamicData(int level)
    {
        _level.FeedData(level);
    }

    public StatsDynamicData Copy() => 
        new StatsDynamicData(_level.Current.IntValue);
}
