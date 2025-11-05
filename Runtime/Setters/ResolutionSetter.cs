using UnityEngine;

namespace LK.Runtime.Setters
{
    public class ResolutionSetter : MonoBehaviour, ISetter
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private FullScreenMode fullScreenMode;

        public void Apply()
        {
            Screen.SetResolution(width, height, fullScreenMode);
        }
    }
}
