using TimeKit.Unity.Link.Core;
using TimeKit.Unity.Link.Particle;
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

            var link = ClockLinkComponentUtils.GetOrAddLinkComponent<ParticleSystemClockLink, ParticleSystem>(particleSystem);
            link.Bind(clockType, particleSystem);
        }
    }
}