using System.Collections.Generic;
using UnityEngine;

namespace Procedures
{
    public class ProcedureQueue : Procedure
    {
        [SerializeField] private List<Procedure> procedures;
        
        private int _currentProcedureIndex;
    
        public override void OnBegin()
        {
            base.OnBegin();
            _currentProcedureIndex = 0;
            for (var i = 1; i < procedures.Count; i++)
            {
                procedures[i].gameObject.SetActive(false);
            }
            procedures[_currentProcedureIndex].gameObject.SetActive(true);
            procedures[_currentProcedureIndex].OnBegin();
        }

        public void Update()
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

        public override void OnEnd()
        {
            base.OnEnd();
            procedures[_currentProcedureIndex].OnEnd();
            procedures[_currentProcedureIndex].gameObject.SetActive(false);
        }
        
        private void SetCurrentProcedure(int index)
        {
            if (index < 0)
            {
                Complete(true);
            }
            else if (index >= procedures.Count)
            {
                Complete();
            }
            else
            {
                procedures[_currentProcedureIndex].OnEnd();
                procedures[_currentProcedureIndex].gameObject.SetActive(false);
                _currentProcedureIndex = index;
                procedures[_currentProcedureIndex].gameObject.SetActive(true);
                procedures[_currentProcedureIndex].OnBegin();
            }
        }
    }
}
