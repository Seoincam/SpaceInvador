using TimeKit.Unity.AudioSource;
using TimeKit.Unity.Component;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAudioSourceExtensions
    {
        public static IClock SetLink(this IClock clock, AudioSource audioSource)
        {
            Register(clock, audioSource);
            return clock;
        }

        public static AudioSource WithClock(this AudioSource audioSource, IClock clock)
        {
            Register(clock, audioSource);
            return audioSource;
        }

        private static void Register(IReadOnlyClock clock, AudioSource audioSource)
        {
            AudioSourceClockLinkGroup.Register(clock.Type, audioSource);
            
            var component = audioSource.GetComponent<ClockLinkComponent>();
            if (!component)
                component = audioSource.gameObject.AddComponent<ClockLinkComponent>();
            component.LinkAudioSource(clock.Type, audioSource);
        }
    }
}