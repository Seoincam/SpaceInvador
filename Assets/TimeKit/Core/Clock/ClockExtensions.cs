namespace TimeKit
{
    public static class ClockExtensions
    {
        public static Cooldown Cooldown(this IReadOnlyClock clock, float duration)
        {
            return new Cooldown(clock, duration);
        }
    }
}