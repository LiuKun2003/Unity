using System;
using UnityEngine;

namespace LK.Runtime.Utilities
{
    [Serializable]
    public struct MaterialsBlock : IEquatable<MaterialsBlock>
    {
        [field: SerializeField] public Material[] NormalMaterials { get; set; }
        [field: SerializeField] public Material[] HighlightedMaterials { get; set; }
        [field: SerializeField] public Material[] PressedMaterials { get; set; }
        [field: SerializeField] public Material[] SelectedMaterials { get; set; }
        [field: SerializeField] public Material[] DisabledMaterials { get; set; }

        public bool Equals(MaterialsBlock other)
        {
            return Equals(NormalMaterials, other.NormalMaterials) &&
                   Equals(HighlightedMaterials, other.HighlightedMaterials) &&
                   Equals(PressedMaterials, other.PressedMaterials) &&
                   Equals(SelectedMaterials, other.SelectedMaterials) &&
                   Equals(DisabledMaterials, other.DisabledMaterials);
        }

        public override bool Equals(object obj)
        {
            return obj is MaterialsBlock other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NormalMaterials, HighlightedMaterials, PressedMaterials, SelectedMaterials, DisabledMaterials);
        }
    }
}
