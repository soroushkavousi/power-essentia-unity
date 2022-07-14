using Assets.Scripts.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class DiamondDynamicData
    {
        [SerializeField] private DiamondName _name = default;
        [SerializeField] private Observable<DiamondKnowledgeState> _knowledgeState;
        [SerializeField] private Level _level;

        public DiamondName Name => _name;
        public Observable<DiamondKnowledgeState> KnowledgeState => _knowledgeState;
        public Level Level => _level;

        private DiamondDynamicData() { }

        public DiamondDynamicData(DiamondName name, DiamondKnowledgeState knowledgeState,
            int level)
        {
            _name = name;
            _knowledgeState = new(knowledgeState);
            _level = new(level, GameManagerBehavior.Instance.Settings.DiamondMaxLevel);
        }
    }
}
