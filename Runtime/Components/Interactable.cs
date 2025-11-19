using System;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] private string moveEnableAxis = "Fire1";
        [SerializeField] private string moveXAxis = "Mouse X";
        [SerializeField] private string moveYAxis = "Mouse Y";
        [SerializeField] private float moveSpeed = 0f;
        [SerializeField] private float moveSmooth = 10f;
        
        [Header("Rotation")]
        [SerializeField] private string rotateEnableAxis = "Fire2";
        [SerializeField] private string rotateXAxis = "Mouse X";
        [SerializeField] private string rotateYAxis = "Mouse Y";
        [SerializeField] private float rotateSpeed = 0f;
        [SerializeField] private float rotateSmooth = 10f;

        [Header("Scale")] 
        [SerializeField] private string scaleEnableAxis = "Mouse ScrollWheel";
        [SerializeField] private string scaleValueAxis = "Mouse ScrollWheel";
        [SerializeField] private float scaleSpeed = 0f;
        [SerializeField] private float scaleSmooth = 10f;
        [SerializeField] private RangedFloat scaleRange = new RangedFloat(1f, 10f);
        
        private Vector3 _targetPosition;
        private Vector3 _targetRotation;
        private float _targetScale;
        
        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private float _startScale;

        private bool _ignoreEnable;
        private bool _moving;
        private bool _rotating;
        private bool _scaling;
        
        public void ResetModel()
        {
            _targetPosition = _startPosition;
            _targetRotation = _startRotation;
            _targetScale = _startScale;
        }
        
        private void Start()
        {
            _startPosition = transform.localPosition;
            _startRotation = transform.eulerAngles;
            _startScale = transform.localScale.x;
            
            _targetPosition = transform.localPosition;
            _targetRotation = transform.eulerAngles;
            _targetScale = transform.localScale.x;
        }

        private void Update()
        {
            GetInput();
            AlignToTarget();
        }

        private void OnMouseEnter()
        {
            if(_moving || _rotating || _scaling) return;
            _ignoreEnable = Input.GetAxis(moveEnableAxis) > 0f 
                            || Input.GetAxis(rotateEnableAxis) > 0f 
                            || Input.GetAxis(scaleEnableAxis) != 0f;
        }

        private void OnMouseOver()
        {
            if (Input.GetAxis(moveEnableAxis) > 0f)
            {
                _moving = !_ignoreEnable;
            }
            else if(Input.GetAxis(rotateEnableAxis) > 0f)
            {
                _rotating = !_ignoreEnable;
            }
            else if (Input.GetAxis(scaleEnableAxis) != 0f)
            {
                _scaling = !_ignoreEnable;
            }
            else
            {
                _ignoreEnable = false;
            }
        }

        private void GetInput()
        {
            if (_moving)
            {
                var raw = new Vector3(Input.GetAxisRaw(moveXAxis), Input.GetAxisRaw(moveYAxis), 0f).normalized;
                _targetPosition += raw * (moveSpeed * Time.deltaTime);
                if (Input.GetAxis(moveEnableAxis) <= 0f)
                {
                    _moving = false;
                }
            }
            
            if (_rotating)
            {
                var raw = new Vector3(Input.GetAxisRaw(rotateYAxis), -Input.GetAxisRaw(rotateXAxis), 0f).normalized;
                _targetRotation += raw * (rotateSpeed * Time.deltaTime);
                if (Input.GetAxis(rotateEnableAxis) <= 0f)
                {
                    _rotating = false;
                }
            }

            if (_scaling)
            {
                var raw = Input.GetAxisRaw(scaleEnableAxis);
                _targetScale += raw * scaleSpeed * Time.deltaTime;
                _targetScale = scaleRange.Clamp(_targetScale);
                if (Input.GetAxis(scaleEnableAxis) == 0f)
                {
                    _scaling = false;
                }
            }
        }
        
        private void AlignToTarget()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, moveSmooth * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_targetRotation), rotateSmooth * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale * Vector3.one, scaleSmooth * Time.deltaTime);
        }
    }
}
