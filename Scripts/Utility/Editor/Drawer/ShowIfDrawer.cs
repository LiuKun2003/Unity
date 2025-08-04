#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utility.Editor.Drawer
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
            return attr.Condition == null || attr.Condition.Invoke();
        }
    }
}
#endif