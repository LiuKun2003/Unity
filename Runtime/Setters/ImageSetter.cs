using UnityEngine;
using UnityEngine.UI;

namespace LK.Runtime.Setters
{
    public class ImageSetter : MonoBehaviour, ISetter
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite sprite;
    
        public void Apply()
        {
            image.sprite = sprite;
        }
    }
}
