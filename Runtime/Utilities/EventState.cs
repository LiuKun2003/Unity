using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct EventState : IEquatable<EventState>
{
    [field: SerializeField] public UnityEvent Normal { get; set; }
    [field: SerializeField] public UnityEvent Highlighted { get; set; }
    [field: SerializeField] public UnityEvent Pressed { get; set; }
    [field: SerializeField] public UnityEvent Selected { get; set; }
    [field: SerializeField] public UnityEvent Disabled { get; set; }

    public bool Equals(EventState other)
    {
        return Equals(Normal, other.Normal) && 
               Equals(Highlighted, other.Highlighted) && 
               Equals(Pressed, other.Pressed) && 
               Equals(Selected, other.Selected) && 
               Equals(Disabled, other.Disabled);
    }

    public override bool Equals(object obj)
    {
        return obj is EventState other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Normal, Highlighted, Pressed, Selected, Disabled);
    }
}
