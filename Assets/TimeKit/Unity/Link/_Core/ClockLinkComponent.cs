using System.Diagnostics;
using TimeKit.Core.Linked;
using UnityEngine;

namespace TimeKit.Unity.Link.Core
{
    public abstract class ClockLinkComponent<T>: MonoBehaviour, IClockSyncLinked where T : Component
    {
        internal abstract T Target { get; }
        
        public abstract ClockType Type { get; }
        
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        internal abstract void Bind(ClockType type, T target);
        
        public abstract void SyncWithClock(IReadOnlyClock clock);

        private void OnDestroy()
        {
            TimeManager.GetRealClock(Type).linked.Unregister(this);
        }

        public string Trace
        {
            get
            {
                var trace = new StackTrace(true);
                var frame = trace.GetFrame(1);
                var method = frame.GetMethod();
                var file = frame.GetFileName();
                var line = frame.GetFileLineNumber();
                var filename = frame.GetFileName();
                
                return $"{filename}:{line} {method.DeclaringType?.Name}.{method.Name}";
            }
        }
    }
}