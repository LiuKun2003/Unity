using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LK.Runtime.Components
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }
        
        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(SceneManager.LoadSceneAsync(sceneName)));
        }

        public void LoadSceneAsync(int buildIndex)
        {
            StartCoroutine(LoadSceneCoroutine(SceneManager.LoadSceneAsync(buildIndex)));
        }

        private static IEnumerator LoadSceneCoroutine(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }
}
