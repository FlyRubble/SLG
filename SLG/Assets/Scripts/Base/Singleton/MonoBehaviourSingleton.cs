using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLG
{
    namespace Singleton
    {
        /// <summary>
        /// 单例
        /// </summary>
        public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour, new()
        {
            /// <summary>
            /// 单例对象
            /// </summary>
            private static T g_instance = null;

            #region Function
            /// <summary>
            /// 得到单例
            /// </summary>
            /// <value>The instance.</value>
            public static T instance
            {
                get
                {
                    if (g_instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        GameObject.DontDestroyOnLoad(go);
                        g_instance = go.AddComponent<T>();
                    }
                    return g_instance;
                }
            }

            #endregion
        }
    }
}