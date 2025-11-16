using System;
using TimeKit.Core;
using UnityEngine;

#if DOTWEEN
namespace TimeKit.DOTween
{
    internal static class DOTweenBootsrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            if (TimeManager.IsInitialized)
            {
                Register();
            }
            else
            {
                TimeManager.Initialized -= Register;
                TimeManager.Initialized += Register;
            }
        }

        private static void Register()
        {
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
            {
                var clock = TimeManager.Clocks[type];

                clock.TimeScaleChanged -= clock.SyncTween;
                clock.Paused -= clock.SyncTween;
                clock.Resumed -= clock.SyncTween;
                
                clock.TimeScaleChanged += clock.SyncTween;
                clock.Paused += clock.SyncTween;
                clock.Resumed += clock.SyncTween;
            }
        }
    }
}
#endif