namespace TimeKit.Core.Linked
{
    internal interface IClockSyncLinked : IClockLinked
    {
        void SyncWithClock(IReadOnlyClock clock);
    }
}