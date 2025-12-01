using System;
using System.Buffers;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Components
{
    [RequireComponent(typeof(Collider))]
    public class Button3D : MonoBehaviour
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
        }
    
        private bool _isMouseDown;
        private bool _isMouseInside;
    
        private State CurrentState
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
    
        public UnityEvent onClick;
    
        public bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
            
                DoStateTransition(CurrentState);
            }
        }

        private void Reset()
        {
            root = GetComponent<Transform>();
        }

        private void OnEnable()
        {
            _isMouseDown = false;
        
            DoStateTransition(CurrentState);
        }

        private void OnDisable()
        {
            _isMouseDown = false;
            _isMouseInside = false;
        }

        private void OnMouseDown()
        {
            _isMouseDown = true;
            EvaluateAndTransition();
        }
    
        private void OnMouseUp()
        {
            _isMouseDown = false;
            EvaluateAndTransition();
        }
    
        private void OnMouseEnter()
        {
            _isMouseInside = true;
            EvaluateAndTransition();
        }
    
        private void OnMouseExit()
        {
            _isMouseInside = false;
            EvaluateAndTransition();
        }

        private void OnMouseUpAsButton()
        {
            onClick.Invoke();
        }

        private void EvaluateAndTransition()
        {
            if (!isActiveAndEnabled || !interactable)
                return;
        
            DoStateTransition(CurrentState);
        }

        private void DoStateTransition(State state)
        {
            if (!gameObject.activeInHierarchy)
                return;

            var materials = GetMaterialsContainer();
            int materialsCount;
            switch (state)
            {
                case State.Normal:
                    Array.Copy(normalMaterials, materials, normalMaterials.Length);
                    materialsCount = normalMaterials.Length;
                    break;
                case State.Highlighted:
                    Array.Copy(highlightedMaterials, materials, highlightedMaterials.Length);
                    materialsCount = highlightedMaterials.Length;
                    break;
                case State.Pressed:
                    Array.Copy(pressedMaterials, materials, pressedMaterials.Length);
                    materialsCount = pressedMaterials.Length;
                    break;
                case State.Disabled:
                    Array.Copy(disabledMaterials, materials, disabledMaterials.Length);
                    materialsCount = disabledMaterials.Length;
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
