using TimeKit.Unity.Link.Core;
using TimeKit.Unity.Link.Particle;
using UnityEngine;

namespace TimeKit
{
    public static class ClockParticleSystemExtensions
    {
        public static IClock SetLink(this IClock clock, ParticleSystem particleSystem)
        {
            ClockLinkBinder.Bind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return clock;
        }

        public static IClock Unlink(this IClock clock, ParticleSystem particleSystem)
        {
            ClockLinkBinder.Unbind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return clock;
        }

        public static ParticleSystem WithClock(this ParticleSystem particleSystem, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return particleSystem;
        }
        
        public static ParticleSystem UnlinkClock(this ParticleSystem particleSystem, IReadOnlyClock clock)
        {
            ClockLinkBinder.Unbind<ParticleSystem, ParticleSystemClockLink>(clock.Type, particleSystem);
            return particleSystem;
        }
    }
}