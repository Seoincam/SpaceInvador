namespace TimeKit
{
    public interface IReadOnlyCooldown
    {
        float Duration { get; }
        bool IsReady { get; }
        float Remaining { get; }
        float RemainingRatio { get; }
    }
}