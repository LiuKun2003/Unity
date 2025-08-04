using UnityEngine;

namespace Procedures
{
    public abstract class Procedure : MonoBehaviour
    {
        private bool _isCompleted;
        private bool _isBack;
        public bool IsCompleted => _isCompleted;
        public bool IsBack => _isBack;
        
        public virtual void OnBegin()
        {
            _isCompleted = false;
        }

        public virtual void OnEnd()
        {
            _isCompleted = false;
        }

        protected void Complete(bool isBack = false)
        {
            _isCompleted = true;
            _isBack = isBack;
        }
    }
}
