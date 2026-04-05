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

        private void OnMouseEnter()
        {
            if(!enabled) return;
            _isHover = true;
        }
        
        private void OnMouseExit()
        {
            _isHover = false;
        }
        
        private void OnDisable()
        {
            _isHover = false;
        }
    }
}
