using System;
using System.Buffers;
using System.Linq;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Collider))]
    public class Button3D : Selectable3D
    {
        [field: SerializeField]
        public UnityEvent OnClick { get; set; }

        private void OnMouseUpAsButton()
        {
            //if(!isActiveAndEnabled || !Interactable) return;
            
            OnClick?.Invoke();
        }
    }
}
