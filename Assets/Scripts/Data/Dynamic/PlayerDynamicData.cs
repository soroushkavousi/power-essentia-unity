﻿using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class PlayerDynamicData
    {
        [SerializeField] private long _patchNumber = default;
        [SerializeField] private Observable<int> _demonLevel = new();
        [SerializeField] private Observable<PlayerSetName> _playerSetName = new();
        [SerializeField] private SelectedItemsDynamicData _selectedItems = default;
        [SerializeField] private List<DiamondDynamicData> _diamonds = default;
        [SerializeField] private List<ResourceBunch> _resourceBunches = default;
        private readonly Dictionary<DiamondName, DiamondDynamicData> _diamondDictionary = default;

        public long PatchNumber => _patchNumber;
        public Observable<int> DemonLevel => _demonLevel;
        public Observable<PlayerSetName> PlayerSetName => _playerSetName;
        public SelectedItemsDynamicData SelectedItems => _selectedItems;
        public Dictionary<DiamondName, DiamondDynamicData> Diamonds => _diamondDictionary;
        public List<ResourceBunch> ResourceBunches => _resourceBunches;

        public PlayerDynamicData(long patchNumber, PlayerSetName playerSetName,
            int demonLevel, SelectedItemsDynamicData selectedItems,
            List<DiamondDynamicData> diamonds, List<ResourceBunch> resourceBunches)
        {
            _patchNumber = patchNumber;
            _playerSetName.Value = playerSetName;
            _demonLevel.Value = demonLevel;
            _selectedItems = selectedItems;
            _diamonds = diamonds;
            _resourceBunches = resourceBunches;

            _diamondDictionary = new Dictionary<DiamondName, DiamondDynamicData>();
            foreach (var diamond in _diamonds)
            {
                _diamondDictionary.Add(diamond.Name, diamond);
            }
        }
    }
}