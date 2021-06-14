using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBehavior))]
public class ResistanceBehavior : MonoBehaviour
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private ResistanceStaticData _data = default;
    [SerializeField] private ThreePartAdvancedNumber _physicalResistance = new ThreePartAdvancedNumber(currentDummyMin: 0f, currentDummyMax: 99f);
    [SerializeField] private ThreePartAdvancedNumber _magicResistance = new ThreePartAdvancedNumber(currentDummyMin: 0f, currentDummyMax: 99f);
    [SerializeField] private ThreePartAdvancedNumber _statusResistance = new ThreePartAdvancedNumber(currentDummyMin: 0f, currentDummyMax: 99f);
    private HealthBehavior _healthBehavior = default;
    private StatusOwnerBehavior _statusOwnerBehavior = default;

    public ThreePartAdvancedNumber PhysicalResistance => _physicalResistance;
    public ThreePartAdvancedNumber MagicResistance => _magicResistance;
    public ThreePartAdvancedNumber StatusResistance => _statusResistance;

    public void FeedData(ResistanceStaticData data)
    {
        _data = data;

        _healthBehavior = GetComponent<HealthBehavior>();
        _statusOwnerBehavior = GetComponent<StatusOwnerBehavior>();

        _physicalResistance.FeedData(_data.StartPhysicalResistance);
        _magicResistance.FeedData(_data.StartMagicResistance);
        _statusResistance.FeedData(_data.StartStatusResistance);
        _healthBehavior.Health.Current.OnNewChangeCommandActions.Add(ApplyPhysicalResistance);
        _healthBehavior.Health.Current.OnNewChangeCommandActions.Add(ApplyMagicResistance);
        if(_statusResistance.Value != 0f)
            _statusOwnerBehavior.StunStatusBehavior.OnPreApplyInstanceActions.Add(ApplyStatusResistanceForStunStatus);
    }

    private void ApplyPhysicalResistance(NumberChangeCommand numberChangeCommand)
    {
        var healthChangeType = numberChangeCommand.Type.ToEnum<HealthChangeType>();
        if (healthChangeType != HealthChangeType.PHYSICAL_DAMAGE)
            return;

        numberChangeCommand.Amount.Peak.Change(-_physicalResistance.Value, name, ObjectResistanceChangeType.PHYSICAL_RESISTANCE);
    }

    private void ApplyMagicResistance(NumberChangeCommand numberChangeCommand)
    {
        var healthChangeType = numberChangeCommand.Type.ToEnum<HealthChangeType>();
        if (healthChangeType != HealthChangeType.MAGICAL_DAMAGE)
            return;

        numberChangeCommand.Amount.Peak.Change(-_magicResistance.Value, name, ObjectResistanceChangeType.MAGICAL_RESISTANCE);
    }

    private void ApplyStatusResistanceForStunStatus(StunStatusInstance stunStatusInstance)
    {
        stunStatusInstance.Duration.Peak.Change(-_statusResistance.Value, name, ObjectResistanceChangeType.STATUS_RESISTANCE);
    }

}
