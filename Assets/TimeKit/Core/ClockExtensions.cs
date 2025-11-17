namespace TimeKit.Core
{
    public static class ClockExtensions
    {
        public static Cooldown Cooldown(this IClock clock, float duration)
        {
            return new Cooldown(clock, duration);
        }

        public static CooldownObserver Observer(this IReadOnlyCooldown cooldown)
        {
            return new CooldownObserver(cooldown);
        }
    }
}