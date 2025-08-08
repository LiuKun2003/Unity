using UnityEngine;

namespace LK.Runtime.Procedures
{
    public abstract class  Procedure : MonoBehaviour
    {
        private bool _isCompleted;
        private bool _isBack;
        public bool IsCompleted => _isCompleted;
        public bool IsBack => _isBack;

        public virtual void Init()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void OnBegin()
        {
            _isCompleted = false;
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnEnd()
        {
            _isCompleted = false;
        }

        public virtual void Release()
        {
            gameObject.SetActive(false);
        }
        
        protected void Complete()
        {
            _isCompleted = true;
            _isBack = false;
        }
        
        protected void Complete(bool isBack)
        {
            _isCompleted = true;
            _isBack = isBack;
        }
    }
}
