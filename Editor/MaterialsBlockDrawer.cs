using System;
using System.Collections.Generic;
using System.Linq;
using LK.Runtime.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LK.Editor
{
    [CustomPropertyDrawer(typeof(MaterialsBlock))]
    public class MaterialsBlockDrawer : PropertyDrawer
    {
        private readonly string[] _paths = new []
        {
            "<NormalMaterials>k__BackingField",
            "<HighlightedMaterials>k__BackingField",
            "<PressedMaterials>k__BackingField",
            "<SelectedMaterials>k__BackingField",
            "<DisabledMaterials>k__BackingField"
        };
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            for (var i = 0; i < _paths.Length; i++)
            {
                EditorGUI.PropertyField(position, property.FindPropertyRelative(_paths[i]));
                position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(_paths[i]));
                if (i < _paths.Length - 1)
                {
                    position.y += EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = _paths.Sum(path => EditorGUI.GetPropertyHeight(property.FindPropertyRelative(path)) + EditorGUIUtility.standardVerticalSpacing);
            return height - EditorGUIUtility.standardVerticalSpacing;
        }
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            
            foreach (var path in _paths)
            {
                var prop = property.FindPropertyRelative(path);
                var field = new PropertyField(prop);
                container.Add(field);
            }

            return container;
        }
    }
}
