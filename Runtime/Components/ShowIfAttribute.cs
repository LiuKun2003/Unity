using UnityEngine;

namespace LK.Runtime.Components
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