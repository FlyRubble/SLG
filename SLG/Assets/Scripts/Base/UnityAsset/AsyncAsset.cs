using UnityEngine;
using System.Collections.Generic;

namespace SLG
{
    namespace UnityAsset
    {
        public abstract class AsyncAsset : AsyncOperation
        {
            /// <summary>
            /// 状态
            /// </summary>
            public enum LoadState
            {
                None = 0,
                Wait = 1,
                Loading = 2,
                Complete = 3,
                Unload = 4,
            }

            /// <summary>
            /// 加载资源类型
            /// </summary>
            public enum LoadType
            {
                None = 0,
                Resource,
                AssetsBundle,
                AssetDataBase,
            }

            #region Variable
            /// <summary>
            /// url
            /// </summary>
            protected string m_url = string.Empty;

            /// <summary>
            /// 加载状态
            /// </summary>
            protected LoadState m_loadState = LoadState.None;

            /// <summary>
            /// 用户数据
            /// </summary>
            protected object m_userData = null;

            /// <summary>
            /// 进度
            /// </summary>
            protected float m_progress = 0f;

            /// <summary>
            /// 依赖资源数组
            /// </summary>
            private AsyncAsset[] m_dependent = null;
            #endregion

            #region Property
            /// <summary>
            /// 记载资源的类型
            /// </summary>
            public virtual LoadType loadType
            {
                get { return LoadType.None; }
            }

            /// <summary>
            /// 得到url
            /// </summary>
            /// <value>The URL Path.</value>
            public string url
            {
                get { return m_url; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
            public virtual bool isDone
            {
                get { return false; }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public virtual float progress
            {
                get { return 0f; }
            }

            /// <summary>
            /// 字节
            /// </summary>
            public virtual byte[] bytes
            {
                get { return new byte[0]; }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public virtual string error
            {
                get { return string.Empty; }
            }

            /// <summary>
            /// 加载状态
            /// </summary>
            /// <value>The state of the load.</value>
            public LoadState loadState
            {
                get { return m_loadState; }

            }

            /// <summary>
            /// 用户数据
            /// </summary>
            public object userData
            {
                get { return m_userData; }
                set { m_userData = value; }
            }

            /// <summary>
            /// 依赖列表
            /// </summary>
            public AsyncAsset[] dependent
            {
                get { return m_dependent; }
                set { m_dependent = value; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public AsyncAsset(string url)
            {
                m_url = url;
                m_loadState = LoadState.Wait;
                m_dependent = new AsyncAsset[0];
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public virtual void AsyncLoad()
            {
                m_loadState = LoadState.Loading;
            }

            /// <summary>
            /// 异步加载完成
            /// </summary>
            public virtual void Complete()
            {
                m_loadState = LoadState.Complete;
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public abstract Object LoadAsset(string name);

            /// <summary>
            /// 卸载资源
            /// </summary>
            public virtual void Unload(bool unloadAllLoadedObjects = true)
            {
                m_loadState = LoadState.Unload;
            }
            #endregion
        }
    }
}