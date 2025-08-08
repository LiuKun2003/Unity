using System;
using System.Collections.Generic;
using UnityEngine;

namespace LK.Runtime.Procedures
{
    public class ProcedureController : MonoBehaviour
    {
        [SerializeField] private List<Procedure> procedures;
        [SerializeField] private int startProcedureIndex = 0;
        
        private int _currentProcedureIndex;
        private bool _init;
        
        private int _newProcedureIndex;
        private bool _dirty;
        
        public void SetProcedure(int index)
        {
            if(!_init) return;
            
            if (index >= procedures.Count || index < 0)
            {
                Debug.LogError("index is out of range.");
                return;
            }
            _newProcedureIndex = index;
            _dirty = true;
        }

        public void NextProcedure()
        {
            SetProcedure(_currentProcedureIndex + 1);
        }

        public void PreviousProcedure()
        {
            SetProcedure(_currentProcedureIndex - 1);
        }

        private void Awake()
        {
            Init();
            
            for (var i = 0; i < procedures.Count; i++)
            {
                var procedure = procedures[i];
                if (i == startProcedureIndex)
                {
                    procedure.Init();
                }
                else
                {
                    procedure.Release();
                }
            }
        }

        private void Start()
        {
            if(!_init) return;
            
            procedures[_currentProcedureIndex].OnBegin();
        }

        private void Update()
        {
            if(!_init) return;
            
            var procedure = procedures[_currentProcedureIndex];
            
            if (_dirty)
            {
                _dirty = false;
                SetCurrentProcedure(_newProcedureIndex);
            }
            else if (!procedure.IsCompleted)
            {
                procedure.OnUpdate();
            }
            else if (procedure.IsBack)
            {
                SetCurrentProcedure(_currentProcedureIndex - 1);
            }
            else
            {
                SetCurrentProcedure(_currentProcedureIndex + 1);
            }
        }

        private void Init()
        {
            _init = false;
            
            if (procedures == null || procedures.Count == 0)
            {
                Debug.LogError("No procedures or procedures count is zero.");
                return;
            }

            if (startProcedureIndex >= procedures.Count || startProcedureIndex < 0)
            {
                Debug.LogError("StartProcedureIndex out of range.");
                return;
            }
            
            _init = true;
        }
        
        private void SetCurrentProcedure(int index)
        {
            if (index >= procedures.Count || index < 0)
            {
                Debug.Log("No more procedure.");
                return;
            }
            procedures[_currentProcedureIndex].OnEnd();
            procedures[_currentProcedureIndex].Release();
            _currentProcedureIndex = index;
            procedures[_currentProcedureIndex].Init();
            procedures[_currentProcedureIndex].OnBegin();
        }
    }
}
