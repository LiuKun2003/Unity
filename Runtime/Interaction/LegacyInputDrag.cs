using UnityEngine;

namespace LK.Runtime.Interaction
{
    public class LegacyInputDrag : InputProviderBase
    {
        [SerializeField] private int pressKey;
        [SerializeField] private string mouseXAxis = "Mouse X";
        [SerializeField] private string mouseYAxis = "Mouse Y";

        private bool _isPressed;
        
        public void Update()
        {
            if (!_isPressed) return;
            OutDelta(GetMouseXYDelta());
            _isPressed = !Input.GetMouseButtonUp(pressKey);
        }

        private void OnMouseOver()
        {
            if (_isPressed) return;
            _isPressed = Input.GetMouseButtonDown(pressKey);
        }
        
        private Vector3 GetMouseXYDelta()
        {
            return new Vector3(Input.GetAxisRaw(mouseXAxis), Input.GetAxisRaw(mouseYAxis), 0f);
        }
    }
}
