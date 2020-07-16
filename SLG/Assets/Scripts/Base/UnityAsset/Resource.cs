using UnityEngine;

namespace SLG
{
    namespace UnityAsset
    {
        public sealed class Resource : AsyncAsset
        {
            #region Variable
            /// <summary>
            /// Resource资源
            /// </summary>
            private ResourceRequest m_resourceRequest = null;

            /// <summary>
            /// 资源
            /// </summary>
            private Object m_mainAsset = null;
            #endregion

            #region Property
            /// <summary>
            /// 记载资源的类型
            /// </summary>
            public override LoadType loadType
            {
                get { return LoadType.Resource; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            public override bool isDone
            {
                get { return m_resourceRequest != null ? m_resourceRequest.isDone : false; }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public override float progress
            {
                get
                {
                    m_progress = m_resourceRequest != null ? m_resourceRequest.progress : 0f;
                    for (int i = 0; i < dependent.Length; ++i)
                    {
                        m_progress += dependent[i].progress;
                    }
                    return m_progress / (dependent.Length + 1);
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public override string error
            {
                get
                {
                    return m_resourceRequest != null ? "" : "unknown error";
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public Resource(string url)
                : base(url)
            { }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public override void AsyncLoad()
            {
                base.AsyncLoad();
                m_resourceRequest = Resources.LoadAsync(m_url);
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override Object LoadAsset(string name)
            {
                if (m_mainAsset == null && m_resourceRequest != null)
                {
                    m_mainAsset = m_resourceRequest.asset;
                }
                return m_mainAsset;
            }

            /// <summary>
            /// 卸载
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public override void Unload(bool unloadAllLoadedObjects = true)
            {
                base.Unload(unloadAllLoadedObjects);
                Resources.UnloadAsset(m_mainAsset);
                m_resourceRequest = null;
            }
            #endregion
        }
    }
}