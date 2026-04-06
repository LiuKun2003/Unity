namespace LK.Runtime.Fsm
{
    /// <summary>
    /// 表示一个状态。
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 当进入该状态时执行的逻辑。
        /// </summary>
        public void OnEnter();
    
        /// <summary>
        /// 在该状态运行期间，每次轮询调用的逻辑。
        /// </summary>
        public void OnUpdate();
    
        /// <summary>
        /// 当离开该状态时执行的清理逻辑。
        /// </summary>
        public void OnExit();
    }
}
