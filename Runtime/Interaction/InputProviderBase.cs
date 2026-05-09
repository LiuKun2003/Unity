using System;
using UnityEngine;

namespace LK.Runtime.Interaction
{
    public abstract class InputProviderBase : MonoBehaviour
    {
        public enum OutMode
        {
            None,
            Move,
            Rotate,
            Scale
        }
        
        [SerializeField] private TransformableObject target;
        [SerializeField] private OutMode outMode = OutMode.None;
        
        public TransformableObject Target { get => target; set => target = value; }
        public OutMode Out { get => outMode; set => outMode = value; }
        
#if UNITY_EDITOR
        protected virtual void Reset()
        {
            target = GetComponentInChildren<TransformableObject>();
            if (target != null) return;
            target = GetComponentInParent<TransformableObject>();
        }
#endif

        protected void OutDelta(Vector3 delta)
        {
            if (target == null) return;
            switch (Out)
            {
                case OutMode.None:
                    break;
                case OutMode.Move:
                    target.ProcessMoveInput(delta);
                    break;
                case OutMode.Rotate:
                    target.ProcessRotateInput(delta);
                    break;
                case OutMode.Scale:
                    target.ProcessScaleInput(delta);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
