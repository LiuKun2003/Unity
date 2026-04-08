using System;
using UnityEngine;

namespace LK.Runtime.Utilities
{
    [Serializable]
    public struct SerializableDateTime
    {
        [SerializeField] private long ticks;
    
        public DateTime DateTime
        {
            get => new(ticks);
            set => ticks = value.Ticks;
        }
    
        public static implicit operator DateTime(SerializableDateTime sdt) => sdt.DateTime;
        public static implicit operator SerializableDateTime(DateTime dt) => new() { DateTime = dt };
    }
}