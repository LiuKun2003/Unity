using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LK.Runtime.Utility
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
        
        protected override void Start()
        {
            base.Start();
            _rectTransform = GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            _tracker.Clear();
            if(target == null) return;
            _tracker.Add(gameObject, _rectTransform, DrivenTransformProperties.AnchoredPosition3D);
            var activeCamera = _canvas.renderMode switch
            {
                RenderMode.ScreenSpaceOverlay => null,
                RenderMode.ScreenSpaceCamera or RenderMode.WorldSpace => _canvas.worldCamera,
                _ => throw new ArgumentOutOfRangeException()
            };
            var screenPoint = (activeCamera ?? activeCameraInScreenSpaceOverlay).WorldToScreenPoint(target.transform.position + offset);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, screenPoint, activeCamera, out var worldPoint);
            _rectTransform.position = worldPoint;
        }

        protected override void OnCanvasHierarchyChanged()
        {
            base.OnCanvasHierarchyChanged();
            CacheCanvas(); 
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _tracker.Clear();
        }
        
        private void CacheCanvas()
        {
            _canvas = GetComponentInParent<Canvas>();
        }
    }
}
