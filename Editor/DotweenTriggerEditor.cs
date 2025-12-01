#if DOTWEEN
#if UNITY_EDITOR
using System;
using LK.Runtime.Utility;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace LK.Editor
{
    [CustomEditor(typeof(DotweenTrigger))]
    [CanEditMultipleObjects]
    public class DotweenTriggerEditor : UnityEditor.Editor
    {
        private SerializedProperty _group, _animationRoot, _animationDuration, _killOnAnimationStart;
        private SerializedProperty _enableActive, _activeUpdateMode;
        private SerializedProperty _enablePosition, _positionTimeWeight, _positionEaseType, _positionSpace, _movementType,
            _initPosition, _useStartTransform, _startPosition, _startTransform, _targetPosition, _useTargetTransform, _targetTransform, _useDirection,
            _movementValue, _movementDirection, _movementDistance;
        private SerializedProperty _enableRotation, _rotationTimeWeight, _rotationEaseType, _rotationType, _rotationSpace, _initRotation,
            _startRotation, _targetRotation, _useAxis, _rotationValue, _rotationAxis, _rotationAngle;
        private SerializedProperty _enableScale, _scaleTimeWeight, _scaleEaseType, _initScale, _startScale, _targetScale;

        private AnimBool _showActiveGroup, _showPositionGroup, _showRotationGroup, _showScaleGroup;
        
        private void OnEnable()
        {
            _group = serializedObject.FindProperty("group");
            _animationRoot = serializedObject.FindProperty("animationRoot");
            _animationDuration = serializedObject.FindProperty("animationDuration");
            _killOnAnimationStart = serializedObject.FindProperty("killOnAnimationStart");
            _enableActive = serializedObject.FindProperty("enableActive");
            _activeUpdateMode = serializedObject.FindProperty("activeUpdateMode");
            _enablePosition = serializedObject.FindProperty("enablePosition");
            _positionTimeWeight = serializedObject.FindProperty("positionTimeWeight");
            _positionEaseType = serializedObject.FindProperty("positionEaseType");
            _movementType = serializedObject.FindProperty("movementType");
            _positionSpace = serializedObject.FindProperty("positionSpace");
            _initPosition = serializedObject.FindProperty("initPosition");
            _useStartTransform = serializedObject.FindProperty("useStartTransform");
            _startPosition = serializedObject.FindProperty("startPosition");
            _startTransform = serializedObject.FindProperty("startTransform");
            _targetPosition = serializedObject.FindProperty("targetPosition");
            _useTargetTransform = serializedObject.FindProperty("useTargetTransform");
            _targetTransform = serializedObject.FindProperty("targetTransform");
            _useDirection = serializedObject.FindProperty("useDirection");
            _movementValue = serializedObject.FindProperty("movementValue");
            _movementDirection = serializedObject.FindProperty("movementDirection");
            _movementDistance = serializedObject.FindProperty("movementDistance");
            _enableRotation = serializedObject.FindProperty("enableRotation");
            _rotationTimeWeight = serializedObject.FindProperty("rotationTimeWeight");
            _rotationEaseType = serializedObject.FindProperty("rotationEaseType");
            _rotationType = serializedObject.FindProperty("rotationType");
            _rotationSpace = serializedObject.FindProperty("rotationSpace");
            _initRotation = serializedObject.FindProperty("initRotation");
            _startRotation = serializedObject.FindProperty("startRotation");
            _targetRotation = serializedObject.FindProperty("targetRotation");
            _useAxis = serializedObject.FindProperty("useAxis");
            _rotationValue = serializedObject.FindProperty("rotationValue");
            _rotationAxis = serializedObject.FindProperty("rotationAxis");
            _rotationAngle = serializedObject.FindProperty("rotationAngle");
            _enableScale = serializedObject.FindProperty("enableScale");
            _scaleTimeWeight = serializedObject.FindProperty("scaleTimeWeight");
            _scaleEaseType = serializedObject.FindProperty("scaleEaseType");
            _initScale = serializedObject.FindProperty("initScale");
            _startScale = serializedObject.FindProperty("startScale");
            _targetScale = serializedObject.FindProperty("targetScale");
            
            _showActiveGroup = new AnimBool(_enableActive.boolValue);
            _showPositionGroup = new AnimBool(_enablePosition.boolValue);
            _showRotationGroup = new AnimBool(_enableRotation.boolValue);
            _showScaleGroup = new AnimBool(_enableScale.boolValue);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_group);
            EditorGUILayout.PropertyField(_animationRoot);
            if (_animationRoot.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("If the root of the animation application is not specified, the animation will be applied by default on this transform.", MessageType.Info);
            }
            EditorGUILayout.PropertyField(_animationDuration);
            EditorGUILayout.PropertyField(_killOnAnimationStart);
            
            //Active组
            EditorGUILayout.BeginVertical(GUI.skin.box);
            _showActiveGroup.target = DrawLeftToggleForBoolProperty(_enableActive, EditorStyles.whiteBoldLabel);
            if (EditorGUILayout.BeginFadeGroup(_showActiveGroup.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_activeUpdateMode);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            //Position组
            EditorGUILayout.BeginVertical(GUI.skin.box);
            _showPositionGroup.target = DrawLeftToggleForBoolProperty(_enablePosition, EditorStyles.whiteBoldLabel);
            if (EditorGUILayout.BeginFadeGroup(_showPositionGroup.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(_positionTimeWeight, 0.0f, 1.0f);
                EditorGUILayout.PropertyField(_positionEaseType);
                EditorGUILayout.PropertyField(_positionSpace);
                if (DrawToggleForBoolProperty(_initPosition))
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_useStartTransform);
                    EditorGUILayout.PropertyField(_useStartTransform.boolValue ? _startTransform : _startPosition);
                    EditorGUI.indentLevel--;
                }

                switch (DrawEnumProperty<DotweenTrigger.AnimationType>(_movementType))
                {
                    case DotweenTrigger.AnimationType.Target:
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(DrawToggleForBoolProperty(_useTargetTransform)
                            ? _targetTransform
                            : _targetPosition);
                        EditorGUI.indentLevel--;
                        break;
                    case DotweenTrigger.AnimationType.Increment:
                        EditorGUI.indentLevel++;
                        if (DrawToggleForBoolProperty(_useDirection))
                        {
                            EditorGUILayout.PropertyField(_movementDirection);
                            EditorGUILayout.PropertyField(_movementDistance);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(_movementValue);
                        }
                        EditorGUI.indentLevel--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            //Rotation组
            EditorGUILayout.BeginVertical(GUI.skin.box);
            _showRotationGroup.target = DrawLeftToggleForBoolProperty(_enableRotation, EditorStyles.whiteBoldLabel);
            if (EditorGUILayout.BeginFadeGroup(_showRotationGroup.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(_rotationTimeWeight, 0.0f, 1.0f);
                EditorGUILayout.PropertyField(_rotationEaseType);
                EditorGUILayout.PropertyField(_rotationSpace);
                if (DrawToggleForBoolProperty(_initRotation))
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_startRotation);
                    EditorGUI.indentLevel--;
                }

                switch (DrawEnumProperty<DotweenTrigger.AnimationType>(_rotationType))
                {
                    case DotweenTrigger.AnimationType.Target:
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(_targetRotation);
                        EditorGUI.indentLevel--;
                        break;
                    case DotweenTrigger.AnimationType.Increment:
                        EditorGUI.indentLevel++;
                        if (DrawToggleForBoolProperty(_useAxis))
                        {
                            EditorGUILayout.PropertyField(_rotationAxis);
                            EditorGUILayout.PropertyField(_rotationAngle);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(_rotationValue);
                        }

                        EditorGUI.indentLevel--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            //Scale组
            EditorGUILayout.BeginVertical(GUI.skin.box);
            _showScaleGroup.target = DrawLeftToggleForBoolProperty(_enableScale, EditorStyles.whiteBoldLabel);
            if (EditorGUILayout.BeginFadeGroup(_showScaleGroup.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(_scaleTimeWeight, 0.0f, 1.0f);
                EditorGUILayout.PropertyField(_scaleEaseType);
                if (DrawToggleForBoolProperty(_initScale))
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_startScale);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.PropertyField(_targetScale);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Apply"))
                {
                    var dotweenTrigger = (DotweenTrigger)target;
                    dotweenTrigger.Apply();
                }
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        private static bool DrawLeftToggleForBoolProperty(SerializedProperty property, GUIStyle guiStyle = null, params GUILayoutOption[] options)
        {
            property.boolValue = guiStyle == null ? EditorGUILayout.ToggleLeft(property.displayName, property.boolValue, options) : EditorGUILayout.ToggleLeft(property.displayName, property.boolValue, guiStyle, options);
            return property.boolValue;
        }
        
        private static bool DrawToggleForBoolProperty(SerializedProperty property, params GUILayoutOption[] options)
        {
            return property.boolValue = EditorGUILayout.Toggle(property.displayName, property.boolValue, options);
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
#endif
#endif