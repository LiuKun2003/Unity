using UnityEngine;

namespace LK.Runtime.Fsm
{
    /// <summary>
    /// 抽象状态基类。
    /// </summary>
    public abstract class BaseState : IState
    {
        private readonly StateMachine parentMachine;
    
        /// <summary>上下文对象，用于获取外部组件或数据。</summary>
        protected IContext<string> Context;

    
        /// <summary>
        /// 初始化状态基类。
        /// </summary>
        /// <param name="parentMachine">持有该状态的状态机。</param>
        /// <param name="context">外部上下文环境。</param>
        protected BaseState(StateMachine parentMachine, IContext<string> context)
        {
            this.parentMachine = parentMachine;
            Context = context;
        }
    
        public virtual void OnEnter()
        {
#if UNITY_EDITOR
            Debug.Log($"{GetType().Name}: State Entered");
#endif
        }

        public virtual void OnUpdate()
        {
#if UNITY_EDITOR
            Debug.Log($"{GetType().Name}: State Update");
#endif
        }

        public virtual void OnExit()
        {
#if UNITY_EDITOR
            Debug.Log($"{GetType().Name}: State Exited");
#endif
        }

        /// <summary>
        /// 切换到新状态。
        /// </summary>
        /// <param name="newState">要跳转到的新状态。</param>
        protected void ChangeState(IState newState)
        {
            parentMachine.ChangeState(newState);
        }
    }
}
