#if UNITY_EDITOR
using System.Collections.Generic;

namespace TimeKit.Editor.Debugging.Data
{
    internal sealed class ClockDebugInfo
    {
        private Clock _clock;
        
        // getter
        internal ClockType Type => _clock.Type;
        internal bool IsStopped => _clock.IsStopped;
        internal bool IsPaused => _clock.IsPaused;
        internal double Time => _clock.Time;
        internal float DeltaTime => _clock.DeltaTime;
        internal float TimeScale => _clock.TimeScale;
        internal int LinkedCount => _clock.Linked.Group.Count;

        internal List<ClockLinkDebugInfo> LinkInfos
        {
            get
            {
                var list = new List<ClockLinkDebugInfo>();
                foreach (var link in _clock.Linked.Group)
                {
                    var info = new ClockLinkDebugInfo(link);
                    list.Add(info);
                }

                return list;
            }
        }
        
        
        internal ClockDebugInfo(ClockType type)
        {
            _clock = TimeManager.GetRealClock(type);
        }
    }
}
#endif