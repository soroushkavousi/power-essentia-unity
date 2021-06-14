using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class PlayerDynamicData
    {
        [SerializeField] private long _patchNumber = default;
        [SerializeField] private OnePartAdvancedNumber _demonLevel = new OnePartAdvancedNumber();
        [SerializeField] private AdvancedString _playerSetName = new AdvancedString();
        [SerializeField] private SelectedItemsDynamicData _selectedItems = default;
        [SerializeField] private List<DiamondDynamicData> _diamonds = default;
        [SerializeField] private ResourceBox _resourceBox = default;
        private Dictionary<DiamondName, DiamondDynamicData> _diamondDictionary = default;

        public long PatchNumber => _patchNumber;
        public OnePartAdvancedNumber DemonLevel => _demonLevel;
        public AdvancedString PlayerSetName => _playerSetName;
        public SelectedItemsDynamicData SelectedItems => _selectedItems;
        public Dictionary<DiamondName, DiamondDynamicData> Diamonds => _diamondDictionary;
        public ResourceBox ResourceBox => _resourceBox;

        public PlayerDynamicData(long patchNumber, PlayerSetName playerSetName,
            int demonLevel, SelectedItemsDynamicData selectedItems,
            List<DiamondDynamicData> diamonds, ResourceBox resourceBox)
        {
            _patchNumber = patchNumber;
            _playerSetName.FeedData(playerSetName);
            _demonLevel.FeedData(demonLevel);
            _selectedItems = selectedItems;
            _diamonds = diamonds;
            _resourceBox = resourceBox;

            _diamondDictionary = new Dictionary<DiamondName, DiamondDynamicData>();
            foreach (var diamond in _diamonds)
            {
                _diamondDictionary.Add(diamond.Name, diamond);
            }
        }
    }
}