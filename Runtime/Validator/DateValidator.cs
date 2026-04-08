using System;
using System.Collections;
using LK.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace LK.Runtime.Validator
{
    [CreateAssetMenu(fileName = "New Date Validator", menuName = "ScriptableObjects/Validator/Date", order = 1)]
    public class DateValidator : Validator
    {
        [SerializeField] private SerializableDateTime expirationDate;
        [SerializeField] private string lastDateTimeKey = "LAST_DATETIME_KEY";
        
        
        public override bool Result { get; protected set; }
        
        public override IEnumerator Verify()
        {
            using var webRequest = UnityWebRequest.Get("https://quan.suning.com/getSysTime.do");
            webRequest.timeout = 5;
            yield return webRequest.SendWebRequest();

            var now = DateTime.Now;
            
#if UNITY_EDITOR
            if (PlayerPrefs.HasKey(lastDateTimeKey))
            {
                var lastDateTime = DateTime.Parse(PlayerPrefs.GetString(lastDateTimeKey));
                if (lastDateTime > now) yield break;
            }
#endif
            
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // 返回格式: {"sysTime2":"2023-10-27 10:00:00","sysTime1":"20231027100000"}
                var json = webRequest.downloadHandler.text;
                // 简单截取 sysTime2 的值
                var datetimeStr = json.Substring(json.IndexOf("sysTime2\":\"", StringComparison.Ordinal) + 11, 19);
                now = DateTime.Parse(datetimeStr);
            }
        
            Result = now < expirationDate;
        }


        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(lastDateTimeKey)) return;
            lastDateTimeKey = "LAST_DATETIME_KEY";
            Debug.LogError("lastDateTimeKey can't be null or empty!");
        }
    }
}
