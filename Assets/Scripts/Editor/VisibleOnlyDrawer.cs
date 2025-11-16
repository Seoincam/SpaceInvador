using Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(VisibleOnly))]
    public class VisibleOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var visibleOnly = attribute as VisibleOnly;

            bool canEdit = visibleOnly?.VisibleOnlyMode switch
            {
                VisibleOnly.Mode.Always => false,
                VisibleOnly.Mode.EditModeOnly => Application.isPlaying,
                VisibleOnly.Mode.PlayModeOnly => !Application.isPlaying,
                _ => false
            };

            using (new GuiEnabledScope(canEdit))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}