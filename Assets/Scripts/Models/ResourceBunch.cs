using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResourceBunch
    {
        [SerializeField] private ResourceType _type = default;
        [SerializeField] private OnePartAdvancedNumber _amount = new OnePartAdvancedNumber(dummyMin: 0f);

        public ResourceType Type { get => _type; set => _type = value; }
        public OnePartAdvancedNumber Amount => _amount;

        private ResourceBunch() { }

        public ResourceBunch(ResourceType type, long valueStartValue)
        {
            Type = type;
            _amount.FeedData(valueStartValue);
        }

        public ResourceBunch Copy() => new ResourceBunch(Type, Amount.IntValue);
    }
}
