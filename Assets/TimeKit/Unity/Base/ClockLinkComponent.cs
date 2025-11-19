using System.Collections.Generic;
using UnityEngine;

namespace TimeKit.Unity
{
    public abstract class ClockLinkComponent<T> : MonoBehaviour, IClockLinked where T: Behaviour
    {
        protected readonly Dictionary<ClockType, List<T>> _linked = new();
        
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
            ClockLinkedGroup.Register(this);
        }

        public abstract void SyncWithClock(IReadOnlyClock clock);

        internal virtual void Register(ClockType type, T toLink)
        {
            _linked[type] ??= new List<T>();
            _linked[type].Add(toLink);
        }

        private void OnDestroy()
        {
            ClockLinkedGroup.Unregister(this);
        }
    }
}