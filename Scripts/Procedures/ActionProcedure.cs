using System;
using UnityEngine;
using UnityEngine.Events;

namespace Procedures
{
    public class ActionProcedure : Procedure
    {
        [SerializeField] private UnityEvent onAction;

        public override void OnBegin()
        {
            base.OnBegin();
            onAction?.Invoke();
            Complete();
        }
    }
}
