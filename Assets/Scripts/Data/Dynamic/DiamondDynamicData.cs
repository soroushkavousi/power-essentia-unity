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
        [SerializeField] private Observable<int> _level;

        public DiamondName Name => _name;
        public Observable<DiamondKnowledgeState> KnowledgeState => _knowledgeState;
        public Observable<int> Level => _level;

        private DiamondDynamicData() { }

        public DiamondDynamicData(DiamondName name, DiamondKnowledgeState knowledgeState,
            int level)
        {
            _name = name;
            _knowledgeState = new(knowledgeState);
            _level = new(level);
        }
    }
}
