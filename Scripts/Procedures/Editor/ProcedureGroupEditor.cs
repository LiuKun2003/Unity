using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Procedures.Editor
{
    [CustomEditor(typeof(ProcedureGroup))]
    public class ProcedureGroupEditor : UnityEditor.Editor
    {
        private SerializedProperty _proceduresProperty;
    
        public void OnEnable()
        {
            _proceduresProperty = serializedObject.FindProperty("procedures");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var group = (ProcedureGroup)target;
        
            if (GUILayout.Button("更新流程组"))
            {
                var procedures = new List<Procedure>();
                for (var i = 0; i < group.transform.childCount; i++)
                {
                    var procedure = group.transform.GetChild(i).GetComponent<Procedure>();
                    if (procedure != null)
                    {
                        procedures.Add(procedure);
                    }
                }

                _proceduresProperty.arraySize = procedures.Count;
                for(var i = 0; i < procedures.Count; i++)
                {
                    _proceduresProperty.GetArrayElementAtIndex(i).objectReferenceValue = procedures[i];
                }
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(group);
            }
        }
    }
}
