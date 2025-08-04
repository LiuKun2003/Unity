using System.Collections.Generic;
using UnityEngine;

namespace Procedures
{
    public class ActiveProcedure : Procedure
    {
        [SerializeField] private List<GameObject> beActive;
        [SerializeField] private List<GameObject> beInactive;

        public override void OnBegin()
        {
            base.OnBegin();
            beActive.ForEach((go) => go.SetActive(true));
            beInactive.ForEach((go) => go.SetActive(false));
            Complete();
        }
    }
}
