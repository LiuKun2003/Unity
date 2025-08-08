using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LK.Runtime.Procedures
{
    public class ProcedureGroup : Procedure
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
            var complete = true;
            
            foreach (var procedure in procedures)
            {
                procedure.OnUpdate();
                complete &= procedure.IsCompleted;
            }

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
                procedure.OnEnd();
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
