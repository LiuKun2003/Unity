using LK.Runtime.Procedures;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace LK.Editor.Procedures
{
    [CustomEditor(typeof(ProcedureRace))]
    public class ProcedureRaceEditor : UnityEditor.Editor
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
                var race = (ProcedureRace)target;
                Undo.RecordObject(race, "Collect");

                var procedures = ListPool<Procedure>.Get();
                for (var i = 0; i < race.transform.childCount; i++)
                {
                    var procedure = race.transform.GetChild(i).GetComponent<Procedure>();
                    if (procedure != null)
                    {
                        procedures.Add(procedure);
                    }
                }

                _proceduresProperty.arraySize = procedures.Count;
                for (var i = 0; i < procedures.Count; i++)
                {
                    _proceduresProperty.GetArrayElementAtIndex(i).objectReferenceValue = procedures[i];
                }

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(race);
            }
        }
    }
}
