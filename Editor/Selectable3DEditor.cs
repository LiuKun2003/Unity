using System;
using LK.Runtime.Components;
using LK.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    [CustomEditor(typeof(Selectable3D), true)]
    [CanEditMultipleObjects]
    public class Selectable3DEditor : UnityEditor.Editor
    {
        private SerializedProperty _interactableProperty;
        private SerializedProperty _transitionModeProperty;
        private SerializedProperty _targetRendererProperty;
        private SerializedProperty _ignoreChildrenProperty;
        private SerializedProperty _materialsBlockProperty;
        private SerializedProperty _eventStateProperty;
        
        private bool _showEvents;
        
        private string[] _propertyPathToExcludeForChildClasses;
        
        private void OnEnable()
        {
            var script = serializedObject.FindProperty("m_Script");
            _interactableProperty = serializedObject.FindProperty("interactable");
            _transitionModeProperty = serializedObject.FindProperty("transitionMode");
            
            _targetRendererProperty = serializedObject.FindProperty("targetRenderer");
            _ignoreChildrenProperty = serializedObject.FindProperty("ignoreChildren");
            _materialsBlockProperty = serializedObject.FindProperty("materialsBlock");
            
            _eventStateProperty = serializedObject.FindProperty("stateEvents");
            
            _propertyPathToExcludeForChildClasses = new[]
            {
                script.propertyPath,
                _interactableProperty.propertyPath,
                _transitionModeProperty.propertyPath,
                _targetRendererProperty.propertyPath,
                _ignoreChildrenProperty.propertyPath,
                _materialsBlockProperty.propertyPath,
                _eventStateProperty.propertyPath,
            };
        }
        
        public override void OnInspectorGUI()
        { 
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_interactableProperty);
            EditorGUILayout.PropertyField(_transitionModeProperty);
            var transitionMode = (Selectable3D.TransitionMode)_transitionModeProperty.enumValueIndex;
            ++EditorGUI.indentLevel;
            {
                switch (transitionMode)
                {
                    case Selectable3D.TransitionMode.None:
                        break;
                    case Selectable3D.TransitionMode.MaterialsSwap:
                        EditorGUILayout.PropertyField(_targetRendererProperty);
                        if (_targetRendererProperty.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox(
                                "You must have a Transform root in order to use a materials swap transition.",
                                MessageType.Warning);
                        }

                        EditorGUILayout.PropertyField(_ignoreChildrenProperty);
                        EditorGUILayout.PropertyField(_materialsBlockProperty);
                        break;
                    case Selectable3D.TransitionMode.Event:
                        _showEvents = EditorGUILayout.Foldout(_showEvents, "State Events");
                        if (_showEvents)
                        {
                            EditorGUILayout.PropertyField(_eventStateProperty);
                        }

                        EditorGUILayout.EndFoldoutHeaderGroup();
                        break;
                }
            }
            --EditorGUI.indentLevel;

            ChildClassPropertiesGUI();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ChildClassPropertiesGUI()
        {
            if (IsDerivedSelectableEditor())
                return;

            DrawPropertiesExcluding(serializedObject, _propertyPathToExcludeForChildClasses);
        }

        private bool IsDerivedSelectableEditor()
        {
            return GetType() != typeof(Selectable3DEditor);
        }
    }
}
