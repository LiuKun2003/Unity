using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LK.Runtime.Utility
{
    public class SimpleOverrideButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Press();

            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }
    
        protected virtual void Click()
        {
        
        }
    
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            Click();
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}
