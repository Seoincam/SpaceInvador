using System;

namespace TimeKit.Core.Save
{
    [Serializable]
    public sealed class ClockSaveData
    {
        public ClockSnapshot[] clockSnapshots;
    }
}