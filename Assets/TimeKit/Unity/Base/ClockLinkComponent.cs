using UnityEngine;

namespace TimeKit.Unity.Base
{
    public abstract class ClockLinkComponent<T>: MonoBehaviour, IClockLinked where T : Component
    {
        internal abstract T Target { get; }
        
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
            ClockLinkedGroup.Register(this);
        }

        public abstract void SyncWithClock(IReadOnlyClock clock);

        private void OnDestroy()
        {
            ClockLinkedGroup.Unregister(this);
        }
    }
}