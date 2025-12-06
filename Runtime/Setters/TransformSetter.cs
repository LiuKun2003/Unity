using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LK.Runtime.Setters
{
    public class TransformSetter : MonoBehaviour, ISetter
    {
        [SerializeField] private Transform root;
        [SerializeField] private Transform target;
        [SerializeField] private bool applyPosition;
        [SerializeField] private bool applyRotation;
        [SerializeField] private bool applyScale;
        public void Apply()
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (applyPosition)
            {
                root.position = target.position;
            }

            if (applyRotation)
            {
                root.rotation = target.rotation;
            }

            if (applyScale)
            {
                root.localScale = target.lossyScale;
            }
        }
    }
}
