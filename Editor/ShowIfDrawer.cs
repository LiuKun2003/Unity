#if UNITY_EDITOR
using LK.Runtime.Components;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldShow(property)
                ? EditorGUI.GetPropertyHeight(property, label)
                : 0f;
        }

        private bool ShouldShow(SerializedProperty property)
        {
            if (attribute is not ShowIfAttribute attr) return true;

            foreach (var fieldName in attr.ConditionFieldNames)
            {
                string conditionPath = property.propertyPath.Replace(property.name, fieldName);
                var condProp = property.serializedObject.FindProperty(conditionPath);

                if (condProp == null ||
                    condProp.propertyType != SerializedPropertyType.Boolean ||
                    !condProp.boolValue)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
#endif