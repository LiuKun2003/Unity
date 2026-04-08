using System.Collections;
using System.Collections.Generic;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Validator
{
    public class ValidatorManager : MonoSingleton<ValidatorManager>
    {
        [SerializeField] private List<Validator> validators;
    
        public UnityEvent onSuccess;
        public UnityEvent onFailure;
    
        private IEnumerator Start()
        {
            foreach (var validator in validators)
            {
                yield return StartCoroutine(validator.Verify());
                if (validator.Result) continue;
                onFailure?.Invoke();
                yield break;
            }
            onSuccess?.Invoke();
        }
    }
}
