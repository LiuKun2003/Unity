using LK.Runtime.Procedures;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace LK.Editor.Procedures
{
    [CustomEditor(typeof(ProcedureController))]
    public class ProcedureControllerEditor : UnityEditor.Editor
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
                var controller = (ProcedureController)target;
                Undo.RecordObject(controller, "Collect");
                
                var procedures = ListPool<Procedure>.Get();
                for (var i = 0; i < controller.transform.childCount; i++)
                {
                    var procedure = controller.transform.GetChild(i).GetComponent<Procedure>();
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
                EditorUtility.SetDirty(controller);
            }
        }
    }
}
