#if UNITY_EDITOR
using LK.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    [CustomPropertyDrawer(typeof(RangedFloat))]
    public class RangedDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var lowerProp = property.FindPropertyRelative("lower");
            var upperProp = property.FindPropertyRelative("upper");

            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            //Rect 
            var originalLabelWidth = EditorGUIUtility.labelWidth;
            const float space = 4f;
            EditorGUIUtility.labelWidth = 50f;
            var lowerRect = position;
            lowerRect.width = lowerRect.width * 0.5f - space;
            var upperRect = position;
            upperRect.x += lowerRect.width + space * 2;
            upperRect.width *= 0.5f;
            
            lowerProp.floatValue = EditorGUI.FloatField(lowerRect, new GUIContent(lowerProp.displayName), lowerProp.floatValue);
            upperProp.floatValue = EditorGUI.FloatField(upperRect, new GUIContent(upperProp.displayName), upperProp.floatValue);

            if (lowerProp.floatValue > upperProp.floatValue)
            {
                lowerProp.floatValue = upperProp.floatValue;
            }

            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUI.EndProperty();
        }
    }
}
#endif