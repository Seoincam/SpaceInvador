namespace TimeKit.Core.Linked
{
    internal interface IClockLinked
    {
#if UNITY_EDITOR
        string Trace { get; }  
#endif
    }
}