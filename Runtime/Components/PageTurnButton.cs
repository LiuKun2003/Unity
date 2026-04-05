using System;
using LK.Runtime.Utilities;
using UnityEngine;

namespace LK.Runtime.Components
{
    public class PageTurnButton : SimpleOverrideButton
    {
        public enum TurnBehavior
        {
            None,
            Previous,
            Next,
            First,
            Trailer,
            Specific,
        }
        
        [SerializeField] private MultiPage multiPage;
        [SerializeField] private TurnBehavior turningType = TurnBehavior.Next;
        [SerializeField] private int specific;
        
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
                TurnBehavior.Next => pageIndex < multiPage.Pages.Count - 1,
                TurnBehavior.First => pageIndex != 0,
                TurnBehavior.Trailer => pageIndex != multiPage.Pages.Count - 1,
                TurnBehavior.Specific => pageIndex != specific,
                _ => false
            };
        }
        
        protected override void Click()
        {
            if (multiPage == null)
            {
                return;
            }

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
                case TurnBehavior.Specific:
                    multiPage.SetCurrentPage(specific);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
