using System.Linq;
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
            if (!animator)
                return;

            AnimatorClockLink link = null;
            
            var links = animator.GetComponents<AnimatorClockLink>();
            if (links != null)
            {
                foreach (var l in links)
                {
                    if (!l || l.Target != animator) continue;
                    link = l;
                    break;
                }
            }

            if (!link)
                link = animator.gameObject.AddComponent<AnimatorClockLink>();
            
            link.Bind(clockType, animator);
        }
    }
}