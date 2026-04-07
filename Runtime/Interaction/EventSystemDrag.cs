using UnityEngine;
using UnityEngine.EventSystems;

namespace LK.Runtime.Interaction
{
    public class EventSystemDrag : InputProviderBase, IDragHandler
    {
        [SerializeField] private bool reverseX;
        [SerializeField]  private bool reverseY;
        
        public void OnDrag(PointerEventData eventData)
        {
            var v = new Vector3(eventData.delta.x, eventData.delta.y, 0);
            if (reverseX) v.x *= -1;
            if (reverseY) v.y *= -1;
            OutDelta(v);
        }
    }
}
