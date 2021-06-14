using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    public enum NumberChangeCommandPart
    {
        FEED_DATA,
        ONE_PART_VALUE,
        THREE_PART_BASE_VALUE,
        THREE_PART_VALUE,
        THREE_PART_CURRENT_VALUE,
    }

    public enum HealthChangeType
    {
        LEVEL,
        PHYSICAL_DAMAGE,
        MAGICAL_DAMAGE,
        PURE_DAMAGE,
        HEAL
    }

    public enum MovementChangeType
    {
        BURN,
        MOVING_SMOUTHLY,
    }

    public enum ObjectResistanceChangeType
    {
        PHYSICAL_RESISTANCE,
        MAGICAL_RESISTANCE,
        STATUS_RESISTANCE
    }

    public enum ResistanceChangeType
    {
        LEVEL,
    }
}
