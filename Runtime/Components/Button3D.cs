using System;
using System.Buffers;
using System.Linq;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LK.Runtime.Components
{
    public class Button3D : Selectable3D, IPointerClickHandler
    {
        [field: SerializeField]
        public UnityEvent OnClick { get; set; }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;
            
            UISystemProfilerApi.AddMarker("Button3D.OnClick", this);
            OnClick?.Invoke();
        }
    }
}
