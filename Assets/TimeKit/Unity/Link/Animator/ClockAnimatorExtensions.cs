using TimeKit.Unity.Link.Animation;
using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        public static IClock SetLink(this IClock clock, Animator animator)
        {
            ClockLinkBinder.Bind<Animator, AnimatorClockLink>(clock.Type, animator);
            return clock;
        }
        
        public static IClock Unlink(this IClock clock, Animator animator)
        {
            ClockLinkBinder.Unbind<Animator, AnimatorClockLink>(clock.Type, animator);
            return clock;
        }

        public static Animator WithClock(this Animator animator, IClock clock)
        {
            ClockLinkBinder.Bind<Animator, AnimatorClockLink>(clock.Type, animator);
            return animator;
        }
        
        public static Animator UnlinkClock(this Animator animator, IClock clock)
        {
            ClockLinkBinder.Unbind<Animator, AnimatorClockLink>(clock.Type, animator);
            return animator;
        }
    }
}