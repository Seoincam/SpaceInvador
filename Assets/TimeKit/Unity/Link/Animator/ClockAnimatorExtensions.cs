using TimeKit.Unity.Link.Animation;
using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        // Link
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
        
        // Unlink
        public static IClock Unlink(this IClock clock, Animator animator)
        {
            Unbind(clock.Type, animator);
            return clock;
        }

        public static Animator UnlinkClock(this Animator animator, IClock clock)
        {
            Unbind(clock.Type, animator);
            return animator;
        }

        private static bool Unbind(ClockType clockType, Animator animator)
        {
            if (!animator)
                return false;

            
            
            return true;
        }
    }
}