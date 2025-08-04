using System.Collections.Generic;
using UnityEngine;

namespace Procedures
{
    public class ProcedureController : MonoBehaviour
    {
        //[SerializeField] private bool debugLog = true;
        [SerializeField] private List<Procedure> procedures;
        [SerializeField] private int startProcedureIndex = 0;
        
        private int _currentProcedureIndex;
        
        private void Awake()
        {
            if (startProcedureIndex >= procedures.Count || startProcedureIndex < 0)
            {
                Debug.Log("StartProcedureIndex out of range.");
                startProcedureIndex = Mathf.Clamp(startProcedureIndex, 0, procedures.Count - 1);
            }
            
            for (var i = 0; i < procedures.Count; i++)
            {
                var procedure = procedures[i];
                if (i == startProcedureIndex)
                {
                    procedure.gameObject.SetActive(true);
                    procedure.OnBegin();
                }
                else
                {
                    procedure.gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (!procedures[_currentProcedureIndex].IsCompleted) return;
            
            if (procedures[_currentProcedureIndex].IsBack)
            {
                SetCurrentProcedure(_currentProcedureIndex - 1);
            }
            else
            {
                SetCurrentProcedure(_currentProcedureIndex + 1);
            }
        }
        
        private void SetCurrentProcedure(int index)
        {
            if (index >= procedures.Count || index < 0)
            {
                Debug.LogError("Procedure Index out of range.");
                return;
            }
            procedures[_currentProcedureIndex].OnEnd();
            procedures[_currentProcedureIndex].gameObject.SetActive(false);
            _currentProcedureIndex = index;
            procedures[_currentProcedureIndex].gameObject.SetActive(true);
            procedures[_currentProcedureIndex].OnBegin();
        }
    }
}
