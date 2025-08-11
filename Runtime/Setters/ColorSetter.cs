using System;
using UnityEngine;
using UnityEngine.UI;

namespace LK.Runtime.Setters
{
    public class ColorSetter : MonoBehaviour, ISetter
    {
        [SerializeField] private Graphic graphic;
        [SerializeField] private Color color = Color.white;
        public void Apply()
        {
            if (graphic == null)
            {
                throw new ArgumentNullException(nameof(graphic));
            }
            
            graphic.color = color;
        }
    }
}
