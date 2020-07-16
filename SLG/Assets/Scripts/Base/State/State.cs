namespace SLG
{
    /// <summary>
    /// 状态
    /// </summary>
    public abstract class State
    {
        #region Variable
        /// <summary>
        /// 参数
        /// </summary>
        protected Param m_param;
        #endregion

        #region Variable
        /// <summary>
        /// 得到类型名
        /// </summary>
        public virtual string getName
        {
            get
            {
                return this.GetType().Name;
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnEnter(Param param = null)
        {
            Param.Destroy(m_param);
            m_param = param ?? Param.Create();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// 退出状态
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnExit()
        {
        }
        #endregion
    }
}