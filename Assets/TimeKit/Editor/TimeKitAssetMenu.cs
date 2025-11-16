#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace TimeKit.Editor
{
    public static class TimeKitAssetMenu
    {
        private const string define = "HAS_UNITASK";
        
        [MenuItem("TimeKit/Setup/Enable UniTask Support")]
        public static void EnableUniTaskSupport()
        {
            var group = EditorUserBuildSettings.selectedBuildTargetGroup;
            var target = NamedBuildTarget.FromBuildTargetGroup(group);

            var defines = PlayerSettings.GetScriptingDefineSymbols(target);
            if (defines.Contains(define))
            {
                Debug.Log("[TimeKit] HAS_UNITASK already defined");
            }
            else
            {
                var newDefines = string.Join(';', defines, define);
                PlayerSettings.SetScriptingDefineSymbols(target, newDefines);
                Debug.Log("[TimeKit] HAS_UNITASK define added.");
            }
        }       
    }
}
#endif