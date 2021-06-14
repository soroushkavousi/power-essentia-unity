using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetCollection",
    menuName = "StaticData/EnumConverters/PlayerSetCollection", order = 1)]
public class PlayerSetCollection : ScriptableObject
{
    [SerializeField] private List<PlayerSet> _playerSets = default;
}
