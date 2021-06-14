using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResourceDisplayBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]
    public TextMeshProUGUI AmountText => _amountText;
}
