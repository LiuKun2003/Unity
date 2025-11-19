using UnityEngine;

namespace LK.Runtime.Components
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // 静态变量用于存储单例实例
        private static T _instance;

        // 属性用于获取单例实例
        public static T Instance
        {
            get
            {
                // 如果实例不存在，则查找场景中已存在的实例
                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<T>();

                // 如果仍然找不到，则创建一个新的实例
                if (_instance != null) return _instance;
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString();

                // 设置游戏对象为DontDestroyOnLoad，使其在场景切换时不被销毁
                DontDestroyOnLoad(singletonObject);

                return _instance;
            }
        }

        // Awake方法用于在对象初始化时设置单例实例
        protected virtual void Awake()
        {
            // 如果已经存在实例且不是当前对象，则销毁当前对象
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }

        // OnDestroy方法用于在对象销毁时清理单例引用
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}