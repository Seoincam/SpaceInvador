using System.Collections.Generic;

namespace TimeKit.Core.Linked
{
    internal class ClockLinkedGroup
    {
        internal readonly List<IClockLinked> Group = new();
        internal readonly List<IClockSyncLinked> SyncGroup = new();
        internal readonly List<IClockTickLinked> TickGroup = new();
        
        internal ClockLinkedGroup(Clock.Clock clock)
        {
            clock.StateChanged += Sync;
        }
        
        internal void Register(IClockSyncLinked linked)
        {
            Group.Add(linked);
            SyncGroup.Add(linked);
        }
        internal void Register(IClockTickLinked linked)
        {
            Group.Add(linked);
            TickGroup.Add(linked);
        }

        internal void Unregister(IClockSyncLinked linked)
        {
            Group.Remove(linked);
            SyncGroup.Remove(linked);
        }
        internal void Unregister(IClockTickLinked linked)
        {
            Group.Remove(linked);
            TickGroup.Remove(linked);
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