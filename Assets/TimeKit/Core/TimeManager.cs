using System;
using System.Collections.Generic;

namespace TimeKit.Core
{
    public static class TimeManager
    {
        private static readonly Dictionary<ClockType, IClock> _clockMap = new();

        public static IReadOnlyDictionary<ClockType, IClock> Clocks => _clockMap;

        public static bool IsInitialized { get; private set; }
        public static event Action Initialized;

        public static void Initialize()
        {
            if (IsInitialized) 
                return;
            
            _clockMap.Clear();
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                _clockMap[type] = new Clock(type);

            IsInitialized = true;
            Initialized?.Invoke();
        }
        
        public static void Tick(float unscaledDeltaTime)
        {
            foreach (var clock in _clockMap.Values)
                clock.Tick(unscaledDeltaTime);
        }
    }
}