using UnityEngine;

namespace LK.Runtime.Utilities
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string[] ConditionFieldNames;
        
        public ShowIfAttribute(string[] conditionFieldNames)
        {
            this.ConditionFieldNames = conditionFieldNames;
        }
    }
}