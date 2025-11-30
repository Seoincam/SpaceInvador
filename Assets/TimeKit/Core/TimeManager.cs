using System;
using System.Collections.Generic;

namespace TimeKit
{
    public static class TimeManager
    {
        private static readonly Dictionary<ClockType, Clock> _clockMap = new();
        
        public static bool IsInitialized { get; private set; }
        public static event Action Initialized;

        internal static event Action Ticked;

        internal static void Initialize()
        {
            if (IsInitialized) 
                return;
            
            _clockMap.Clear();
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                _clockMap[type] = new Clock(type);

            IsInitialized = true;
            Initialized?.Invoke();
        }
        
        internal static void Tick(float unscaledDeltaTime)
        {
            foreach (var clock in _clockMap.Values)
                clock.Tick(unscaledDeltaTime);
            Ticked?.Invoke();
        }

        public static IClock GetClock(ClockType type) => GetRealClock(type);

        internal static Clock GetRealClock(ClockType type) => _clockMap[type];
    }
}