using System;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace LK.Runtime.Components
{
    public class InputProviderByLegacyInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler
    {
        [SerializeField] private TransformableObject target;
        [SerializeField] private bool enableMoveInput = true;
        [SerializeField] private bool enableRotateInput = true;
        [SerializeField] private bool enableScaleInput = true;
        
        [SerializeField] private PointerEventData.InputButton moveKey = PointerEventData.InputButton.Left;
        [SerializeField] private PointerEventData.InputButton rotateKey = PointerEventData.InputButton.Right;
        
        [SerializeField] private string mouseXAxis = "Mouse X";
        [SerializeField] private string mouseYAxis = "Mouse Y";
        [SerializeField] private string scaleAxis = "Mouse ScrollWheel";
        
        private bool _isPressed;
        private PointerEventData.InputButton _lastPressedButton;
        
#if UNITY_EDITOR
        private void Reset()
        {
            target = GetComponent<TransformableObject>();
        }
#endif

        public void Update()
        {
            if (!_isPressed) return;
            if(enableMoveInput && moveKey == _lastPressedButton) target.ProcessMoveInput(GetMouseXYDelta());
            if(enableRotateInput && rotateKey == _lastPressedButton) target.ProcessRotateInput(GetMouseXYDelta());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _lastPressedButton = eventData.button;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }
        
        public void OnScroll(PointerEventData eventData)
        {
            if (!enableScaleInput) return;
            target.ProcessScaleInput(GetScrollDelta());
        }
        
        private Vector3 GetMouseXYDelta()
        {
            return new Vector3(Input.GetAxisRaw(mouseXAxis), Input.GetAxisRaw(mouseYAxis), 0f);
        }

        private Vector3 GetScrollDelta()
        {
            return Input.GetAxisRaw(scaleAxis) * Vector3.one;
        }
    }
}
