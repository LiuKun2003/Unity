using System;
using System.Buffers;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class Selectable3D : MonoBehaviour
    {
        protected enum State
        {
            Normal,
            Highlighted,
            Pressed,
            Disabled,
        }
    
        public enum Transition
        {
            None,
            MaterialsSwap,
            Event,
        }
    
        private bool _isMouseDown;
        private bool _isMouseInside;
    
        protected State CurrentState
        {
            get
            {
                if (!interactable)
                    return State.Disabled;
            
                if (_isMouseDown)
                    return State.Pressed;
            
                if (_isMouseInside)
                    return State.Highlighted;
            
                return State.Normal;
            }
        }
    
        [SerializeField] private bool interactable = true;
        [SerializeField] private Transition transition = Transition.MaterialsSwap;
    
        [SerializeField] private Transform root;
        [SerializeField] private bool ignoreChildren;
        [SerializeField] private Material[] normalMaterials;
        [SerializeField] private Material[] highlightedMaterials;
        [SerializeField] private Material[] pressedMaterials;
        [SerializeField] private Material[] disabledMaterials;

        [SerializeField] private UnityEvent normal;
        [SerializeField] private UnityEvent highlighted;
        [SerializeField] private UnityEvent pressed;
        [SerializeField] private UnityEvent disabled;
    
        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
            
                DoStateTransition(CurrentState);
            }
        }

        protected virtual void Reset()
        {
            root = GetComponent<Transform>();
        }

        protected virtual void OnEnable()
        {
            _isMouseDown = false;
        
            DoStateTransition(CurrentState);
        }

        protected virtual void OnDisable()
        {
            _isMouseDown = false;
            _isMouseInside = false;
        }

        protected virtual void OnMouseDown()
        {
            _isMouseDown = true;
            EvaluateAndTransition();
        }
    
        protected virtual void OnMouseUp()
        {
            _isMouseDown = false;
            EvaluateAndTransition();
        }
    
        protected virtual void OnMouseEnter()
        {
            _isMouseInside = true;
            EvaluateAndTransition();
        }
    
        protected virtual void OnMouseExit()
        {
            _isMouseInside = false;
            EvaluateAndTransition();
        }

        private void EvaluateAndTransition()
        {
            if (!isActiveAndEnabled || !interactable)
                return;
        
            DoStateTransition(CurrentState);
        }

        protected void DoStateTransition(State state)
        {
            if (!gameObject.activeInHierarchy)
                return;

            var materials = GetMaterialsContainer();
            int materialsCount;

            UnityEvent unityEvent;
            
            switch (state)
            {
                case State.Normal:
                    Array.Copy(normalMaterials, materials, normalMaterials.Length);
                    materialsCount = normalMaterials.Length;
                    unityEvent = normal;
                    break;
                case State.Highlighted:
                    Array.Copy(highlightedMaterials, materials, highlightedMaterials.Length);
                    materialsCount = highlightedMaterials.Length;
                    unityEvent = highlighted;
                    break;
                case State.Pressed:
                    Array.Copy(pressedMaterials, materials, pressedMaterials.Length);
                    materialsCount = pressedMaterials.Length;
                    unityEvent = pressed;
                    break;
                case State.Disabled:
                    Array.Copy(disabledMaterials, materials, disabledMaterials.Length);
                    materialsCount = disabledMaterials.Length;
                    unityEvent = disabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        
            switch (transition)
            {
                case Transition.None:
                    break;
                case Transition.MaterialsSwap:
                    DoMaterialsSwap(materials, materialsCount);
                    ArrayPool<Material>.Shared.Return(materials);
                    break;
                case Transition.Event:
                    unityEvent.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Material[] GetMaterialsContainer()
        {
            var minimumLength = Math.Max(normalMaterials.Length, highlightedMaterials.Length);
            minimumLength = Math.Max(minimumLength, pressedMaterials.Length);
            minimumLength = Math.Max(minimumLength, disabledMaterials.Length);
            return ArrayPool<Material>.Shared.Rent(minimumLength);
        }

        private void DoMaterialsSwap(Material[] materials, int materialsCount)
        {
            if(root == null) return;
            var meshRenderers = ignoreChildren ? root.GetComponents<MeshRenderer>() : root.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                var original = meshRenderer.sharedMaterials;
                var toRemove = normalMaterials.Union(highlightedMaterials).Union(pressedMaterials).Union(disabledMaterials);
                var newMaterials = original.Except(toRemove).Union(materials[..materialsCount]).ToArray();
                meshRenderer.materials = newMaterials;
            }
        }
    }
}
