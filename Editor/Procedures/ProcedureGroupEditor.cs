using LK.Runtime.Procedures;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace LK.Editor.Procedures
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
            EditorGUILayout.Separator();
            if (GUILayout.Button("Collect"))
            {
                var group = (ProcedureGroup)target;
                Undo.RecordObject(group, "Collect");
                
                var procedures = ListPool<Procedure>.Get();
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
