using System;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Utilities
{
    [Serializable]
    public struct StateEvents : IEquatable<StateEvents>
    {
        [field: SerializeField] public UnityEvent Normal { get; set; }
        [field: SerializeField] public UnityEvent Highlighted { get; set; }
        [field: SerializeField] public UnityEvent Pressed { get; set; }
        [field: SerializeField] public UnityEvent Selected { get; set; }
        [field: SerializeField] public UnityEvent Disabled { get; set; }

        public bool Equals(StateEvents other)
        {
            return Equals(Normal, other.Normal) && 
                   Equals(Highlighted, other.Highlighted) && 
                   Equals(Pressed, other.Pressed) && 
                   Equals(Selected, other.Selected) && 
                   Equals(Disabled, other.Disabled);
        }

        public override bool Equals(object obj)
        {
            return obj is StateEvents other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Normal.GetHashCode();
                hash = hash * 23 + Highlighted.GetHashCode();
                hash = hash * 23 + Pressed.GetHashCode();
                hash = hash * 23 + Selected.GetHashCode();
                hash = hash * 23 + Disabled.GetHashCode();
                return hash;
            }
        }
    }
}
