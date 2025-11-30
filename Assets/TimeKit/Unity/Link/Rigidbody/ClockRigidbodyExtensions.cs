using TimeKit.Unity.Link.Core;
using TimeKit.Unity.Link.Rb;
using UnityEngine;

namespace TimeKit.Unity.RigidbodySupport
{
    public static class ClockRigidbodyExtensions
    {
        // TODO Rigidbody 넣을지 말지 고민!
        // TODO 넣을거면 Unlink도 구현해야함.
        
        public static IClock SetLink(this IClock clock, Rigidbody rb)
        {
            ClockLinkBinder.Bind<Rigidbody, RigidbodyClockLink>(clock.Type, rb);
            return clock;
        }

        public static Rigidbody WithClock(this Rigidbody rb, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<Rigidbody, RigidbodyClockLink>(clock.Type, rb);
            return rb;
        }

        
        public static IClock SetLink(this IClock clock, Rigidbody2D rb)
        {
            ClockLinkBinder.Bind<Rigidbody2D, Rigidbody2DClockLink>(clock.Type, rb);
            return clock;
        }

        public static Rigidbody2D WithClock(this Rigidbody2D rb, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<Rigidbody2D, Rigidbody2DClockLink>(clock.Type, rb);
            return rb;
        }
    }
}