using UnityEngine;
using UnityEngine.EventSystems;

namespace LK.Runtime.Components
{
    [AddComponentMenu("UI/Effects/UIFollowTarget", 84)]
    public class UIFollowTarget : UIBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Camera activeCameraInScreenSpaceOverlay;
        
        private Canvas _canvas;
        private DrivenRectTransformTracker _tracker;
        private RectTransform _rectTransform;
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            CacheCanvas();
            activeCameraInScreenSpaceOverlay = Camera.main;
        }
#endif

        protected override void OnCanvasHierarchyChanged()
        {
            base.OnCanvasHierarchyChanged();
            CacheCanvas(); 
        }

        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
            Follow();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            _tracker.Clear();
        }
        
        private void Update()
        {
            _tracker.Clear();
            _tracker.Add(this, _rectTransform, DrivenTransformProperties.AnchoredPosition3D);
            Follow();
        }
        
        private void CacheCanvas()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        private void Follow()
        {
            if(target == null) return;
            var activeCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
            var screenPoint = (activeCamera ?? activeCameraInScreenSpaceOverlay).WorldToScreenPoint(target.transform.position + offset);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, screenPoint, activeCamera, out var worldPoint);
            _rectTransform.position = worldPoint;
        }
    }
}
