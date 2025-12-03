using System;

namespace TimeKit.Core.Save
{
    [Serializable]
    public struct ClockSnapshot
    {
        public ClockType type;
        public double time;
        public float timeScale;
        public bool isPaused;
    }
}