using UnityEngine;

namespace LK.Runtime.Utility
{
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float zoomMaxLimit = 5f;
        [SerializeField] private float zoomMinLimit = 0.5f;
        [SerializeField] private float zoomSmooth = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float rotationSmooth = 10f;
    
        private Vector3 _targetRotation;
        private float _targetScale;
    
        private void Start()
        {
            _targetRotation = transform.eulerAngles;
            _targetScale = transform.localScale.x;
        }

        private void Update()
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_targetRotation), rotationSmooth * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale * Vector3.one, zoomSmooth * Time.deltaTime);
        }

        private void OnMouseDrag()
        {
            _targetRotation.x += Input.GetAxisRaw("Mouse Y") * rotationSpeed;
            _targetRotation.y -= Input.GetAxisRaw("Mouse X") * rotationSpeed;
        }

        private void OnMouseOver()
        {
            _targetScale += Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
            _targetScale = Mathf.Clamp(_targetScale, zoomMinLimit, zoomMaxLimit);
        }
    }
}
