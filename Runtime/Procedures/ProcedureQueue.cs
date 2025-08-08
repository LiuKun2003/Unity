using System.Collections.Generic;
using UnityEngine;

namespace LK.Runtime.Procedures
{
    public class ProcedureQueue : Procedure
    {
        [SerializeField] private List<Procedure> procedures;
        
        private int _currentProcedureIndex;
        private bool _init;
        
        public override void Init()
        {
            _init = false;
            if (procedures == null || procedures.Count == 0)
            {
                Debug.LogError("No procedures or procedures count is zero.");
                return;
            }
            _init = true;
            
            base.Init();
            _currentProcedureIndex = 0;
            procedures[_currentProcedureIndex].Init();
        }

        public override void OnBegin()
        {
            if (!_init) return;
            base.OnBegin();
            procedures[_currentProcedureIndex].OnBegin();
        }

        public override void OnUpdate()
        {
            if (!_init) return;
            
            var procedure = procedures[_currentProcedureIndex];
            
            if (!procedure.IsCompleted)
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

        public override void OnEnd()
        {
            if (!_init) return;
            
            base.OnEnd();
            procedures[_currentProcedureIndex].OnEnd();
            
        }

        public override void Release()
        {
            if (!_init) return;
            
            base.Release();
            procedures[_currentProcedureIndex].Release();
        }

        private void SetCurrentProcedure(int index)
        {
            if (index < 0 || index >= procedures.Count)
            {
                Complete(index < 0);
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
