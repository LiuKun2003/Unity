#if DOTWEEN
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Utility
{
    public class DotweenTrigger : MonoBehaviour
    {
        [Serializable]
        internal enum ActiveUpdateMode
        {
            /// <summary>
            /// 动画开始时激活
            /// </summary>
            ActiveOnAnimationStart = 0,
            /// <summary>
            /// 动画开始时失活
            /// </summary>
            InactiveOnAnimationStart = 1,
            /// <summary>
            /// 动画开始切换状态
            /// </summary>
            SwitchOnAnimationStart = 2,
            /// <summary>
            /// 动画结束时激活
            /// </summary>
            ActiveOnAnimationFinish = 3,
            /// <summary>
            /// 动画结束时失活
            /// </summary>
            InactiveOnAnimationFinish = 4,
            /// <summary>
            /// 动画结束时切换状态
            /// </summary>
            SwitchOnAnimationFinish = 5,
        }

        [Serializable]
        internal enum AnimationType
        {
            /// <summary>
            /// 动画将会结束在指定的值
            /// </summary>
            Target = 0,
            /// <summary>
            /// 增量式动画
            /// </summary>
            Increment = 1,
        }
        
        [Serializable]
        internal enum  AnimationSpace
        {
            World = 0,
            Local = 1,
        }
        
        [SerializeField] private DotweenTriggerGroup group;
        [SerializeField] private Transform animationRoot = null;
        [SerializeField] [Min(0f)]private float animationDuration = 0.5f;
        [SerializeField] private bool killOnAnimationStart = true;
        
        [SerializeField]
        private bool enableActive = false;
        [SerializeField]
        private ActiveUpdateMode activeUpdateMode = ActiveUpdateMode.SwitchOnAnimationStart;

        [SerializeField] 
        private bool enablePosition = false;
        [SerializeField]
        private float positionTimeWeight = 1f;
        [SerializeField]
        private Ease positionEaseType = Ease.Linear;
        [SerializeField]
        private AnimationSpace positionSpace = AnimationSpace.Local;
        [SerializeField]
        private AnimationType movementType = AnimationType.Target;
        [SerializeField]
        private bool initPosition = false;
        [SerializeField]
        private bool useStartTransform = false;
        [SerializeField]
        private Vector3 startPosition = Vector3.zero;
        [SerializeField]
        private Transform startTransform = null;
        [SerializeField]
        private Vector3 targetPosition = Vector3.zero;
        [SerializeField]
        private bool useTargetTransform = false;
        [SerializeField]
        private Transform targetTransform = null;
        [SerializeField]
        private Vector3 movementValue = Vector3.zero;
        [SerializeField]
        private bool useDirection = false;
        [SerializeField]
        private Vector3 movementDirection = Vector3.zero;
        [SerializeField] 
        private float movementDistance = 0f;
        
        [SerializeField]
        private bool enableRotation = false;
        [SerializeField]
        private float rotationTimeWeight = 1f;
        [SerializeField]
        private Ease rotationEaseType = Ease.Linear;
        [SerializeField]
        private AnimationSpace rotationSpace = AnimationSpace.Local;
        [SerializeField]
        private AnimationType rotationType = AnimationType.Target;
        [SerializeField]
        private bool initRotation = false;
        [SerializeField]
        private Vector3 startRotation = Vector3.zero;
        [SerializeField]
        private Vector3 targetRotation = Vector3.zero;
        [SerializeField]
        private bool useAxis;
        [SerializeField]
        private Vector3 rotationValue = Vector3.zero;
        [SerializeField]
        private Vector3 rotationAxis = Vector3.zero;
        [SerializeField]
        private float rotationAngle = 0f;
        
        [SerializeField]
        private bool enableScale = false;
        [SerializeField]
        private float scaleTimeWeight = 1f;
        [SerializeField]
        private Ease scaleEaseType = Ease.Linear;
        [SerializeField]
        private bool initScale = false;
        [SerializeField]
        private Vector3 startScale = Vector3.one;
        [SerializeField]
        private Vector3 targetScale = Vector3.one;

        public DotweenTriggerGroup Group
        {
            get => group;
            set
            {
                if (group == value) return;
                group.Unregister(this);
                group = value;
                group.Register(this);
            }
        }
        
        public void DoAnimation()
        {
            if (group == null)
            {
                Apply();
            }
            else
            {
                Debug.LogWarning("This DotweenTrigger is hosted by the specified group. Apply this animation through the group.");
            }
        }

        internal void Apply()
        {
            // 如果未指定根物体，则默认应用动画到自身
            var root = animationRoot == null ? transform : animationRoot;
            
            // 在开始清除已有的动画
            if (killOnAnimationStart)
            {
                root.DOKill();
            }
        
            //应用激活动画
            if (enableActive)
            {
                ApplyActiveAnimation(root);
            }
            
            //应用位移动画
            if (enablePosition)
            {
                ApplyPositionAnimation(root);
            }

            //应用旋转动画
            if (enableRotation)
            {
                ApplyRotationAnimation(root);
            }
            
            //应用缩放动画
            if (enableScale)
            {
                ApplyScaleAnimation(root);
            }
        }

        private void OnEnable()
        {
            if (group != null)
            {
                group.Register(this);
            }
        }

        private void OnDisable()
        {
            if (group != null)
            {
                group.Unregister(this);
            }
        }

        private void ApplyActiveAnimation(Transform root)
        {
            //根据枚举值来生成对应的 激活/失活/切换状态 的动画
            switch (activeUpdateMode)
            {
                case ActiveUpdateMode.ActiveOnAnimationStart:
                    root.gameObject.SetActive(true);
                    break;
                case ActiveUpdateMode.InactiveOnAnimationStart:
                    root.gameObject.SetActive(false);
                    break;
                case ActiveUpdateMode.SwitchOnAnimationStart:
                    root.gameObject.SetActive(!root.gameObject.activeSelf);
                    break;
                case ActiveUpdateMode.ActiveOnAnimationFinish:
                    DOTween.To(() => 0, _ => { }, 0, animationDuration).
                        SetTarget(root).
                        OnComplete(() => root.gameObject.SetActive(true));
                    break;
                case ActiveUpdateMode.InactiveOnAnimationFinish:
                    DOTween.To(() => 0, _ => { }, 0, animationDuration).
                        SetTarget(root).
                        OnComplete(() => root.gameObject.SetActive(false));
                    break;
                case ActiveUpdateMode.SwitchOnAnimationFinish:
                    DOTween.To(() => 0, _ => { }, 0, animationDuration).
                        SetTarget(root).
                        OnComplete(() => root.gameObject.SetActive(!root.gameObject.activeSelf));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyPositionAnimation(Transform root)
        {
            //是否初始化位置
            if (initPosition)
            {
                if (useStartTransform)
                {
                    if (startTransform != null)
                    {
                        root.position = startTransform.position;
                    }
                    else
                    {
                        Debug.LogWarning("StartTransform is null");
                    }
                }
                else
                {
                    if (positionSpace == AnimationSpace.World)
                    {
                        root.position = startPosition;
                    }
                    else
                    {
                        root.localPosition = startPosition;
                    }
                }
            }
            
            //位移动画时间
            var duration = animationDuration * positionTimeWeight;
            //根据枚举值来决定动画类型
            Tweener animTweener;
            switch (movementType)
            {
                case AnimationType.Target:
                    if (useTargetTransform)
                    {
                        if (targetTransform != null)
                        {
                            animTweener = root.DOMove(targetTransform.position, duration);
                        }
                        else
                        {
                            animTweener = null;
                            Debug.LogWarning("TargetTransform is null");
                        }
                    }
                    else
                    {
                        animTweener = positionSpace == AnimationSpace.World ? root.DOMove(targetPosition, duration) : root.DOLocalMove(targetPosition, duration);
                    }
                    break;
                case AnimationType.Increment:
                    Vector3 byValue;
                    if (useDirection)
                    {
                        var normalizedDirection = movementDirection.normalized;
                        byValue = normalizedDirection * movementDistance;
                    }
                    else
                    {
                        byValue = movementValue;
                    }
                    animTweener = positionSpace == AnimationSpace.World ? root.DOBlendableMoveBy(byValue, duration) : root.DOBlendableLocalMoveBy(byValue, duration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            //为动画设置曲线
            animTweener?.SetEase(positionEaseType);
        }

        private void ApplyRotationAnimation(Transform root)
        {
            if (initRotation)
            {
                if (rotationSpace == AnimationSpace.World)
                {
                    root.rotation = Quaternion.Euler(startRotation);
                }
                else
                {
                    root.localRotation = Quaternion.Euler(startRotation);
                }
            }

            var duration = animationDuration * rotationTimeWeight;
            Tweener animTweener;
            switch (rotationType)
            {
                case AnimationType.Target:
                    animTweener = rotationSpace == AnimationSpace.World ? root.DORotate(targetRotation, duration) : root.DOLocalRotate(targetRotation, duration);
                    break;
                case AnimationType.Increment:
                    Vector3 byValue;
                    if (useAxis)
                    {
                        var normalizedAxis = rotationAxis.normalized;
                        byValue = normalizedAxis * rotationAngle;
                    }
                    else
                    {
                        byValue = rotationValue;
                    }
                    
                    animTweener = rotationSpace == AnimationSpace.World ? root.DOBlendableRotateBy(byValue, duration) : root.DOBlendableLocalRotateBy(byValue, duration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            animTweener.SetEase(rotationEaseType);
        }
        
        private void ApplyScaleAnimation(Transform root)
        {
            //是否初始化缩放
            if (initScale)
            {
                root.localScale = startScale;
            }
            
            //缩放动画时间
            var duration = animationDuration * scaleTimeWeight;
            //缩放动画
            root.DOScale(targetScale, duration).SetEase(scaleEaseType);
        }
    }
}
#endif
