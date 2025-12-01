using System;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    [CustomEditor(typeof(Button3D))]
    [CanEditMultipleObjects]
    public class Button3DEditor : UnityEditor.Editor
    {
        private SerializedProperty _interactable, _transition, _onClick;
        private SerializedProperty _root, _ignoreChildren, _normalMaterials, _highlightedMaterials, _pressedMaterials, _disabledMaterials;
        
        
        private void OnEnable()
        {
            _interactable = serializedObject.FindProperty("interactable");
            _transition = serializedObject.FindProperty("transition");
            _onClick = serializedObject.FindProperty("onClick");
            _root = serializedObject.FindProperty("root");
            _ignoreChildren = serializedObject.FindProperty("ignoreChildren");
            _normalMaterials = serializedObject.FindProperty("normalMaterials");
            _highlightedMaterials = serializedObject.FindProperty("highlightedMaterials");
            _pressedMaterials = serializedObject.FindProperty("pressedMaterials");
            _disabledMaterials = serializedObject.FindProperty("disabledMaterials");
        }
        
        public override void OnInspectorGUI()
        { 
            EditorGUILayout.PropertyField(_interactable);
            
            switch (DrawEnumProperty<Button3D.Transition>(_transition))
            {
                case Button3D.Transition.None:
                    break;
                case Button3D.Transition.MaterialsSwap:
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(_onClick);
            
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
