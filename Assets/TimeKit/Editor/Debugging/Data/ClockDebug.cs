#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using TimeKit.Core.Linked;
using UnityEngine;

namespace TimeKit.Editor.Debugging.Data
{
    internal static class ClockDebug
    {
        internal static ClockDebugInfo[] Infos;
        internal static Dictionary<ClockType, ClockDebugInfo> InfoMap = new();

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
                var types = Enum.GetValues(typeof(ClockType))
                    .Cast<ClockType>()
                    .ToArray();
                Infos = new ClockDebugInfo[types.Length];

                for (int i = 0; i < types.Length; i++)
                {
                    var info = new ClockDebugInfo(types[i]);
                    Infos[i] = info;
                    InfoMap[types[i]] = info;
                }
            }
        }
        
        internal class ClockDebugInfo
        {
            private readonly Clock _clock;

            internal ClockType Type => _clock.Type;
            
            internal double Time => _clock.Time;
            internal float DeltaTime => _clock.DeltaTime;
            internal float TimeScale => _clock.TimeScale;
            internal bool IsStopped => _clock.IsStopped;

            internal List<IClockLinked> Linked => _clock.Linked.Group;
            
            internal ClockDebugInfo(ClockType clockType)
            {
                _clock = TimeManager.GetRealClock(clockType);
            }
        }
    }
}
#endif