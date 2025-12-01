#if UNITY_EDITOR
using UnityEngine;

namespace TimeKit.Editor.Debugging.Data
{
    internal static class ClockDebugBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize()
        {
            if (TimeManager.IsInitialized)
            {
                ClockDebugManager.Initialize();
            }
            else
            {
                TimeManager.Initialized -= ClockDebugManager.Initialize;
                TimeManager.Initialized += ClockDebugManager.Initialize;
            }
        }
    }
}
#endif