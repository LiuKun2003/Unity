using UnityEngine;

namespace LK.Runtime.Utilities
{
    public abstract class TransformableObject : MonoBehaviour
    {
        public abstract void ProcessMoveInput(Vector3 delta);
        public abstract void ProcessRotateInput(Vector3 delta);
        public abstract void ProcessScaleInput(Vector3 delta);
    }
}
