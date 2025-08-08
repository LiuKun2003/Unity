using System;
using UnityEngine;

namespace LK.Runtime.Utility
{
    public static class Executing
    {
        public static void Delayed(Action method, float seconds, Action onEnd = null)
        {
            if (seconds < 0)
            {
                throw new System.ArgumentException("seconds cannot be less than 0");
            }
#if UNITY_2023_2_OR_NEWER
            _ = LocalDelayed(method, seconds, onEnd);
#else
            StartCoroutine(LocalDelayed(method, seconds, onEnd));
#endif
        }

        public static void Continuous(Action method, float seconds, Action onEnd = null)
        {
            if (seconds < 0)
            {
                throw new System.ArgumentException("seconds cannot be less than 0");
            }
#if UNITY_2023_2_OR_NEWER
            _ = LocalContinuous(method, seconds, onEnd);
#else
            StartCoroutine(LocalContinuous(method, seconds, onEnd));
#endif
        }
        
#if UNITY_2023_2_OR_NEWER
        private static async Awaitable LocalDelayed(Action method, float seconds, Action callback = null)
        {
            await Awaitable.WaitForSecondsAsync(seconds);
            method?.Invoke();
            callback?.Invoke();
        }

        private static async Awaitable LocalContinuous(Action method, float seconds, Action callback = null)
        {
            var time = 0f;
            while (time <= seconds)
            {
                await Awaitable.NextFrameAsync();
                time += Time.deltaTime;
                method?.Invoke();
            }
            callback?.Invoke();
        }
#else
        private static IEnumerator LocalDelayed(Action method, float seconds, Action callback = null)
        {
            yield return new WaitForSeconds(seconds);
            method?.Invoke();
            callback?.Invoke();
        }
        
        private static IEnumerator LocalContinuous(Action method, float seconds, Action callback = null)
        {
            var time = 0f;
            while (time <= seconds)
            {
                yield return null;
                time += Time.deltaTime;
                method?.Invoke();
            }
            callback?.Invoke();
        }
#endif  
    }
}
