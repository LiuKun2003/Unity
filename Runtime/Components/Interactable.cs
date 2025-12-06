using System;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace LK.Runtime.Components
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveSmooth = 0.2f;

        [SerializeField] private float rotateSpeed = 30f;
        [SerializeField] private float rotateSmooth = 0.2f;

        [SerializeField] private float scaleSpeed = 1f;
        [SerializeField] private float scaleSmooth = 0.2f;
        [SerializeField] private RangedFloat scaleRange = new RangedFloat(1f, 10f);

        private Vector3 _targetPosition;
        private Vector3 _targetRotation;
        private float _targetScale;

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private float _startScale;
        private bool _startInfoInitialized;

        private Vector3 _positionVelocity;
        private Vector3 _rotationVelocity;
        private Vector3 _scaleVelocity;
        
        public Vector3 TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }
        
        public Vector3 TargetRotation
        {
            get => _targetRotation;
            set => _targetRotation = value;
        }
        
        public float TargetScale
        {
            get => _targetScale;
            set => _targetScale = value;
        }

        public void InputMove(Vector3 delta)
        {
            _targetPosition += delta * moveSpeed;
        }

        public void InputRotate(Vector3 delta)
        {
            _targetRotation += new Vector3(delta.y, -delta.x, 0f) * rotateSpeed;
        }

        public void InputScale(float delta)
        {
            _targetScale = scaleRange.Clamp(_targetScale + delta * scaleSpeed);
        }

        public void ResetModel(bool smooth)
        {
            InitializeStartInfo();
            
            _targetPosition = _startPosition;
            _targetRotation = _startRotation;
            _targetScale = _startScale;
            
            if (!smooth)
            {
                transform.localPosition = _startPosition;
                transform.localEulerAngles = _startRotation;
                transform.localScale = Vector3.one * _startScale;
            }
        }
        
        private void Start()
        {
            _targetPosition = transform.localPosition;
            _targetRotation = transform.eulerAngles;
            _targetScale = transform.localScale.x;
        }
        
        private void LateUpdate()
        {
            AlignToTarget();
        }

        private void InitializeStartInfo()
        {
            if(_startInfoInitialized) return;
            _startPosition = transform.localPosition;
            _startRotation = transform.localEulerAngles;
            _startScale = transform.localScale.x;
            _startInfoInitialized = true;
        }
        
        private void AlignToTarget()
        {
            // 对齐位置
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetPosition,
                ref _positionVelocity, moveSmooth);

            // 对齐旋转
            var targetQuaternion = Quaternion.Euler(_targetRotation);
            var rotationStep = Quaternion.Angle(transform.localRotation, targetQuaternion) * Time.deltaTime / rotateSmooth;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, rotationStep);

            // 对齐缩放
            var targetVector3Scale = _targetScale * Vector3.one;
            transform.localScale =
                Vector3.SmoothDamp(transform.localScale, targetVector3Scale, ref _scaleVelocity, scaleSmooth);
        }
    }
}
