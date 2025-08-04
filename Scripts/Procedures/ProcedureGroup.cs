using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedures
{
    public class ProcedureGroup : Procedure
    {
        [SerializeField] private List<Procedure> procedures;
     
        public override void OnBegin()
        {
            base.OnBegin();
            foreach (var procedure in procedures)
            {
                procedure.gameObject.SetActive(true);
                procedure.OnBegin();
            }
        }

        private void Update()
        {
            var isCompleted = procedures.Aggregate(false, (current, procedure) => current | procedure.IsCompleted);
            if (isCompleted)
            {
                Complete(procedures.Last().IsBack);
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();
            foreach (var procedure in procedures)
            {
                procedure.OnEnd();
                procedure.gameObject.SetActive(false);
            }
        }
    }
}
