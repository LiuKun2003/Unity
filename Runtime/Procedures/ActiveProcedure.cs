using System.Collections.Generic;
using UnityEngine;

namespace LK.Runtime.Procedures
{
    public class ActiveProcedure : Procedure
    {
        [SerializeField] private List<GameObject> beActive;
        [SerializeField] private List<GameObject> beInactive;

        public override void OnBegin()
        {
            base.OnBegin();
            beInactive.ForEach((go) => go.SetActive(false));
            beActive.ForEach((go) => go.SetActive(true));
            Complete();
        }
    }
}
