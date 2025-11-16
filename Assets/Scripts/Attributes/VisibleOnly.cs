
using UnityEngine;

namespace Attributes
{
    public class VisibleOnly : PropertyAttribute
    {
        public Mode VisibleOnlyMode { get; }

        public VisibleOnly(Mode mode = Mode.Always)
        {
            VisibleOnlyMode = mode;
        }
        
        public enum Mode
        {
            Always,
            EditModeOnly,
            PlayModeOnly
        }
    }
}