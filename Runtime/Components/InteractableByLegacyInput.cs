using System;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Collider))]
    public class InteractableByLegacyInput : MonoBehaviour
    {
        [SerializeField] private int moveKey = 0;
        [SerializeField] private int rotateKey = 1;
        [SerializeField] private string scaleKey = "Mouse ScrollWheel";
        
        public UnityEvent<Vector3> onMove;
        public UnityEvent<Vector3> onRotate;
        public UnityEvent<float> onScale;

        private bool _isMoving;
        private bool _isRotating;
        private bool _isScaling;
        
        private void Update()
        {
            if (_isMoving)
            {
                onMove?.Invoke(GetMouseXY());
                _isMoving = !Input.GetMouseButtonUp(moveKey);
            }
            if (_isRotating)
            {
                onRotate?.Invoke(GetMouseXY());
                _isRotating = !Input.GetMouseButtonUp(rotateKey);
            }
            if (_isScaling)
            {
                onScale?.Invoke(Input.GetAxis(scaleKey));
                _isScaling = Input.GetAxis(scaleKey) != 0;
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(moveKey))
            {
                _isMoving = !_isRotating; 
            }
            if (Input.GetMouseButtonDown(rotateKey))
            {
                _isRotating = !_isMoving;
            }
            if (Input.GetAxis(scaleKey) != 0)
            {
                _isScaling = true;
            }
        }

        private static Vector3 GetMouseXY()
        {
            return new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0f);
        }
    }
}
