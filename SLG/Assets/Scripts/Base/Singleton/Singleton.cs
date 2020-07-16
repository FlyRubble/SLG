
namespace SLG
{
	namespace Singleton
	{
		/// <summary>
		/// 单例
		/// </summary>
		public abstract class Singleton<T> where T : new()
		{
			#region Function
			/// <summary>
			/// 得到单例
			/// </summary>
			/// <value>The instance.</value>
			public static T instance
			{
				get
				{
					return Nested.g_instance;
				}
			}

            /// <summary>
            /// 清理数据
            /// </summary>
            public virtual void Clear() { }
            #endregion

            // 内部类
            sealed class Nested
			{
				#region Variable
				internal static readonly T g_instance = new T();
				#endregion
				
				#region Function
				static Nested() { }
				#endregion
			}
		}
	}
}