namespace TimeKit
{
    public interface IClockAware
    {
        IClock Clock { get; set; }
    }
}