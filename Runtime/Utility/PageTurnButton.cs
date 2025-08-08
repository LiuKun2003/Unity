using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LK.Runtime.Utility
{
    public class PageTurnButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        public enum TurnBehavior
        {
            None,
            Previous,
            Next,
            First,
            Trailer
        }
        
        [SerializeField] private MultiPage multiPage;
        [SerializeField] private TurnBehavior turningType = TurnBehavior.Next;

        public MultiPage MultiPage
        {
            get => multiPage;
            set
            {
                if(multiPage == value) return;
                multiPage.OnPageTurn.RemoveListener(CheckInteractable);
                multiPage = value;
                if (multiPage != null)
                {
                    multiPage.OnPageTurn.AddListener(CheckInteractable);
                    CheckInteractable(multiPage.CurrentPageIndex);
                }
                else
                {
                    interactable = false;
                }
            }
        }
        
        public TurnBehavior TurningType
        {
            get => turningType;
            set
            {
                if (turningType == value) return;
                turningType = value;
                if (multiPage != null)
                {
                    CheckInteractable(multiPage.CurrentPageIndex);
                }
                else
                {
                    interactable = false;
                }
            }
        }
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Turn();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Turn();
            
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            if (multiPage == null) return;
            multiPage.OnPageTurn.AddListener(CheckInteractable);
            CheckInteractable(multiPage.CurrentPageIndex);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (multiPage == null) return;
            multiPage.OnPageTurn.RemoveListener(CheckInteractable);
        }
        
        private void CheckInteractable(int pageIndex)
        {
            interactable = turningType switch
            {
                TurnBehavior.None => true,
                TurnBehavior.Previous => pageIndex > 0,
                TurnBehavior.Next => pageIndex < multiPage.PagesCount - 1,
                TurnBehavior.First => pageIndex != 0,
                TurnBehavior.Trailer => pageIndex != multiPage.PagesCount - 1,
                _ => false
            };
        }
        
        private void Turn()
        {
            if (!IsActive() || !IsInteractable() || multiPage == null)
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            
            switch (turningType)
            {
                case TurnBehavior.None:
                    break;
                case TurnBehavior.Previous:
                    multiPage.PreviousPage();
                    break;
                case TurnBehavior.Next:
                    multiPage.NextPage();
                    break;
                case TurnBehavior.First:
                    multiPage.FirstPage();
                    break;
                case TurnBehavior.Trailer:
                    multiPage.TrailerPage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
