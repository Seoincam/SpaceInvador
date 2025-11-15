using System;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// GUI.enabled를 스코프 기반으로 임시 변경.
    /// </summary>
    public readonly struct GuiEnabledScope : IDisposable
    {
        private readonly bool _previous;

        public GuiEnabledScope(bool enabled)
        {
            _previous = GUI.enabled;
            GUI.enabled = enabled;
        }
        
        public void Dispose()
        {
            GUI.enabled = _previous;
        }
    }
}