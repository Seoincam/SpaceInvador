using System;
using System.Collections.Generic;
using TimeKit.Core.Clock;
using TimeKit.Core.Save;
using UnityEngine;

namespace TimeKit
{
    public static class TimeManager
    {
        private static readonly Dictionary<ClockType, Clock> ClockMap = new();
        
        internal static bool IsInitialized { get; private set; }
        internal static event Action Initialized;

        internal static event Action Ticked;

        internal static void Initialize()
        {
            if (IsInitialized) 
                return;
            
            ClockMap.Clear();
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                ClockMap[type] = new Clock(type);

            IsInitialized = true;
            Initialized?.Invoke();
        }
        
        internal static void Tick(float unscaledDeltaTime)
        {
            foreach (var clock in ClockMap.Values)
                clock.Tick(unscaledDeltaTime);
            Ticked?.Invoke();
        }
        
        internal static Clock GetRealClock(ClockType type) => ClockMap[type];

        public static IClock GetClock(ClockType type) => GetRealClock(type);

        /// <summary>
        /// Captures snapshot data for all registered clocks.
        /// </summary>
        public static ClockSaveData CaptureSaveData()
        {
            return CaptureSaveDataInternal(ClockMap.Values);
        }

        /// <summary>
        /// Captures snapshot data for the specified clock types.
        /// </summary>
        /// <param name="clockTypes">Clock types to include in the snapshot.</param>
        public static ClockSaveData CaptureSaveData(params ClockType[] clockTypes)
        {
            if (clockTypes == null || clockTypes.Length == 0)
                return CaptureSaveData();
            
            var clocks = new List<Clock>(clockTypes.Length);

            foreach (var type in clockTypes)
            {
                if (!ClockMap.TryGetValue(type, out var clock))
                {
                    Debug.LogWarning($"Clock snapshot unsupported type: {type}. Skipped.");
                    continue;
                }
                
                clocks.Add(clock);
            }
            
            return CaptureSaveDataInternal(clocks);
        }

        /// <summary>
        /// Captures snapshot data for the provided clock collection.
        /// </summary>
        /// <param name="clocks">Clocks to snapshot.</param>
        private static ClockSaveData CaptureSaveDataInternal(IEnumerable<Clock> clocks)
        {
            var snapshots = new List<ClockSnapshot>();

            foreach (var clock in clocks)
                snapshots.Add(clock.CreateSnapshot());

            return new ClockSaveData()
            {
                clockSnapshots = snapshots.ToArray()
            };
        }

        /// <summary>
        /// Restores clock states from the given save data.
        /// </summary>
        /// <param name="saveData">Saved clock data to restore.</param>
        public static void RestoreFromSaveData(ClockSaveData saveData)
        {
            if (saveData?.clockSnapshots == null)
                return;

            foreach (var snapshot in saveData.clockSnapshots)
            {
                if (!ClockMap.TryGetValue(snapshot.type, out var clock))
                {
                    Debug.LogWarning($"Clock restoring unsupported type: {snapshot.type}. Skipped.");
                    continue;
                }
                
                clock.Restore(snapshot);
            }
        }
    }
}