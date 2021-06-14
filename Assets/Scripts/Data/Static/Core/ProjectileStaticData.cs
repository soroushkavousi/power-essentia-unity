using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ProjectileStaticData
{
    [SerializeField] private MovementStaticData _movementStaticData = default;

    public MovementStaticData MovementStaticData => _movementStaticData;
}
