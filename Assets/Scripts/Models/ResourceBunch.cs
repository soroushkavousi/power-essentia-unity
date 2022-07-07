using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResourceBunch
    {
        [SerializeField] private ResourceType _type = default;
        [SerializeField] private Observable<long> _amount = default;

        public ResourceType Type => _type;
        public Observable<long> Amount => _amount;

        private ResourceBunch() { }

        public ResourceBunch(ResourceType type, long amount)
        {
            _type = type;
            _amount = new(amount);
        }
    }

    [Serializable]
    public class ResourceBunchWithLevel
    {
        [SerializeField] private ResourceType _type = default;
        [SerializeField] private Number _amount = default;

        public ResourceType Type => _type;
        public Number Amount => _amount;

        private ResourceBunchWithLevel() { }

        public ResourceBunchWithLevel(ResourceType type, Number amount)
        {
            _type = type;
            _amount = amount;
        }

        public ResourceBunch ToResourceBunch()
        {
            return new ResourceBunch(_type, _amount.Value.ToLong());
        }
    }
}
