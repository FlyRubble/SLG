using UnityEngine;

namespace SLG
{
    using Event;
    using System.IO;

    namespace UnityAsset
    {
        /// <summary>
        /// 异步资源包
        /// </summary>
        public struct UnityAsyncAsset
        {
            #region Variable
            /// <summary>
            /// 资源名字
            /// </summary>
            private string m_assetName;

            /// <summary>
            /// 异步资源
            /// </summary>
            private AsyncAsset m_asyncAsset;

            /// <summary>
            /// 完成事件
            /// </summary>
            private Action<bool, UnityAsyncAsset> m_action;
            #endregion

            #region Property
            /// <summary>
            /// 资源名字
            /// </summary>
            public string assetName
            {
                get { return m_assetName; }
            }

            /// <summary>
            /// 异步资源
            /// </summary>
            public AsyncAsset asyncAsset
            {
                get { return m_asyncAsset; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            public bool isDone
            {
                get { return m_asyncAsset != null ? m_asyncAsset.isDone : false; }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public string error
            {
                get
                {
                    return m_asyncAsset != null ? m_asyncAsset.error : "unknown error";
                }
            }

            /// <summary>
            /// 得到资源
            /// </summary>
            public Object mainAsset
            {
                get
                {
                    return m_asyncAsset.LoadAsset(m_assetName);
                }
            }

            /// <summary>
            /// 得到文本
            /// </summary>
            public string text
            {
                get
                {
                    Object mainAsset = m_asyncAsset.LoadAsset(m_assetName);
                    return mainAsset != null ? mainAsset.ToString() : string.Empty;
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="asyncAsset"></param>
            /// <param name="action"></param>
            public UnityAsyncAsset(string assetName, AsyncAsset asyncAsset, Action<bool, UnityAsyncAsset> action)
            {
                m_assetName = Path.GetFileNameWithoutExtension(assetName);
                m_asyncAsset = asyncAsset;
                m_action = action;
            }

            /// <summary>
            /// 异步加载完成
            /// </summary>
            public void Complete()
            {
                Object o = mainAsset;
                m_action?.Invoke(string.IsNullOrEmpty(error), this);
            }
            #endregion
        }
    }
}