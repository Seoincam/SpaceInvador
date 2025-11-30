namespace TimeKit.Core.Linked
{
    internal interface IClockTickLinked : IClockLinked
    {
        void Tick();
    }
}