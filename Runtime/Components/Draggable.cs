using UnityEngine;
using UnityEngine.EventSystems;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Draggable : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float smoothness = 0.1f;
        
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private bool _isDragging;
        private Vector2 _targetPosition;
        private Vector2 _currentVelocity;
        
        public float Smoothness
        {
            get => smoothness;
            set => smoothness = value;
        }

        protected override void Start()
        {
            base.Start();
            CacheCanvas();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _targetPosition = _rectTransform.anchoredPosition;
        }

        private void Update()
        {
            if (_isDragging)
            {
                _rectTransform.anchoredPosition = Vector2.SmoothDamp(_rectTransform.anchoredPosition, _targetPosition, ref _currentVelocity, smoothness);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_isDragging)
            {
                _canvasGroup.blocksRaycasts = true;
                _isDragging = false;
                _targetPosition = _rectTransform.anchoredPosition;
            }
        }

        protected override void OnCanvasGroupChanged()
        {
            base.OnCanvasGroupChanged();
            CacheCanvas();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_canvasGroup == null) return;
            _targetPosition = _rectTransform.anchoredPosition;
            _canvasGroup.blocksRaycasts = false;
            _currentVelocity = Vector2.zero;
            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_canvasGroup == null || _canvas == null) return;
            _targetPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_canvasGroup == null) return;
            _canvasGroup.blocksRaycasts = true;
            _isDragging = false;
            _targetPosition = _rectTransform.anchoredPosition;
        }

        private void CacheCanvas()
        {
            _canvas = GetComponentInParent<Canvas>();
        }
    }
}