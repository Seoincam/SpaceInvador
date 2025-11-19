using System;
using System.Collections.Generic;
using TimeKit.Unity.Base;
using UnityEngine;

namespace TimeKit.Unity
{
    public static class ClockLinkedGroup
    {
        private static readonly List<IClockLinked> LinkedGroup = new();

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
                    var clock = TimeManager.Clocks[type];

                    clock.TimeScaleChanged -= Sync;
                    clock.Paused -= Sync;
                    clock.Resumed -= Sync;

                    clock.TimeScaleChanged += Sync;
                    clock.Paused += Sync;
                    clock.Resumed += Sync;
                }
            }
        }
        
        internal static void Register(IClockLinked linked)
        {
            LinkedGroup.Add(linked);
        }

        internal static void Unregister(IClockLinked linked)
        {
            LinkedGroup.Remove(linked);
        }

        private static void Sync(IReadOnlyClock clock)
        {
            foreach (var linked in LinkedGroup)
                linked.SyncWithClock(clock);
        }
    }
}