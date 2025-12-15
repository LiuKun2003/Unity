using LK.Runtime.Utilities;
using UnityEngine;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Camera))]
    public class CameraOrbit : TransformableObject
    {
        [SerializeField] private Transform lookAt;
        [SerializeField] private Vector3 offset = Vector3.zero;
    
        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private RangedFloat rotationYRange = new RangedFloat(-90f, 90f);
        [SerializeField] private float rotateSmooth = 0.2f;
    
        [SerializeField] private float targetDistance = 5.0f;
        [SerializeField] private float distanceSpeed = 1f;
        [SerializeField] private RangedFloat distanceRange = new RangedFloat(1f, 10f);
        [SerializeField] private float distanceSmooth = 0.2f;
    
        private Vector3 _targetRotation;
        private float _distanceVelocity;

        private Vector3 _startRotation;
        private float _startDistance;
        private bool _startInfoInitialized;
    
        public void ResetView(bool smooth)
        {
            InitializeStartInfo();
            
            targetDistance = _startDistance;
            _targetRotation = _startRotation;
            
            if (!smooth)
            {
                ToTargetValues();
            }
        }
    
        public override void ProcessMoveInput(Vector3 delta)
        {
            // Unsupported feature
        }
    
        public override void ProcessRotateInput(Vector3 delta)
        {
            _targetRotation.x -= delta.y * rotateSpeed;
            _targetRotation.y += delta.x * rotateSpeed;
            _targetRotation.x = rotationYRange.ClampAngle(_targetRotation.x);
        }
    
        public override void ProcessScaleInput(Vector3 delta)
        {
            targetDistance -= delta.x * distanceSpeed;
            targetDistance = distanceRange.Clamp(targetDistance);
        }

        private void Start()
        {
            if (lookAt == null)
            {
                Debug.LogError("No lookAt target.");
                return;
            }

            _targetRotation = transform.rotation.eulerAngles;
        
            InitializeStartInfo();
            ToTargetValues();
        }

        private void LateUpdate()
        {
            if (lookAt == null) return;
            AlignToTarget();
        }
    
        private void AlignToTarget()
        {
            var point = lookAt.position + offset;
        
            var currentDistance = Vector3.Distance(transform.position, point);
            currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref _distanceVelocity, distanceSmooth);

            var currentRotation = transform.rotation;
            var targetQuaternion = Quaternion.Euler(_targetRotation);
            var rotationStep = Quaternion.Angle(currentRotation, targetQuaternion) * Time.deltaTime / rotateSmooth;
            currentRotation = Quaternion.RotateTowards(currentRotation, targetQuaternion, rotationStep);
            transform.rotation = RemoveRoll(currentRotation);
        
            var negDistance = new Vector3(0.0f, 0.0f, -currentDistance);
            var currentPosition = currentRotation * negDistance + point;
            transform.position = currentPosition;
        }

        private void InitializeStartInfo()
        {
            if(_startInfoInitialized) return;
            _startDistance = targetDistance;
            _startRotation = transform.rotation.eulerAngles;
            _startInfoInitialized = true;
        }
    
        private static Quaternion RemoveRoll(Quaternion rotation)
        {
            var euler = rotation.eulerAngles;
            return Quaternion.Euler(euler.x, euler.y, 0);
        }

        private void ToTargetValues()
        {
            var targetQuaternion = Quaternion.Euler(_targetRotation);
            var point = lookAt.position + offset;
            var negDistance = new Vector3(0.0f, 0.0f, -targetDistance);
            var pos = targetQuaternion * negDistance + point;
            transform.rotation = targetQuaternion;
            transform.position = pos;
        }
    }
}