using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LK.Runtime.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class Selectable3D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public enum TransitionMode
        {
            None,
            MaterialsSwap,
            Event,
        }
        
        protected enum SelectionState
        {
            Normal,
            Highlighted,
            Pressed,
            Selected,
            Disabled,
        }
        
        [SerializeField] private bool interactable = true;
        [FormerlySerializedAs("transition")] 
        [SerializeField] 
        private TransitionMode transitionMode = TransitionMode.MaterialsSwap;
        
        [SerializeField] private Transform targetRenderer;
        [SerializeField] private bool ignoreChildren;
        [SerializeField] private MaterialsBlock  materialsBlock;
        
        [SerializeField] private StateEvents stateEvents;
        
        protected static Selectable3D[] Selectables = new Selectable3D[10];
        protected static int SelectableCount;
        protected int CurrentIndex = -1; 
        private bool _enableCalled;
        
        private bool IsPointerInside { get; set; }
        private bool IsPointerDown { get; set; }
        private bool HasSelection { get; set; }
        
        public static Selectable3D[] AllSelectablesArray
        {
            get
            {
                var temp = new Selectable3D[SelectableCount];
                Array.Copy(Selectables, temp, SelectableCount);
                return temp;
            }
        }
        
        public static int AllSelectableCount => SelectableCount;
        
        public TransitionMode Transition
        {
            get => transitionMode;
            set
            {
                if (SetStruct(ref transitionMode, value)) OnSetProperty(); 
            }
        }

        public Transform TargetRenderer
        {
            get => targetRenderer;
            set
            {
                if (!SetPropertyUtility.SetClass(ref targetRenderer, value)) return;
                InstantClearState();
                OnSetProperty();
            }
        }

        public bool IgnoreChildren
        {
            get => ignoreChildren;
            set
            {
                if (!SetPropertyUtility.SetStruct(ref ignoreChildren, value)) return;
                InstantClearState();
                OnSetProperty();
            }
        }
        
        public MaterialsBlock MaterialsBlock
        {
            get => materialsBlock;
            set
            {
                if (SetPropertyUtility.SetStruct(ref materialsBlock, value)) OnSetProperty();
            }
        }
        
        public StateEvents StateEvents
        {
            get => stateEvents;
            set
            {
                if(SetPropertyUtility.SetStruct(ref stateEvents, value)) OnSetProperty();
            }
        }

        public bool Interactable
        {
            get => interactable;
            set
            {
                if (!SetPropertyUtility.SetStruct(ref interactable, value)) return;
                if (!interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                    EventSystem.current.SetSelectedGameObject(null);
                OnSetProperty();
            }
        }
        
        protected SelectionState CurrentSelectionState
        {
            get
            {
                if (!IsInteractable())
                    return SelectionState.Disabled;
                if (IsPointerDown)
                    return SelectionState.Pressed;
                if (HasSelection)
                    return SelectionState.Selected;
                if (IsPointerInside)
                    return SelectionState.Highlighted;
                return SelectionState.Normal;
            }
        }
        
        public static int AllSelectablesNoAlloc(Selectable3D[] selectables)
        {
            var copyCount = selectables.Length < SelectableCount ? selectables.Length : SelectableCount;

            Array.Copy(Selectable3D.Selectables, selectables, copyCount);

            return copyCount;
        }
        
        public virtual bool IsInteractable()
        {
            return interactable;
        }
        
        public virtual void Select()
        {
            if (EventSystem.current == null || EventSystem.current.alreadySelecting)
                return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            if (IsInteractable()  && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            IsPointerDown = true;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            IsPointerDown = false;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerInside = true;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            IsPointerInside = false;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            HasSelection = true;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            HasSelection = false;
            EvaluateAndTransitionToSelectionState();
        }
        
        protected Selectable3D()
        {}
        
        protected virtual void OnValidate()
        {
            if (!IsActive()) return;
            if (!interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);

            DoMaterialsSwap(Array.Empty<Material>());

            DoStateTransition(CurrentSelectionState);
        }
        
        protected virtual void Reset()
        {
            targetRenderer = GetComponent<Transform>();
        }
        
        protected void OnEnable()
        {
            if (_enableCalled)
                return;

            if (SelectableCount == Selectables.Length)
            {
                var temp = new Selectable3D[Selectables.Length * 2];
                Array.Copy(Selectables, temp, Selectables.Length);
                Selectables = temp;
            }

            if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                HasSelection = true;
            }

            CurrentIndex = SelectableCount;
            Selectables[CurrentIndex] = this;
            SelectableCount++;
            IsPointerDown = false;
            DoStateTransition(CurrentSelectionState);

            _enableCalled = true;
        }
        
        protected virtual void OnDisable()
        {
            //Check to avoid multiple OnDisable() calls for each selectable
            if (!_enableCalled)
                return;

            SelectableCount--;

            // Update the last elements index to be this index
            Selectables[SelectableCount].CurrentIndex = CurrentIndex;

            // Swap the last element and this element
            Selectables[CurrentIndex] = Selectables[SelectableCount];

            // null out last element.
            Selectables[SelectableCount] = null;

            InstantClearState();

            _enableCalled = false;
        }
        
        protected virtual void InstantClearState()
        {
            IsPointerInside = false;
            IsPointerDown = false;
            HasSelection = false;

            switch (Transition)
            {
                
                case TransitionMode.MaterialsSwap:
                    DoMaterialsSwap(Array.Empty<Material>());
                    break;
                case TransitionMode.None:
                case TransitionMode.Event:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Transition), Transition, null);
            }
        }
        
        protected void DoStateTransition(SelectionState state)
        {
            if (!gameObject.activeInHierarchy)
                return;

            Material[] materials;

            UnityEvent unityEvent;
            
            switch (state)
            {
                case SelectionState.Normal:
                    materials = materialsBlock.NormalMaterials;
                    unityEvent = stateEvents.Normal;
                    break;
                case SelectionState.Highlighted:
                    materials = materialsBlock.HighlightedMaterials;
                    unityEvent = stateEvents.Highlighted;
                    break;
                case SelectionState.Pressed:
                    materials = materialsBlock.PressedMaterials;
                    unityEvent = stateEvents.Pressed;
                    break;
                case SelectionState.Selected:
                    materials  = materialsBlock.SelectedMaterials;
                    unityEvent = stateEvents.Selected;
                    break;
                case SelectionState.Disabled:
                    materials = materialsBlock.DisabledMaterials;
                    unityEvent = stateEvents.Disabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        
            switch (Transition)
            {
                case TransitionMode.None:
                    break;
                case TransitionMode.MaterialsSwap:
                    DoMaterialsSwap(materials ?? Array.Empty<Material>());
                    break;
                case TransitionMode.Event:
                    unityEvent.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }
        
        protected bool IsHighlighted()
        {
            if (!IsActive() || !IsInteractable())
                return false;
            return IsPointerInside && !IsPointerDown && !HasSelection;
        }
        
        protected bool IsPressed()
        {
            if (!IsActive() || !IsInteractable())
                return false;
            return IsPointerDown;
        }
        
        private void EvaluateAndTransitionToSelectionState()
        {
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(CurrentSelectionState);
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus && IsPressed())
            {
                InstantClearState();
            }
        }
        
        private void OnSetProperty()
        {
            DoStateTransition(CurrentSelectionState);
        }
        
        private Material[] GetMaterialsContainer()
        {
            var minimumLength = Math.Max(materialsBlock.NormalMaterials.Length, materialsBlock.HighlightedMaterials.Length);
            minimumLength = Math.Max(minimumLength, materialsBlock.PressedMaterials.Length);
            minimumLength = Math.Max(minimumLength, materialsBlock.DisabledMaterials.Length);
            minimumLength = Math.Max(minimumLength, materialsBlock.SelectedMaterials.Length);
            return ArrayPool<Material>.Shared.Rent(minimumLength);
        }

        private void DoMaterialsSwap(Material[] materials)
        {
            var root = targetRenderer;
            if(root == null) return;
            var meshRenderers = ignoreChildren ? root.GetComponents<MeshRenderer>() : root.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.materials = materials;
            }
        }
        
        private static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }

        private static bool SetReference<T>(ref T currentValue, T newValue) where T : class
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return false;
            
            currentValue = newValue;
            return true;
        }
    }
}
