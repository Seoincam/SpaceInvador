using System.Diagnostics;
using System.Reflection;
using Attributes;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
    [CustomPropertyDrawer(typeof(DisableIf))]
    [CustomPropertyDrawer(typeof(EnableIf))]
    public class ConditionalEnableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool canEdit = EvaluateCondition(property);

            using (new GuiEnabledScope(canEdit))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        /// <returns>활성화 가능한가?</returns>
        private bool EvaluateCondition(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
            if (target == null)
                return true; // 못 찾으면 활성화

            string memberName;
            bool invert;

            if (attribute is DisableIf disable)
            {
                memberName = disable.ConditionMemberName;
                invert = true;
            }
            else if (attribute is EnableIf enable)
            {
                memberName = enable.ConditionMemberName;
                invert = false;
            }
            else
                return true;

            var type = target.GetType();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            bool conditionValue = false;
            bool found = false;

            var field = type.GetField(memberName, flags);
            if (field != null && field.FieldType == typeof(bool))
            {
                conditionValue = (bool)field.GetValue(target);
                found = true;
            }
            else
            {
                var prop = type.GetProperty(memberName, flags);
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    conditionValue = (bool)prop.GetValue(target);
                    found = true;
                }
            }

            if (!found)
            {
                Debug.LogWarning($"[ConditionalEnableDrawer] Could not find bool member '{memberName}' on '{type.Name}'.");
                return true; // 못 찾으면 활성화
            }

            return invert ? !conditionValue : conditionValue;
        }
    }
}