using System;
using UnityEngine;

namespace LK.Runtime.Setters
{
    public class AnimatorParameterSetter : MonoBehaviour, ISetter
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;
        [SerializeField] private AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Float;
        [SerializeField] private string parameterValue;
        
        public void Apply()
        {
            if (animator == null)
            {
                throw new ArgumentNullException(nameof(animator));
            }

            if (!CheckParameterNameAndType())
            {
                throw new ArgumentException("There is no parameter with the specified name and type in Animtor.", nameof(parameterName));
            }

            switch (parameterType)
            {
                case AnimatorControllerParameterType.Float:
                    if (float.TryParse(parameterValue, out var floatValue))
                    {
                        animator.SetFloat(parameterName, floatValue);
                    }
                    else
                    {
                        throw new ArgumentException("The value cannot be converted to float.", nameof(parameterValue));
                    }
                    break;
                case AnimatorControllerParameterType.Bool:
                    if (bool.TryParse(parameterValue, out var boolValue))
                    {
                        animator.SetBool(parameterName, boolValue);
                    }
                    else
                    {
                        throw new ArgumentException("The value cannot be converted to bool.", nameof(parameterValue));
                    }
                    break;
                case AnimatorControllerParameterType.Int:
                    if (int.TryParse(parameterValue, out var intValue))
                    {
                        animator.SetInteger(parameterName, intValue);
                    }
                    else
                    {
                        throw new ArgumentException("The value cannot be converted to integer.", nameof(parameterValue));
                    }
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(parameterName);
                    break;
            }
        }
        
        private bool CheckParameterNameAndType()
        {
            foreach (var parameter in animator.parameters)
            {
                if (parameter.name == parameterName && parameter.type == parameterType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
