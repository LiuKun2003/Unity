using System;
using UnityEngine;

namespace Utility
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly Func<bool> Condition;
        
        public ShowIfAttribute(Func<bool> condition)
        {
            Condition = condition;
        }
    }
}