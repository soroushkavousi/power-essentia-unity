using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class RangeAttackerStaticData
{
    [SerializeField] private AudioClip _shootSound = default;
    [SerializeField] private AttackerStaticData _attackerStaticData = default;
    [SerializeField] private ProjectileBehavior _projectileBehavior = default;
    [SerializeField] private ProjectileStaticData _projectileStaticData = default;

    public AudioClip ShootSound => _shootSound;
    public AttackerStaticData AttackerData => _attackerStaticData;
    public ProjectileBehavior ProjectileBehavior => _projectileBehavior;
    public ProjectileStaticData ProjectileStaticData => _projectileStaticData;
}
