using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility
{
    [AddComponentMenu("UI/Effects/UIFollowTarget", 84)]
    public class UIFollowTarget : UIBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera activeCameraInScreenSpaceOverlay;
        
        private DrivenRectTransformTracker  _tracker;
        private RectTransform _rectTransform;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            canvas = transform.GetComponentInParent<Canvas>();
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
            var activeCamera = canvas.renderMode switch
            {
                RenderMode.ScreenSpaceOverlay => null,
                RenderMode.ScreenSpaceCamera or RenderMode.WorldSpace => canvas.worldCamera,
                _ => throw new ArgumentOutOfRangeException()
            };
            var screenPoint = (activeCamera ?? activeCameraInScreenSpaceOverlay).WorldToScreenPoint(target.transform.position + offset);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, screenPoint, activeCamera, out var worldPoint);
            _rectTransform.position = worldPoint;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _tracker.Clear();
        }
    }
}
