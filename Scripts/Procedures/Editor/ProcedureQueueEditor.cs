using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Procedures.Editor
{
    [CustomEditor(typeof(ProcedureQueue))]
    public class ProcedureQueueEditor : UnityEditor.Editor
    {
        private SerializedProperty _proceduresProperty;
    
        public void OnEnable()
        {
            _proceduresProperty = serializedObject.FindProperty("procedures");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var queue = (ProcedureQueue)target;
        
            if (GUILayout.Button("更新流程队列"))
            {
                var procedures = new List<Procedure>();
                for (var i = 0; i < queue.transform.childCount; i++)
                {
                    var procedure = queue.transform.GetChild(i).GetComponent<Procedure>();
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
                EditorUtility.SetDirty(queue);
            }
        }
    }
}
