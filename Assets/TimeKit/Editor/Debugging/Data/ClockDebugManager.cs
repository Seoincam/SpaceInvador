#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeKit.Editor.Debugging.Data
{
    internal static class ClockDebugManager
    {
        internal static ClockDebugInfo[] Infos;
        internal static Dictionary<ClockType, ClockDebugInfo> InfoMap = new();

        internal static void Initialize()
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
}
#endif