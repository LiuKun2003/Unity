using UnityEngine;

namespace LK.Runtime.Utility
{
    public class AnimatorParameter : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        public void SetBool(bool value)
        {
            if (animator != null)
            {
                animator.SetBool(parameterName, value);
            }
        }

        public void SetFloat(float value)
        {
            if (animator != null)
            {
                animator.SetFloat(parameterName, value);
            }
        }

        public void SetInt(int value)
        {
            if (animator != null)
            {
                animator.SetInteger(parameterName, value);
            }
        }

        public void Trigger()
        {
            if (animator != null)
            {
                animator.SetTrigger(parameterName);
            }
        }
    }
}
