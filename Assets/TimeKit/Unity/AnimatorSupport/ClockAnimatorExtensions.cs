using TimeKit.Unity.AnimatorSupport;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        public static IClock SetLink(this IClock clock, Animator animator)
        {
            Register(clock.Type, animator);
            return clock;
        }

        public static Animator WithClock(this Animator animator, IClock clock)
        {
            Register(clock.Type, animator);
            return animator;
        }

        private static void Register(ClockType clockType, Animator animator)
        {
            var link = animator.GetComponent<AnimatorClockLink>();
            if (!link)
                link = animator.gameObject.AddComponent<AnimatorClockLink>();
            
            link.Register(clockType, animator);
        }
    }
}