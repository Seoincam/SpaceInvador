namespace TimeKit.Unity.Base
{
    internal interface IClockLinked
    {
        void SyncWithClock(IReadOnlyClock clock);
    }
}