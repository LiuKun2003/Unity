using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    public static class Executing
    {
        public static void Delay(Action method, float seconds)
        {
            if (seconds < 0)
            {
                throw new System.ArgumentException("seconds cannot be less than 0");
            }
#if UNITY_2023_2_OR_NEWER
            _ = ExecuteAfter(method, seconds);
#else
            StartCoroutine(ExecuteAfter(method, seconds));
#endif
        }
        
#if UNITY_2023_2_OR_NEWER
        private static async Awaitable ExecuteAfter(Action method, float seconds)
        {
            await Awaitable.WaitForSecondsAsync(seconds);
            method?.Invoke();
        }
#else
        private static IEnumerator ExecuteAfter(Action method, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            method?.Invoke();
        }
#endif  
    }
}
