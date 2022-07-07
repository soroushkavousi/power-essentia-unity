using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class WaveDescription
    {
        [SerializeField] private int _columnCount = default;
        [SerializeField] private List<DemonBunch> _demonBunches = default;

        public int ColumnCount => _columnCount;
        public List<DemonBunch> DemonBunches => _demonBunches;
        public int DemonCount => _demonBunches.Sum(ib => ib.Count);
        public int WaveNumber { get; set; }
    }

    [Serializable]
    public class DemonBunch
    {
        [SerializeField] private DemonName _demonName = default;
        [SerializeField] private int _count = default;

        public DemonName DemonName => _demonName;
        public int Count => _count;
    }
}
