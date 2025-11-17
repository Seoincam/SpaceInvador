using TimeKit.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace TimeKit.Unity
{
    internal static class TimeBootstrapper
    {
        private static PlayerLoopSystem _system;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize()
        {
            TimeManager.Initialize();
            
            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            if (!InsertTimeManager<Update>(ref currentPlayerLoop, 0))
            {
                Debug.LogWarning("Failed to register TimeManager into the Update loop.");
                return;
            }
            
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged += OnPlayModeState;

            static void OnPlayModeState(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                    PlayerLoopUtils.RemoveSystem<Update>(ref currentPlayerLoop, _system);
                    PlayerLoop.SetPlayerLoop(currentPlayerLoop);
                }
            }
#endif
        }

        private static bool InsertTimeManager<T>(ref PlayerLoopSystem loopSystem, int index)
        {
            _system = new PlayerLoopSystem()
            {
                type = typeof(TimeManager),
                updateDelegate = TimeManagerUpdate,
                subSystemList = null
            };

            return PlayerLoopUtils.InsertSystem<T>(ref loopSystem, in _system, index);
        }

        private static void TimeManagerUpdate()
        {
            float udt = Time.unscaledDeltaTime;
            TimeManager.Tick(udt);
        }
    }
}