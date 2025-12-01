#if UNITY_EDITOR
using System.Collections.Generic;
using TimeKit.Core.Linked;
using UnityEngine;

namespace TimeKit.Editor.Debugging.Data
{
    internal sealed class ClockLinkDebugInfo
    {
        // Common
        internal string TargetType;
        
        // GameObject
        internal GameObject TargetGameObject;
        internal string TargetGameObjectName;
        internal string SceneName;
        internal string HierarchyPath;

        internal ClockLinkDebugInfo(IClockLinked linked)
        {
            TargetType = linked.Target?.GetType().Name ?? string.Empty;

            var go = linked.GameObject;
            TargetGameObject = go;

            if (go)
            {
                var scene = go.scene;
                SceneName = scene.IsValid() ?scene.name : string.Empty;
                HierarchyPath = BuildHierarchyPath(go.transform);
                TargetGameObjectName = go.name;
            }
            else
            {
                SceneName = string.Empty;
                HierarchyPath = string.Empty;
                TargetGameObjectName = string.Empty;
            }
        }

        private static string BuildHierarchyPath(Transform t)
        {
            var names = new List<string>();
            while (t)
            {
                names.Add(t.name);
                t = t.parent;
            }
            names.Reverse();
            return string.Join("/", names);
        }
    }
}
#endif