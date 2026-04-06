namespace LK.Runtime.Fsm
{
    /// <summary>
    /// 表示一段状态机上下文。
    /// </summary>
    /// <typeparam name="TIndex">要使用的索引类型。</typeparam>
    public interface IContext<in TIndex>
    {
        /// <summary>
        /// 获取指定的资源
        /// </summary>
        /// <param name="index">资源的索引。</param>
        /// <typeparam name="TResource">资源的类型。</typeparam>
        /// <returns>指定索引的资源，没有则为 null。</returns>
        public TResource Get<TResource>(TIndex index);
    }
}
