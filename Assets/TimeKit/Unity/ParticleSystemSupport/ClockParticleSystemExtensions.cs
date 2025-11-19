using TimeKit.Unity.ParticleSystemSupport;
using UnityEngine;

namespace TimeKit
{
    public static class ClockParticleSystemExtensions
    {
        public static IClock SetLink(this IClock clock, ParticleSystem particleSystem)
        {
            Bind(clock.Type, particleSystem);
            return clock;
        }

        public static ParticleSystem WithClock(this ParticleSystem particleSystem, IReadOnlyClock clock)
        {
            Bind(clock.Type, particleSystem);
            return particleSystem;
        }

        private static void Bind(ClockType clockType, ParticleSystem particleSystem)
        {
            if (!particleSystem)
                return;

            ParticleSystemClockLink link = null;

            var links = particleSystem.GetComponents<ParticleSystemClockLink>();
            if (links != null)
            {
                foreach (var l in links)
                {
                    if (!l || l.Target != particleSystem) 
                        continue;
                    link = l;
                    break;
                }
            }

            if (!link)
                link = particleSystem.gameObject.AddComponent<ParticleSystemClockLink>();
            
            link.Bind(clockType, particleSystem);
        }
    }
}