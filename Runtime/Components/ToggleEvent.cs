using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleEvent : MonoBehaviour
    {
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        private Toggle _toggle;
        
        private void Awake() => (_toggle = GetComponent<Toggle>()).onValueChanged.AddListener(TriggerEvent);

        private void OnEnable() => (_toggle.isOn ? onTrue : onFalse)?.Invoke();

        private void TriggerEvent(bool value) => (value ? onTrue : onFalse)?.Invoke();
    }
}
