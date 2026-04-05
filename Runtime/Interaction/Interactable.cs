using LK.Runtime.Utilities;
using UnityEngine;

namespace LK.Runtime.Interaction
{
    public class Interactable : TransformableObject
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveSmooth = 0.2f;

        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private float rotateSmooth = 0.2f;

        [SerializeField] private float scaleSpeed = 1f;
        [SerializeField] private float scaleSmooth = 0.2f;
        [SerializeField] private RangedFloat scaleRange = new RangedFloat(0.5f, 2f);

        private Vector3 _targetPosition;
        private Vector3 _targetRotation;
        private Vector3 _targetScale;

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _startScale;
        private bool _startInfoInitialized;

        private Vector3 _positionVelocity;
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
        
        public Vector3 TargetScale
        {
            get => _targetScale;
            set => _targetScale = value;
        }
        
        public override void ProcessMoveInput(Vector3 delta)
        {
            _targetPosition += delta * moveSpeed;
        }

        public override void ProcessRotateInput(Vector3 delta)
        {
            _targetRotation += new Vector3(delta.y, delta.x, 0f) * rotateSpeed;
        }

        public override void ProcessScaleInput(Vector3 delta)
        {
            _targetScale += delta * scaleSpeed;
            _targetScale = ClampScale(_targetScale);
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
                transform.localScale = _startScale;
            }
        }
        
        private void Start()
        {
            _targetPosition = transform.localPosition;
            _targetRotation = transform.localEulerAngles;
            _targetScale = transform.localScale;
            
            InitializeStartInfo();
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
            _startScale = transform.localScale;
            _startInfoInitialized = true;
        }
        
        private void AlignToTarget()
        {
            // 对齐位置
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetPosition, ref _positionVelocity, moveSmooth);

            // 对齐旋转
            var targetQuaternion = Quaternion.Euler(_targetRotation);
            var rotationStep = Quaternion.Angle(transform.localRotation, targetQuaternion) * Time.deltaTime / rotateSmooth;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, rotationStep);

            // 对齐缩放
            transform.localScale = Vector3.SmoothDamp(transform.localScale, _targetScale, ref _scaleVelocity, scaleSmooth);
        }

        private Vector3 ClampScale(Vector3 scale)
        {
            scale.x = Mathf.Clamp(scale.x, _startScale.x * scaleRange.Lower, _startScale.x * scaleRange.Upper);
            scale.y = Mathf.Clamp(scale.y, _startScale.y * scaleRange.Lower, _startScale.y * scaleRange.Upper);
            scale.z = Mathf.Clamp(scale.z, _startScale.z * scaleRange.Lower, _startScale.z * scaleRange.Upper);
            return scale;
        }
    }
}
