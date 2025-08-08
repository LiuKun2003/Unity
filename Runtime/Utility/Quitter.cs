using UnityEngine;

namespace LK.Runtime.Utility
{
    public class Quitter : MonoBehaviour
    {
        public void QuitImmediately()
        {
            QuitApplication();
        }
        
        public void QuitDelayed(float delay)
        {
            Executing.Delayed(QuitApplication, delay);
        }
        
        private static void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
