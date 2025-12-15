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

        }
        
        public virtual void OnBegin()
        {
            gameObject.SetActive(true);
            _isCompleted = false;
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnEnd()
        {
            _isCompleted = false;
            gameObject.SetActive(false);
        }

        public virtual void Release()
        {

        }
        
        protected void Complete()
        {
            Complete(false);
        }
        
        protected void Complete(bool isBack)
        {
            _isCompleted = true;
            _isBack = isBack;
        }
    }
}
