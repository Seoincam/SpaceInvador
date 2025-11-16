namespace TimeKit.Core
{
    public interface IClockAware
    {
        IClock Clock { get; set; }
    }
}