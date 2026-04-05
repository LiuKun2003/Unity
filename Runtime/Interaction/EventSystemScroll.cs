using UnityEngine;
using UnityEngine.EventSystems;

namespace LK.Runtime.Interaction
{
    public class EventSystemScroll : InputProviderBase, IScrollHandler
    {
        [SerializeField] private Vector3 scrollScale = Vector3.one;
        
        public void OnScroll(PointerEventData eventData)
        {
            OutDelta(eventData.scrollDelta.y * scrollScale);
        }
    }
}
