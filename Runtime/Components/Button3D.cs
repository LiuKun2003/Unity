using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Collider))]
    public class Button3D : Selectable3D, IPointerClickHandler, ISubmitHandler
    {
        [Serializable] public class ButtonClickedEvent : UnityEvent {}
        
        [SerializeField] private ButtonClickedEvent onClick = new();

        protected Button3D() {}
        
        public ButtonClickedEvent OnClick
        {
            get => onClick;
            set => onClick = value;
        }
        
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button3D.OnClick", this);
            onClick.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed);
            StartCoroutine(OnFinishSubmit());
        }
        
        private IEnumerator OnFinishSubmit()
        {
            const float fadeTime = 0.1f;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(CurrentSelectionState);
        }
    }
}
