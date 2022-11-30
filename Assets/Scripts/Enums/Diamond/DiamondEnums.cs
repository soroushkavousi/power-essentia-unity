namespace Assets.Scripts.Enums
{
    public enum DiamondName
    {
        NONE,
        //Base
        ARCHER,
        BOW,
        WALL,
        KING,
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
        BASE = 1,
        LEFT = 2,
        DECK = 2,
        RIGHT = 3,
    }

    public enum DiamondState
    {
        READY,
        USING,
        COOLDOWN,
        DEACTIVED
    }
}
