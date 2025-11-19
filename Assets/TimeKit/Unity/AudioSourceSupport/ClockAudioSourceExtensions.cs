using TimeKit.Unity;
using TimeKit.Unity.AudioSourceSupport;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAudioSourceExtensions
    {
        public static IClock SetLink(this IClock clock, AudioSource audioSource)
        {
            Bind(clock.Type, audioSource);
            return clock;
        }

        public static AudioSource WithClock(this AudioSource audioSource, IReadOnlyClock clock)
        {
            Bind(clock.Type, audioSource);
            return audioSource;
        }

        private static void Bind(ClockType clockType, AudioSource audioSource)
        {
            if (!audioSource)
                return;

            var link = ClockLinkComponentUtils.GetOrAddLinkComponent<AudioSourceClockLink, AudioSource>(audioSource);
            link.Bind(clockType, audioSource);
        }
    }
}