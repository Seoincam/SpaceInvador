using System;
using System.Collections.Generic;
using UnityEngine;

namespace TimeKit.Unity.AudioSource
{
    internal static class AudioSourceClockLinkGroup
    {
        private static readonly Dictionary<ClockType, List<UnityEngine.AudioSource>> Group = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            if (TimeManager.IsInitialized)
            {
                OnInitialized();
            }
            else
            {
                TimeManager.Initialized -= OnInitialized;
                TimeManager.Initialized += OnInitialized;
            }

            return;
            void OnInitialized()
            {
                foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                {
                    Group[type] = new List<UnityEngine.AudioSource>();
                
                    var clock = TimeManager.Clocks[type];

                    clock.TimeScaleChanged -= SyncAudioSource;
                    clock.Paused -= SyncAudioSource;
                    clock.Resumed -= SyncAudioSource;

                    clock.TimeScaleChanged += SyncAudioSource;
                    clock.Paused += SyncAudioSource;
                    clock.Resumed += SyncAudioSource;
                }
            }
        }

        private static void SyncAudioSource(IReadOnlyClock clock)
        {
            var list = Group[clock.Type];
            foreach (var audioSource in list)
            {
                if (clock.IsStopped)
                    audioSource.Pause();
                else
                    audioSource.pitch = clock.TimeScale;
            }
        }

        public static void Register(ClockType type, UnityEngine.AudioSource audioSource)
        {
            if (Group[type].Contains(audioSource))
                return;
            Group[type].Add(audioSource);
        }

        public static void Unregister(ClockType type, UnityEngine.AudioSource audioSource)
        {
            Group[type].Remove(audioSource);
        }
    }
}