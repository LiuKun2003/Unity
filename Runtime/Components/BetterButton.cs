using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace LK.Runtime.Components
{
    [AddComponentMenu("UI/Effects/BetterButton", 83)]
    [RequireComponent(typeof(Selectable))]
    public class BetterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] [Range(0, 2)] private float hoverScaleMultiplier = 1.2f;
        [SerializeField] [Range(0, 2)] private float pressedScaleMultiplier = 1.1f;
        [SerializeField] [Range(0, 1)] private float animationDuration = 0.2f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    
        private Selectable _selectable;
        private Vector3 _originalScale;
        private bool _isHovered;
        private bool _isPressed;
        private bool _dirtyTarget;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_selectable.interactable) return;
            _isHovered = true;
            UpdateScale();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            UpdateScale();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if(!_selectable.interactable || eventData.button != PointerEventData.InputButton.Left) return;
            _isPressed = true;
            UpdateScale();
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            UpdateScale();
        }

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _originalScale = _selectable.transform.localScale;
        }

        private void OnEnable()
        {
            _selectable.transform.localScale = _originalScale;
        }

        private void OnDisable()
        {
            _dirtyTarget = false;
        }
        
        private void UpdateScale()
        {
            Vector3 to;
            if (_isPressed)
            {
                to = _originalScale * pressedScaleMultiplier;
            }
            else if (_isHovered)
            {
                to = _originalScale * hoverScaleMultiplier;
            }
            else
            {
                to = _originalScale;
            }
            _dirtyTarget = true;
            StartCoroutine(LocalScaleTo(to));
        }

        private IEnumerator LocalScaleTo(Vector3 to)
        {
            yield return null;
            _dirtyTarget = false;
            
            var from = _selectable.transform.localScale;
            if(from == to) yield break;
            
            float t = 0;
            while (t < 1f && !_dirtyTarget)
            {
                t += Time.deltaTime / animationDuration;
                _selectable.transform.localScale = Vector3.Lerp(from, to, animationCurve.Evaluate(t));
                yield return null;
            }

            if (t >= 1f)
            {
                _selectable.transform.localScale = to;
            }
        }
    }
}
