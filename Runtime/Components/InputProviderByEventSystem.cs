using System;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace LK.Runtime.Components
{
    public class InputProviderByEventSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IScrollHandler
    {
        [SerializeField] private TransformableObject target;
        [SerializeField] private bool enableMoveInput = true;
        [SerializeField] private bool enableRotateInput = true;
        [SerializeField] private bool enableScaleInput = true;
        
        [SerializeField] private PointerEventData.InputButton moveKey = PointerEventData.InputButton.Left;
        [SerializeField] private PointerEventData.InputButton rotateKey = PointerEventData.InputButton.Right;
        
        private bool _isPressed;
        private PointerEventData.InputButton _lastPressedButton;
        
#if UNITY_EDITOR
        private void Reset()
        {
            target = GetComponent<TransformableObject>();
        }
#endif

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _lastPressedButton = eventData.button;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_isPressed) return;
            if(enableMoveInput && moveKey == _lastPressedButton) target.ProcessMoveInput(eventData.delta);
            if(enableRotateInput && rotateKey == _lastPressedButton) target.ProcessRotateInput(eventData.delta);
        }
        
        public void OnScroll(PointerEventData eventData)
        {
            if (!enableScaleInput) return;
            target.ProcessScaleInput(eventData.scrollDelta.y * Vector3.one);
        }
    }
}
