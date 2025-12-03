using System;

namespace TimeKit.Core.Save
{
    [Serializable]
    public struct CooldownSnapshot
    {
        public bool isActive;
        public float remaining;
        public float duration;
    }
}