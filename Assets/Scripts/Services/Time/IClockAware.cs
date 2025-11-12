namespace Services.Time
{
    public interface IClockAware
    {
        IClock Clock { get; set; }
    }
}