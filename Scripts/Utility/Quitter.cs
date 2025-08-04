using System.Collections;
using UnityEngine;

namespace Utility
{
    public class Quitter : MonoBehaviour
    {
        public void QuitImmediately()
        {
            QuitApplication();
        }

#if UNITY_2023_2_OR_NEWER
        public void QuitDelayed(float delay)
        {
            if (delay <= 0)
            {
                QuitApplication();
            }
            else
            {
                _ = QuitApplication(delay);
            }
        }
        
        private static async Awaitable QuitApplication(float delay)
        {
            await Awaitable.WaitForSecondsAsync(delay);
            QuitApplication();
        }
#else
        public void QuitDelayed(float delay)
        {
            if (delay <= 0)
            {
                QuitApplication();
            }
            else
            {
                StartCoroutine(QuitApplication(delay));
            }
        }
        
        private static IEnumerator QuitApplication(float delay)
        {
            var endTime = Time.time + delay;
            while (Time.time < endTime)
            {
                yield return null;
            }
            QuitApplication();
        }
#endif
        

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
