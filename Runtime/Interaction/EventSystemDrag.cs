using UnityEngine.EventSystems;

namespace LK.Runtime.Interaction
{
    public class EventSystemDrag : InputProviderBase, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            OutDelta(eventData.delta);
        }
    }
}
