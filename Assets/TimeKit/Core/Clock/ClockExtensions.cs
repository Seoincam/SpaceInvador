using System.Runtime.CompilerServices;

namespace TimeKit
{
    public static class ClockExtensions
    {
        public static Cooldown Cooldown(this IClock clock, float duration,
            [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
        {
            return new Cooldown(clock, duration, file, line, member);
        }

        public static CooldownObserver Observer(this IReadOnlyCooldown cooldown)
        {
            return new CooldownObserver(cooldown);
        }
    }
}