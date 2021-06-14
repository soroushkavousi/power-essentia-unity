using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    public enum DemonName
    {
        LIZARD,
        BLACK_MAGE,
        FOX,
    }

    public enum DemonType
    {
        NONE,
        MELEE,
        RANGE,
    }

    public enum LizardState
    {
        NOT_DEFINED,
        IDLING,
        MOVING,
        ATTACKING,
        PRE_STUNNING,
        STUNNING,
        POST_STUNNING,
    }

    public enum BlackMageState
    {
        NOT_DEFINED,
        IDLING,
        MOVING,
        SHOOTING,
        PRE_STUNNING,
        STUNNING,
        POST_STUNNING,
        HEALING
    }

    public enum FoxState
    {
        RUNNING,
        ATTACKING,
        JUMPING
    }
}
