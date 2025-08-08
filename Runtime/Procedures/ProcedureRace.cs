using System.Collections.Generic;
using UnityEngine;

namespace LK.Runtime.Procedures
{
    public class ProcedureRace : Procedure
    {
        [SerializeField] private List<Procedure> procedures;

        public override void Init()
        {
            base.Init();
            foreach (var procedure in procedures)
            {
                procedure.Init();
            }
        }

        public override void OnBegin()
        {
            base.OnBegin();
            foreach (var procedure in procedures)
            {
                procedure.OnBegin();
            }
        }

        public override void OnUpdate()
        {
            var complete = false;
            
            foreach (var procedure in procedures)
            {
                procedure.OnUpdate();
                complete |= procedure.IsCompleted;
            }

            complete |= procedures.Count == 0;
            
            if (complete)
            {
                Complete();
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();
            foreach (var procedure in procedures)
            {
                if (procedure.IsCompleted)
                {
                    procedure.OnEnd();
                }
            }
        }

        public override void Release()
        {
            base.Release();
            foreach (var procedure in procedures)
            {
                procedure.Release();
            }
        }
    }
}
