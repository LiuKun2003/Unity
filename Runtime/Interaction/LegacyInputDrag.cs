using UnityEngine;

namespace LK.Runtime.Interaction
{
    public class LegacyInputDrag : InputProviderBase
    {
        [SerializeField] private int pressKey;
        [SerializeField] private string mouseXAxis = "Mouse X";
        [SerializeField] private bool reverseX;
        [SerializeField] private string mouseYAxis = "Mouse Y";
        [SerializeField] private bool reverseY;

        private bool _isPressed;
        
        private void Update()
        {
            if (!_isPressed) return;
            OutDelta(GetMouseXYDelta());
            _isPressed = !Input.GetMouseButtonUp(pressKey);
        }

        private void OnMouseOver()
        {
            if (!enabled || _isPressed) return;
            _isPressed = Input.GetMouseButtonDown(pressKey);
        }
        
        private void OnDisable()
        {
            _isPressed = false;
        }
        
        private Vector3 GetMouseXYDelta()
        {
            var x = Input.GetAxisRaw(mouseXAxis) * (reverseX ? -1f : 1f);
            var y = Input.GetAxisRaw(mouseYAxis) * (reverseY ? -1f : 1f);
            return new Vector3(x, y, 0f);
        }
    }
}
