using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResourceBox
    {
        [SerializeField] private List<ResourceBunch> _resourceBunches = default;
        private Dictionary<ResourceType, OnePartAdvancedNumber> _resourceBunchDictionary = default;

        //public List<ResourceBunch> ResourceBunches => _resourceBunches;
        public Dictionary<ResourceType, OnePartAdvancedNumber> ResourceBunches => _resourceBunchDictionary;

        //public OnePartAdvancedNumber GetResourceAmount(ResourceType type)
        //{
        //    var _resourceBunch = _resourceBunches.Find(rb => rb.Type == type);
        //    if (_resourceBunch != null)
        //        return _resourceBunch.Amount;

        //    return new OnePartAdvancedNumber();
        //}

        private ResourceBox() { }

        public ResourceBox(List<ResourceBunch> resourceBunches)
        {
            _resourceBunches = resourceBunches;
            _resourceBunchDictionary = new Dictionary<ResourceType, OnePartAdvancedNumber>();
            _resourceBunches.ForEach(rb => _resourceBunchDictionary.Add(rb.Type, rb.Amount));
        }

        public ResourceBox Copy() => new ResourceBox(_resourceBunches);
    }
}
