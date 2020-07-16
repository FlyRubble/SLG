using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace SLG
{
    namespace UnityAsset
    {
        public sealed class AssetsBundle : AsyncAsset
        {
            #region Variable
            /// <summary>
            /// WWW
            /// </summary>
            private WWW m_www = null;

            /// <summary>
            /// 资源
            /// </summary>
            private Dictionary<string, Object> m_asset = null;
            #endregion

            #region Property
            /// <summary>
            /// 记载资源的类型
            /// </summary>
            public override LoadType loadType
            {
                get { return LoadType.AssetsBundle; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            public override bool isDone
            {
                get { return m_www != null ? m_www.isDone : false; }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public override float progress
            {
                get
                {
                    m_progress = m_www != null ? m_www.progress : 0f;
                    for (int i = 0; i < dependent.Length; ++i)
                    {
                        m_progress += dependent[i].progress;
                    }
                    return m_progress / (dependent.Length + 1);
                }
            }

            /// <summary>
            /// 字节
            /// </summary>
            public override byte[] bytes
            {
                get
                {
                    return m_www != null ? m_www.bytes : base.bytes;
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public override string error
            {
                get
                {
                    return m_www != null ? m_www.error : "unknown error";
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public AssetsBundle(string url)
                : base(url)
            {
                m_asset = new Dictionary<string, Object>();
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public override void AsyncLoad()
            {
                base.AsyncLoad();
                m_www = new WWW(PathUtil.GetRealUrl(m_url));
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override Object LoadAsset(string name)
            {
                Object asset = null;
                if (!m_asset.TryGetValue(name, out asset))
                {
                    if (m_www != null && m_www.assetBundle != null && !m_www.assetBundle.isStreamedSceneAssetBundle)
                    {
                        asset = m_www.assetBundle.LoadAsset(name);
                    }
                    m_asset.Add(name, asset);
                }
                return asset;
            }

            /// <summary>
            /// 卸载资源
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public override void Unload(bool unloadAllLoadedObjects = true)
            {
                if (null != dependent)
                {
                    foreach (var data in dependent)
                    {
                        if (data.loadState != LoadState.Unload)
                        { 
                            data.Unload(unloadAllLoadedObjects);
                        }
                    }
                }
                base.Unload(unloadAllLoadedObjects);
                if (m_www != null && m_www.assetBundle != null && !m_www.assetBundle.isStreamedSceneAssetBundle)
                {
                    m_www.assetBundle.Unload(unloadAllLoadedObjects);
                }
                m_www?.Dispose();
                m_www = null;
            }
            #endregion
        }
    }
}