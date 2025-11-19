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

            AudioSourceClockLink link = null;

            var links = audioSource.GetComponents<AudioSourceClockLink>();
            if (links != null)
            {
                foreach (var l in links)
                {
                    if (!l || l.Target != audioSource) continue;
                    link = l;
                    break;
                }
            }
            
            if (!link)
                link = audioSource.gameObject.AddComponent<AudioSourceClockLink>();
            
            link.Bind(clockType, audioSource);
        }
    }
}