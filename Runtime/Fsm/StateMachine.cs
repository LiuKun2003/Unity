namespace LK.Runtime.Fsm
{
    /// <summary>
    /// 状态机类，负责管理当前活跃状态的生命周期并处理状态切换。
    /// </summary>
    public class StateMachine : IState
    {
        private IState currentState;

        /// <summary>
        /// 初始化状态机的新实例。
        /// </summary>
        public StateMachine() { }
    
        /// <summary>
        /// 初始化状态机的新实例并设置初始状态。
        /// </summary>
        /// <param name="state">初始状态。</param>
        public StateMachine(IState state) => currentState = state;

        /// <summary>
        /// 进入状态机逻辑。
        /// </summary>
        public void OnEnter()
        {
            currentState?.OnEnter();
        }

        /// <summary>
        /// 状态机的逻辑更新。
        /// </summary>
        public void OnUpdate()
        {
            currentState?.OnUpdate();
        }

        /// <summary>
        /// 退出状态机逻辑。
        /// </summary>
        public void OnExit()
        {
            currentState?.OnExit();
        }
    
        /// <summary>
        /// 切换到新的状态。
        /// </summary>
        /// <param name="newState">要切换到的目标状态实例。</param>
        public void ChangeState(IState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }
    }
}
