using UnityEngine;
using UnityEngine.UI;

namespace LK.Runtime.Procedures
{
    public class ButtonProcedure : Procedure
    {
        [SerializeField] private Button button;

        public override void OnBegin()
        {
            base.OnBegin();
            button.onClick.AddListener(Complete);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            button.onClick.RemoveListener(Complete);
        }
    }
}
