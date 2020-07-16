
namespace SLG
{
    /// <summary>
    /// 状态
    /// </summary>
    public class StartLogin : State
    {
        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

            //Lua.instance.OnStart();
        }
        #endregion
    }
}