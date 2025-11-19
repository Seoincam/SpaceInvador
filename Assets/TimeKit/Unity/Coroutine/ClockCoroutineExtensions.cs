using System;
using System.Collections;

namespace TimeKit.Unity.Coroutine
{
    public static class ClockCoroutineExtensions
    {
        public static IEnumerator Wait(this IReadOnlyClock clock, float seconds)
        {
            if (seconds < 0f)
                throw new ArgumentOutOfRangeException(nameof(seconds));

            double end = clock.Time + seconds;
            while (clock.Time < end)
                yield return null;
        }

        public static IEnumerator Wait(this IReadOnlyClock clock, TimeSpan timeSpan)
        {
            var seconds = (float)timeSpan.TotalSeconds;
            return clock.Wait(seconds);
        }

        public static IEnumerator WaitUntil(this IReadOnlyClock clock, Func<bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            while (!predicate())
                yield return null;
        }

        public static IEnumerator WaitWhile(this IReadOnlyClock clock, Func<bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            while (predicate())
                yield return null;
        }

        public static IEnumerator WaitUntilReady(this Cooldown cooldown)
        {
            while (!cooldown.IsReady)
                yield return null;
        }
    }
}