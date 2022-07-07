using TMPro;
using UnityEngine;

public class ResourceDisplayBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]
    public TextMeshProUGUI AmountText => _amountText;
}
