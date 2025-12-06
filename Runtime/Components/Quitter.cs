using System.Collections;
using UnityEngine;

namespace LK.Runtime.Components
{
    public class Quitter : MonoBehaviour
    {
        public void QuitImmediately()
        {
            QuitApplication();
        }
        
        public void QuitDelayed(float delay)
        {
            StartCoroutine(Delayed(delay));
        }

        private static IEnumerator Delayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            QuitApplication();
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
