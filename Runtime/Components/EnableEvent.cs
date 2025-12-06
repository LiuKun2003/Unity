using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Components
{
    /// <summary>
    /// 在游戏对象启用/禁用时触发事件的组件
    /// </summary>
    public class EnableEvent : MonoBehaviour
    {
        /// <summary>
        /// 当游戏对象启用时触发的事件
        /// </summary>
        public UnityEvent onEnable;
        
        /// <summary>
        /// 当游戏对象禁用时触发的事件
        /// </summary>
        public UnityEvent onDisable;

        private void OnEnable()
        {
            onEnable?.Invoke();
        }
        
        private void OnDisable()
        {
            onDisable?.Invoke();
        }
    }
}
