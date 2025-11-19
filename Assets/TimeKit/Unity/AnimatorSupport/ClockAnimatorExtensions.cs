using TimeKit.Unity;
using TimeKit.Unity.AnimatorSupport;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        public static IClock SetLink(this IClock clock, Animator animator)
        {
            Bind(clock.Type, animator);
            return clock;
        }

        public static Animator WithClock(this Animator animator, IClock clock)
        {
            Bind(clock.Type, animator);
            return animator;
        }

        private static void Bind(ClockType clockType, Animator animator)
        {
            if (!animator)
                return;

            var link = ClockLinkComponentUtils.GetOrAddLinkComponent<AnimatorClockLink, Animator>(animator);
            link.Bind(clockType, animator);
        }
    }
}