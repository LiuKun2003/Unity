using System;
using System.Collections.Generic;
using System.Diagnostics;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace LK.Runtime.Procedures
{
    public class ProcedureController : MonoBehaviour
    {
        [SerializeField] private List<Procedure> procedures;
        [SerializeField] private int startProcedureIndex = 0;
        
        private int _currentProcedureIndex = -1;
        private bool _init;
        private bool _isRunning;
        private bool disableStateCache = true;
        private bool hasDisabled;
        
        private bool Running
        {
            get => _isRunning;
            set
            {
                if (!SetPropertyUtility.SetStruct(ref _isRunning, value)) return;
                
                if (value)
                {
                    InitializeAllProcedure();
                }
                else
                {
                    ReleaseAllProcedure();
                }
            }
        }

        public void Activate()
        {
            Running = true;
            Debug.Log("ProcedureController : Active");
        }
        
        public void Deactivate()
        {
            Running = false;
            Debug.Log("ProcedureController : Inactive");
        }
        
        public void Restart()
        {
            if(_init) ReleaseAllProcedure();
            InitializeAllProcedure();
            BeginStartProcedure();
            _isRunning = true;
            hasDisabled = !gameObject.activeSelf;
        }
        
        public void SetProcedure(int index)
        {
            EvaluateAndSetCurrentProcedure(index);
        }

        public void NextProcedure()
        {
            EvaluateAndSetCurrentProcedure(_currentProcedureIndex + 1);
        }

        public void PreviousProcedure()
        {
            EvaluateAndSetCurrentProcedure(_currentProcedureIndex - 1);
        }

        private void OnEnable()
        {
            if(!hasDisabled) return;
            Running = disableStateCache;
        }

        private void Update()
        {
            if(!Running) return;
            var procedure = procedures[_currentProcedureIndex];
            
            if (!procedure.IsCompleted)
            {
                procedure.OnUpdate();
            }
            else if (procedure.IsBack)
            {
                if (_currentProcedureIndex - 1 < 0)
                {
                    End();
                }
                else
                {
                    SetCurrentProcedure(_currentProcedureIndex - 1);
                }
            }
            else
            {
                if (_currentProcedureIndex + 1 >= procedures.Count)
                {
                    End();
                }
                else
                {
                    SetCurrentProcedure(_currentProcedureIndex + 1);
                }
            }
        }

        private void OnDisable()
        {
            disableStateCache = Running;
            Running = false;
            hasDisabled = true;
        }

        private void EvaluateAndSetCurrentProcedure(int index)
        {
            if (index >= procedures.Count || index < 0) throw new ArgumentOutOfRangeException(nameof(index), index, "Index is out of range.");
            SetCurrentProcedure(index);
        }
        
        /// <summary>
        /// 初始化所有<see cref="Procedure"/>
        /// </summary>
        private void InitializeAllProcedure()
        {
            foreach (var procedure in procedures)
            {
                procedure.Init();
            }
            
            _init = true;
            Debug.Log("ProcedureController : All Procedure are initialized.");
        }
        
        /// <summary>
        /// 释放所有<see cref="Procedure"/>
        /// </summary>
        private void ReleaseAllProcedure()
        {
            foreach (var procedure in procedures)
            {
                procedure.Release();
            }
            _init = false;
            Debug.Log("ProcedureController : All procedure are released.");
        }
        
        /// <summary>
        /// 设置当前<see cref="Procedure"/>
        /// </summary>
        private void SetCurrentProcedure(int index)
        {
            var procedure = procedures[_currentProcedureIndex];
            procedures[_currentProcedureIndex].OnEnd();
            _currentProcedureIndex = index;
            procedures[_currentProcedureIndex].OnBegin();
            LogCurrentProcedure();
        }

        private void BeginStartProcedure()
        {
            if (startProcedureIndex < 0 || startProcedureIndex >= procedures.Count)
            {
                Debug.LogError("Start Procedure Index is out of range.");
            }
            
            _currentProcedureIndex = startProcedureIndex;
            procedures[_currentProcedureIndex].OnBegin();
            LogCurrentProcedure();
        }

        private void End()
        {
            Running = false;
            procedures[_currentProcedureIndex].OnEnd();
            _currentProcedureIndex = -1;
            Debug.Log("ProcedureController : All procedure are completed.");
        }

        private void LogCurrentProcedure()
        {
            Debug.Log($"ProcedureController current procedure : {procedures[_currentProcedureIndex].name} [{procedures[_currentProcedureIndex].GetType().Name}]");
        }
    }
}
