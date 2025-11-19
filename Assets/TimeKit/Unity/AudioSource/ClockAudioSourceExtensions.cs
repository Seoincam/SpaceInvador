using TimeKit.Unity.AudioSource;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAudioSourceExtensions
    {
        public static IClock SetLink(this IClock clock, AudioSource audioSource)
        {
            Register(clock.Type, audioSource);
            return clock;
        }

        public static AudioSource WithClock(this AudioSource audioSource, IClock clock)
        {
            Register(clock.Type, audioSource);
            return audioSource;
        }

        private static void Register(ClockType clockType, AudioSource audioSource)
        {
            var link = audioSource.GetComponent<AudioSourceClockLink>();
            if (!link)
                link = audioSource.gameObject.AddComponent<AudioSourceClockLink>();
            
            link.Register(clockType, audioSource);
        }
    }
}