using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
public class HealthBehavior : MonoBehaviour
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private MaxHealth _maxHealth;
    [SerializeField] private CurrentHealth _currentHealth;
    [SerializeField] private Death _death;
    
    public MaxHealth MaxHealth => _maxHealth;
    public CurrentHealth CurrentHealth => _currentHealth;
    public Death Death => _death;

    public void FeedData(MaxHealth maxHealth, CurrentHealth currentHealth, Death death)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _death = death;
    }
}
