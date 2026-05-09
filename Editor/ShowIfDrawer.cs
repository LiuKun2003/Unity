#if UNITY_EDITOR
using System.Linq;
using LK.Runtime.Components;
using LK.Runtime.Utilities;
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
            ShowIfAttribute attr = attribute as ShowIfAttribute;
            
            return attr != null && attr.ConditionFieldNames
                    .Select(fieldName => property.propertyPath.Replace(property.name, fieldName))
                    .Select(conditionPath => property.serializedObject.FindProperty(conditionPath))
                    .All(condProp => condProp is { propertyType: SerializedPropertyType.Boolean, boolValue: true });
        }
    }
}
#endif