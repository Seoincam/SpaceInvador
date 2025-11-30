using TimeKit.Unity.Link.Core;
using TimeKit.Unity.Link.Rb;
using UnityEngine;

namespace TimeKit.Unity.RigidbodySupport
{
    public static class ClockRigidbodyExtensions
    {
        public static IClock SetLink(this IClock clock, Rigidbody rb)
        {
            Bind(clock.Type, rb);
            return clock;
        }

        public static Rigidbody WithClock(this Rigidbody rb, IReadOnlyClock clock)
        {
            Bind(clock.Type, rb);
            return rb;
        }

        private static void Bind(ClockType clockType, Rigidbody rb)
        {
            if (!rb) return;

            var link = ClockLinkComponentUtils.GetOrAddLinkComponent<RigidbodyClockLink, Rigidbody>(rb);
            link.Bind(clockType, rb);
        }

        public static IClock SetLink(this IClock clock, Rigidbody2D rb)
        {
            Bind(clock.Type, rb);
            return clock;
        }

        public static Rigidbody2D WithClock(this Rigidbody2D rb, IReadOnlyClock clock)
        {
            Bind(clock.Type, rb);
            return rb;
        }

        private static void Bind(ClockType clockType, Rigidbody2D rb)
        {
            if (!rb) return;

            var link = ClockLinkComponentUtils.GetOrAddLinkComponent<Rigidbody2DClockLink, Rigidbody2D>(rb);
            link.Bind(clockType, rb);
        }
    }
}