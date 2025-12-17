using System;
using UnityEngine;

namespace LK.Runtime.Interaction
{
    public class LegacyInputScroll : InputProviderBase
    {
        [SerializeField] private string scrollAxis = "Mouse ScrollWheel";
        [SerializeField] private Vector3 scrollScale = Vector3.one;

        private bool _isHover;
        
        private void Update()
        {
            if (!_isHover) return;
            OutDelta(scrollScale * Input.GetAxisRaw(scrollAxis));
        }

        public void OnMouseEnter()
        {
            _isHover = true;
        }
        
        public void OnMouseExit()
        {
            _isHover = false;
        }
    }
}
