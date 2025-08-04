#if DOTWEEN
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [DisallowMultipleComponent]
    public class DotweenTriggerGroup : MonoBehaviour
    {
        private List<DotweenTrigger> _triggers;
        
        public void ApplyAllTriggers()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Apply();
            }
        }

        internal void Register(DotweenTrigger trigger)
        {
            if (!_triggers.Contains(trigger))
            {
                _triggers.Add(trigger);
            }
        }

        internal void Unregister(DotweenTrigger trigger)
        {
            if (_triggers.Contains(trigger))
            {
                _triggers.Remove(trigger);
            }
        }
    }
}
#endif
