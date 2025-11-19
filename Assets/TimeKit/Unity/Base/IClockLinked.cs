namespace TimeKit.Unity
{
    internal interface IClockLinked
    {
        void SyncWithClock(IReadOnlyClock clock);
    }
}