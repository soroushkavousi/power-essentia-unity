using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class StunStatusInstance
{
    [SerializeField] private GameObject _refGameObject = default;
    [SerializeField] private ThreePartAdvancedNumber _damage = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _duration = new ThreePartAdvancedNumber(currentDummyMin: 0f);

    public GameObject RefGameObject => _refGameObject;
    public ThreePartAdvancedNumber Damage => _damage;
    public ThreePartAdvancedNumber Duration => _duration;

    public StunStatusInstance(GameObject refGameObject, float damage, 
        float duration)
    {
        _refGameObject = refGameObject;
        _damage.FeedData(damage);
        _duration.FeedData(duration);
    }
}
