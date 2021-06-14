using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class DiamondDynamicData
    {
        [SerializeField] private DiamondName _name = default;
        [SerializeField] private AdvancedBoolean _isDiscovered = new AdvancedBoolean();
        [SerializeField] private AdvancedBoolean _isOwned = new AdvancedBoolean();
        [SerializeField] private OnePartAdvancedNumber _level = new OnePartAdvancedNumber();

        public DiamondName Name => _name;
        public AdvancedBoolean IsDiscovered => _isDiscovered;
        public AdvancedBoolean IsOwned => _isOwned;
        public OnePartAdvancedNumber Level => _level;

        private DiamondDynamicData() { }

        public DiamondDynamicData(DiamondName name, bool isDiscovered,
            bool isOwned, int level)
        {
            _name = name;
            _isDiscovered.FeedData(isDiscovered);
            _isOwned.FeedData(isOwned);
            _level.FeedData(level);
        }
    }
}
