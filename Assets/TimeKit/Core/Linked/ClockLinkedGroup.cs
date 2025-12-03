using System;
using System.Collections.Generic;

namespace TimeKit.Core.Linked
{
    internal class ClockLinkedGroup
    {
        internal readonly List<IClockLinked> Group = new();
        internal readonly List<IClockSyncLinked> SyncGroup = new();
        internal readonly List<IClockTickLinked> TickGroup = new();

        private ClockType _clockType;

        internal event Action<ClockType> Changed;
        
        internal ClockLinkedGroup(Clock.Clock clock)
        {
            _clockType = clock.Type;
            clock.StateChanged += Sync;
        }
        
        internal void Register(IClockSyncLinked linked)
        {
            Group.Add(linked);
            SyncGroup.Add(linked);
            
            Changed?.Invoke(_clockType);
        }
        internal void Register(IClockTickLinked linked)
        {
            Group.Add(linked);
            TickGroup.Add(linked);
            
            Changed?.Invoke(_clockType);
        }

        internal void Unregister(IClockSyncLinked linked)
        {
            Group.Remove(linked);
            SyncGroup.Remove(linked);
            
            Changed?.Invoke(_clockType);
        }
        internal void Unregister(IClockTickLinked linked)
        {
            Group.Remove(linked);
            TickGroup.Remove(linked);
            
            Changed?.Invoke(_clockType);
        }

        private void Sync(IReadOnlyClock clock)
        {
            foreach (var linked in SyncGroup)
                linked.SyncWithClock(clock);
        }

        internal void Tick()
        {
            foreach (var linked in TickGroup)
                linked.Tick();
        }
    }
}