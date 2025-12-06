using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LK.Runtime.Utilities
{
    [Serializable]
    public struct RangedFloat
    {
        [SerializeField] private float lower;
        [SerializeField] private float upper;

        public float Lower
        {
            get => lower;
            set
            {
                CheckValidity(value, upper);
                lower = value;
            }
        }
        
        public float Upper
        {
            get => upper;
            set
            {
                CheckValidity(lower, value);
                upper = value;
            }
        }
        
        public RangedFloat(float value1, float value2)
        {
            if (value1 > value2)
            {
                upper = value1;
                lower = value2;
            }
            else
            {
                lower = value1;
                upper = value2;
            }
        }

        public void Set(float value1, float value2)
        {
            if (value1.CompareTo(value2) > 0)
            {
                upper = value1;
                lower = value2;
            }
            else
            {
                lower = value1;
                upper = value2;
            }
        }

        public float Clamp(float value)
        {
            var result = value;
            
            if (value < lower)
            {
                result = lower;
            }
            else if (value > upper)
            {
                result = upper;
            }
            
            return result;
        }
        
        private static void CheckValidity(float lower, float upper)
        {
            if (lower > upper)
            {
                throw new ArgumentException($"The lower limit of the range cannot be greater than the upper limit.");
            }
        }
    }
}
