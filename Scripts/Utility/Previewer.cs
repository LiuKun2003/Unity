using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    public class Previewer : UIBehaviour, IDragHandler, IScrollHandler
    {
        [SerializeField] private Transform target;
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float zoomMaxLimit = 5f;
        [SerializeField] private float zoomMinLimit = 0.5f;
        [SerializeField] private float zoomSmooth = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float rotationSmooth = 10f;
        
        private Vector3 _originalRotation;
        private float _originalScale;
        private Vector3 _targetRotation;
        private float _targetScale;
        
        public void ResetModel()
        {
            if (target == null) return;
            target.rotation = Quaternion.Euler(_originalRotation);
            target.localScale = Vector3.one * _originalScale;
            _targetRotation = _originalRotation;
            _targetScale = _originalScale;
        }
    
        protected override void Start()
        {
            base.Start();
            _originalRotation = target.eulerAngles;
            _originalScale = target.localScale.x;
            _targetRotation = target.eulerAngles;
            _targetScale = target.localScale.x;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            _targetRotation.x += eventData.delta.y * rotationSpeed;
            _targetRotation.y -= eventData.delta.x * rotationSpeed;
        }

        public void OnScroll(PointerEventData eventData)
        {
            _targetScale += eventData.scrollDelta.y * zoomSpeed;
            _targetScale = Mathf.Clamp(_targetScale, zoomMinLimit, zoomMaxLimit);
        }

        private void Update()
        {
            if(target == null) return;
            target.localRotation = Quaternion.Lerp(target.localRotation, Quaternion.Euler(_targetRotation), rotationSmooth * Time.deltaTime);
            target.localScale = Vector3.Lerp(target.localScale, _targetScale * Vector3.one, zoomSmooth * Time.deltaTime);
        }
    }
}