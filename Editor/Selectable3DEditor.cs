using System;
using LK.Runtime.Components;
using LK.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    [CustomEditor(typeof(Selectable3D), true)]
    [CanEditMultipleObjects]
    public class Button3DEditor : UnityEditor.Editor
    {
        private SerializedProperty _interactable, _transition;
        private SerializedProperty _root, _ignoreChildren, _normalMaterials, _highlightedMaterials, _pressedMaterials, _disabledMaterials;
        private SerializedProperty _normal, _highlighted, _pressed, _disabled;
        
        private bool _showEvents;
        
        private string[] _propertyPathToExcludeForChildClasses;
        
        private void OnEnable()
        {
            var script = serializedObject.FindProperty("m_Script");
            _interactable = serializedObject.FindProperty("interactable");
            _transition = serializedObject.FindProperty("transition");
            
            _root = serializedObject.FindProperty("root");
            _ignoreChildren = serializedObject.FindProperty("ignoreChildren");
            _normalMaterials = serializedObject.FindProperty("normalMaterials");
            _highlightedMaterials = serializedObject.FindProperty("highlightedMaterials");
            _pressedMaterials = serializedObject.FindProperty("pressedMaterials");
            _disabledMaterials = serializedObject.FindProperty("disabledMaterials");
            
            _normal = serializedObject.FindProperty("normal");
            _highlighted = serializedObject.FindProperty("highlighted");
            _pressed = serializedObject.FindProperty("pressed");
            _disabled = serializedObject.FindProperty("disabled");
            
            _propertyPathToExcludeForChildClasses = new[]
            {
                script.propertyPath,
                _interactable.propertyPath,
                _transition.propertyPath,
                _root.propertyPath,
                _ignoreChildren.propertyPath,
                _normalMaterials.propertyPath,
                _highlightedMaterials.propertyPath,
                _pressedMaterials.propertyPath,
                _disabledMaterials.propertyPath,
                _normal.propertyPath,
                _highlighted.propertyPath,
                _pressed.propertyPath,
                _disabled.propertyPath
            };
        }
        
        public override void OnInspectorGUI()
        { 
            EditorGUILayout.PropertyField(_interactable);
            
            switch (DrawEnumProperty<Selectable3D.Transition>(_transition))
            {
                case Selectable3D.Transition.None:
                    break;
                case Selectable3D.Transition.MaterialsSwap:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_root);
                    if (_root.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("You must have a Transform root in order to use a materials swap transition.", MessageType.Warning);
                    }
                    EditorGUILayout.PropertyField(_ignoreChildren);
                    EditorGUILayout.PropertyField(_normalMaterials);
                    EditorGUILayout.PropertyField(_highlightedMaterials);
                    EditorGUILayout.PropertyField(_pressedMaterials);
                    EditorGUILayout.PropertyField(_disabledMaterials);
                    EditorGUI.indentLevel--;
                    break;
                case Selectable3D.Transition.Event:
                    _showEvents = EditorGUILayout.BeginFoldoutHeaderGroup(_showEvents, "Events");
                    if (_showEvents)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(_normal);
                        EditorGUILayout.PropertyField(_highlighted);
                        EditorGUILayout.PropertyField(_pressed);
                        EditorGUILayout.PropertyField(_disabled);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EditorGUILayout.Space(10);
            
            DrawPropertiesExcluding(serializedObject, _propertyPathToExcludeForChildClasses);
            
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
        
        private static T DrawEnumProperty<T>(SerializedProperty property, params GUILayoutOption[] options) where T : Enum
        {
            var selected = (T)Enum.ToObject(typeof(T), property.intValue);
            var newValue = (T)EditorGUILayout.EnumPopup(property.displayName, selected, options);
            property.intValue = Convert.ToInt32(newValue);
            return newValue;
        }
    }
}
