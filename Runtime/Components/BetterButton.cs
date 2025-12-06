using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if !UNITY_2023_2_OR_NEWER
using System.Collections;
#endif 

namespace LK.Runtime.Components
{
    [AddComponentMenu("UI/Effects/BetterButton", 83)]
    [RequireComponent(typeof(Selectable))]
    public class BetterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] [Range(1, 2)] private float hoverScaleMultiplier = 1.2f;
        [SerializeField] [Range(0, 1)] private float animationDuration = 0.2f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    
        private Selectable _selectable;
        private Vector3 _originalScale;
        private bool _isHovered;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_selectable.interactable) return;
            _isHovered = true;
            BlowUp();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            Restore();
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
            _isHovered = false;
        }
        
#if UNITY_2023_2_OR_NEWER
        private void BlowUp()
        {
            _ = ScaleTo(_originalScale * hoverScaleMultiplier, true);
        }
        
        private void Restore()
        {
            _ = ScaleTo(_originalScale, false);
        }
        
        private async Awaitable ScaleTo(Vector3 to, bool hoverCheck)
        {
            var from = _selectable.transform.localScale;
            if(from == to) return;
            
            float t = 0;
            while (t < 1f && hoverCheck == _isHovered)
            {
                t += Time.deltaTime / animationDuration;
                _selectable.transform.localScale = Vector3.Lerp(from, to, animationCurve.Evaluate(t));
                await Awaitable.NextFrameAsync();
            }

            if (t >= 1f)
            {
                _selectable.transform.localScale = to;
            }
        }
#else
        private void BlowUp()
        {
            StartCoroutine(ScaleTo(_originalScale * hoverScaleMultiplier, true));
        }

        private void Restore()
        {
            StartCoroutine(ScaleTo(_originalScale, false));
        }

        private IEnumerator ScaleTo(Vector3 to, bool hoverCheck)
        {
            var from = _selectable.transform.localScale;
            if(from == to) yield break;

            float t = 0;
            while (t < 1f && hoverCheck == _isHovered)
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
#endif
        
    }
}
