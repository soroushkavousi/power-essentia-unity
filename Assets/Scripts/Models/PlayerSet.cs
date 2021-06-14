using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayerSet
{ 
    [SerializeField] private PlayerSetName _name = default;
    [SerializeField] private ProjectileBehavior _projectileBehavior = default;

    public PlayerSetName Name => _name;
    public ProjectileBehavior ProjectileBehavior => _projectileBehavior;

    private PlayerSet() { }

    public PlayerSet(PlayerSetName name, ProjectileBehavior projectileBehavior)
    {
        _name = name;
        _projectileBehavior = projectileBehavior;
    }

    public PlayerSet Copy() => new PlayerSet(_name, _projectileBehavior);
}
