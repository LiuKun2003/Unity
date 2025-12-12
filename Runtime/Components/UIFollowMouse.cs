using UnityEngine;
using UnityEngine.EventSystems;

namespace LK.Runtime.Components
{
    /// <summary>
    /// UI跟随鼠标组件
    /// </summary>
    [AddComponentMenu("UI/Effects/UIFollowTarget", 85)]
    public class UIFollowMouse : UIBehaviour
    {
        [SerializeField] private float smoothSpeed = 0.1f;
        [SerializeField] private Vector2 offset;

        private DrivenRectTransformTracker _tracker;
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector3 _currentVelocity;
        
        public void ResetPosition()
        {
            var endPoint = CalculateEndPoint();
            _rectTransform.position = endPoint;
        }
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            CacheCanvas();
        }
#endif
        
        protected override void OnCanvasHierarchyChanged()
        {
            base.OnCanvasHierarchyChanged();
            CacheCanvas(); 
        }
        
        protected override void Awake()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
        }
        
        private void Update()
        { 
            _tracker.Clear();
            _tracker.Add(this, _rectTransform, DrivenTransformProperties.AnchoredPosition3D);
            
            var endPoint = CalculateEndPoint();
            var smoothTime = smoothSpeed; // 转换为平滑时间
            _rectTransform.position = Vector3.SmoothDamp(
                _rectTransform.position,
                endPoint,
                ref _currentVelocity,
                smoothTime
            );
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _tracker.Clear();
        }
        
        private Vector3 CalculateEndPoint()
        {
            // 获取鼠标屏幕位置
            var mouseScreenPosition = GetMousePosition();

            // 应用偏移
            mouseScreenPosition += offset;
            
            var activeCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvas.transform as RectTransform,
                mouseScreenPosition,
                activeCamera,
                out var worldPoint
            );
            
            return worldPoint;
        }
        
        private static Vector2 GetMousePosition()
        {
            return Input.mousePosition;
        }
        
        private void CacheCanvas()
        {
            _canvas = GetComponentInParent<Canvas>();
        }
    }
}