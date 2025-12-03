using TimeKit.Unity.Link.Animation;
using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAnimatorExtensions
    {
        /// <summary>
        /// Links an animator to the clock.
        /// </summary>
        public static Clock SetLink(this Clock clock, Animator animator)
        {
            ClockLinkBinder.Bind<Animator, AnimatorClockLink>(clock.Type, animator);
            return clock;
        }
        
        /// <summary>
        /// Unlinks an animator from the clock.
        /// </summary>
        public static Clock Unlink(this Clock clock, Animator animator)
        {
            ClockLinkBinder.TryUnbind<Animator, AnimatorClockLink>(clock.Type, animator);
            return clock;
        }
       
        /// <summary>
        /// Binds the animator to the specified clock.
        /// </summary>
        public static Animator WithClock(this Animator animator, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<Animator, AnimatorClockLink>(clock.Type, animator);
            return animator;
        }
       
        /// <summary>
        /// Unbinds the animator from the specified clock.
        /// </summary>
        public static Animator UnlinkClock(this Animator animator, IReadOnlyClock clock)
        {
            ClockLinkBinder.TryUnbind<Animator, AnimatorClockLink>(clock.Type, animator);
            return animator;
        }
    }
}