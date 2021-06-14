using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enums
{
    public enum DiamondName
    {
        NONE,
        //Tools
        ARCHER,
        BOW,
        WALL,
        BLOOD,

        //Deck
        FIRE,
        STONE,
        POISON,
        ELECTRICITY,
        ICE,
        HERO,
        WEAPON,
    }

    public enum RingName
    {
        NONE = 0,
        TOOLS = 1,
        LEFT = 2,
        DECK = 2,
        RIGHT = 3,
    }
}
