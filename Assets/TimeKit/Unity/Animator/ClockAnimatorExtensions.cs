using TimeKit.Unity.Animator;
using TimeKit.Unity.Component;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        public static IReadOnlyClock SetLink(this IReadOnlyClock clock, UnityEngine.Animator animator)
        {
            AnimatorClockLinkGroup.Register(clock.Type, animator);
            SetComponent(clock, animator);
            return clock;
        }

        public static UnityEngine.Animator WithClock(this UnityEngine.Animator animator, IReadOnlyClock clock)
        {
            AnimatorClockLinkGroup.Register(clock.Type, animator);
            SetComponent(clock, animator);
            return animator;
        }

        private static void SetComponent(IReadOnlyClock clock, UnityEngine.Animator animator)
        {
            var linkComponent = animator.GetComponent<ClockLinkComponent>();
            if (!linkComponent)
                linkComponent = animator.gameObject.AddComponent<ClockLinkComponent>();
            linkComponent.LinkAnimator(clock.Type, animator);
        }
    }
}