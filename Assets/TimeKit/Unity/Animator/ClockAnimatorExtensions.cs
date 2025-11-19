using TimeKit.Unity.Animator;
using TimeKit.Unity.Component;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        public static IClock SetLink(this IClock clock, Animator animator)
        {
            Register(clock, animator);
            return clock;
        }

        public static Animator WithClock(this Animator animator, IClock clock)
        {
            Register(clock, animator);
            return animator;
        }

        private static void Register(IReadOnlyClock clock, Animator animator)
        {
            AnimatorClockLinkGroup.Register(clock.Type, animator);
            
            var component = animator.GetComponent<ClockLinkComponent>();
            if (!component)
                component = animator.gameObject.AddComponent<ClockLinkComponent>();
            component.LinkAnimator(clock.Type, animator);
        }
    }
}