using TimeKit.Unity.Link.Core;
using TimeKit.Unity.Link.Particle;
using UnityEngine;

namespace TimeKit
{
    public static class ClockParticleSystemExtensions
    {
        /// <summary>
        /// Links the particle system to the clock.
        /// </summary>
        public static Clock SetLink(this Clock clock, ParticleSystem particleSystem)
        {
            ClockLinkBinder.Bind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return clock;
        }

        /// <summary>
        /// Unlinks the particle system from the clock.
        /// </summary>
        public static Clock Unlink(this Clock clock, ParticleSystem particleSystem)
        {
            ClockLinkBinder.TryUnbind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return clock;
        }

        /// <summary>
        /// Binds the particle system to the specified clock.
        /// </summary>
        public static ParticleSystem WithClock(this ParticleSystem particleSystem, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return particleSystem;
        }

        /// <summary>
        /// Unbinds the particle system from the specified clock.
        /// </summary>
        public static ParticleSystem UnlinkClock(this ParticleSystem particleSystem, IReadOnlyClock clock)
        {
            ClockLinkBinder.TryUnbind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return particleSystem;
        }
    }
}