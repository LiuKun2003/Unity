using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(RawImage))]
    public class InteractableByRawImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler
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
            }
            if (_isRotating)
            {
                onRotate?.Invoke(GetMouseXY());
            }
            if (_isScaling)
            {
                onScale?.Invoke(Input.GetAxis(scaleKey));
            }
        }
        
        private static Vector3 GetMouseXY()
        {
            return new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(_isMoving || _isRotating) return;
            
            if (Input.GetMouseButtonDown(moveKey))
            {
                _isMoving = true; 
            }
            else if (Input.GetMouseButtonDown(rotateKey))
            {
                _isRotating = true;
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(moveKey))
            {
                _isMoving = false; 
            }
            else if (Input.GetMouseButtonUp(rotateKey))
            {
                _isRotating = false;
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            _isScaling = Input.GetAxis(scaleKey) != 0;
        }
    }
}