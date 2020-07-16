using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SLG
{
    using Event;
    using Singleton;

    namespace UnityAsset
    {
        public class AssetManager : Singleton<AssetManager>
        {
            #region Variable
            /// <summary>
            /// 记录已加载完成的所有异步资源
            /// </summary>
            Dictionary<string, AsyncAsset> m_complete = null;

            /// <summary>
            /// 记录正在加载中的异步资源
            /// </summary>
            Dictionary<string, AsyncAsset> m_loading = null;

            /// <summary>
            /// 等待加载的异步资源
            /// </summary>
            List<UnityAsyncAsset> m_queue = null;

            /// <summary>
            /// 待移除表
            /// </summary>
            List<AsyncAsset> m_remove = null;

            /// <summary>
            /// 是否允许加载
            /// </summary>
            bool m_isAllowLoad = true;

            /// <summary>
            /// 最大同时加载个数
            /// </summary>
            int m_maxLoader = 1;

            /// <summary>
            /// 当前最大加载数
            /// </summary>
            int m_currentMaxLoader = 0;
            #endregion

            #region Property
            /// <summary>
            /// 得到或设置是否允许加载
            /// </summary>
            public bool isAllowload
            {
                get { return m_isAllowLoad; }
                set { m_isAllowLoad = value; }
            }

            /// <summary>
            /// 得到或设置最大同时加载数
            /// </summary>
            /// <value>The max loader.</value>
            public int maxLoader
            {
                get { return m_maxLoader; }
                set { m_maxLoader = value; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public AssetManager()
            {
                m_complete = new Dictionary<string, AsyncAsset>(1 << 7);
                m_loading = new Dictionary<string, AsyncAsset>(1 << 4);
                m_queue = new List<UnityAsyncAsset>(1 << 4);
                m_remove = new List<AsyncAsset>(m_loading.Count);
            }

            /// <summary>
            /// Resources加载(无需后缀名)
            /// </summary>
            /// <param name="path">Path.</param>
            public Object ResourceLoad(string path)
            {
                return Resources.Load(path);
            }

            /// <summary>
            /// Resources加载(无需后缀名)
            /// </summary>
            /// <param name="path">Path.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public T ResourceLoad<T>(string path) where T : Object
            {
                return Resources.Load<T>(path);
            }

            /// <summary>
            /// Resources加载(无需后缀名)
            /// </summary>
            /// <param name="path">Path.</param>
            /// <param name="type">Type.</param>
            public Object ResourceLoad(string path, System.Type type)
            {
                return Resources.Load(path, type);
            }

            /// <summary>
            /// Resource异步加载(无需后缀名)
            /// </summary>
            /// <param name="path"></param>
            /// <param name="action"></param>
            /// <returns></returns>
            public UnityAsyncAsset ResourceAsyncLoad(string path, Action<bool, UnityAsyncAsset> complete)
            {
                AsyncAsset async = null;
                if (m_complete.ContainsKey(path))
                {
                    async = m_complete[path];
                }
                else if (m_loading.ContainsKey(path))
                {
                    async = m_loading[path];
                }
                else
                {
                    async = new Resource(path);
                    m_loading.Add(async.url, async);
                }
                UnityAsyncAsset unityAsyncAsset = new UnityAsyncAsset(path, async, complete);
                m_queue.Add(unityAsyncAsset);

                return unityAsyncAsset;
            }

            /// <summary>
            /// AssetBundle加载
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public UnityAsyncAsset AssetBundleLoad(string path)
            {
                path = GetBundlePath(path);

                AsyncAsset async = null;
                if (m_complete.ContainsKey(path))
                {
                    async = m_complete[path];
                }
                else
                {
                    if (m_loading.ContainsKey(path))
                    {
                        async = m_loading[path];
                    }
                    else
                    {
                        async = new AssetsBundle(path);
                        var data = GetDependencies(path);
                        async.dependent = new AsyncAsset[data.Count];
                        for (int i = 0; i < data.Count; ++i)
                        {
                            async.dependent[i] = AssetBundleLoad(data[i]).asyncAsset;
                        }
                        async.AsyncLoad();
                    }
                    while (!async.isDone) { }
                    async.Complete();
                    m_complete.Add(async.url, async);
                }

                UnityAsyncAsset unityAsyncAsset = new UnityAsyncAsset(path, async, null);
                return unityAsyncAsset;
            }

            /// <summary>
            /// AssetBundle依赖资源加载
            /// </summary>
            /// <param name="path"></param>
            /// <param name="action"></param>
            /// <returns></returns>
            public UnityAsyncAsset AssetBundleAsyncLoad(string path, Action<bool, UnityAsyncAsset> complete, Action<bool, UnityAsyncAsset> action = null)
            {
                path = GetBundlePath(path);
                return AssetBundleAsyncLoadDependent(path, complete, action);
            }

            /// <summary>
            /// AssetBundle依赖资源加载
            /// </summary>
            /// <param name="path"></param>
            /// <param name="action"></param>
            /// <param name="dic"></param>
            private UnityAsyncAsset AssetBundleAsyncLoadDependent(string path, Action<bool, UnityAsyncAsset> complete, Action<bool, UnityAsyncAsset> action)
            {
                AsyncAsset async = null;
                if (m_complete.ContainsKey(path))
                {
                    async = m_complete[path];
                }
                else if (m_loading.ContainsKey(path))
                {
                    async = m_loading[path];
                }
                else
                {
                    async = new AssetsBundle(path);
                    var data = GetDependencies(path);
                    async.dependent = new AsyncAsset[data.Count];
                    for (int i = 0; i < data.Count; ++i)
                    {
                        async.dependent[i] = AssetBundleAsyncLoadDependent(data[i], action, action).asyncAsset;
                    }
                    m_loading.Add(async.url, async);
                }
                UnityAsyncAsset unityAsyncAsset = new UnityAsyncAsset(path, async, complete);
                m_queue.Add(unityAsyncAsset);

                return unityAsyncAsset;
            }

            /// <summary>
            /// 更新
            /// </summary>
            public void Update()
            {
                if (m_isAllowLoad)
                {
                    // 正在加载中的处理
                    m_currentMaxLoader = Mathf.Min(m_maxLoader, m_loading.Count);
                    if (m_currentMaxLoader > 0)
                    {
                        m_remove.Clear();
                        foreach (var kvp in m_loading)
                        {
                            switch (kvp.Value.loadState)
                            {
                                case AsyncAsset.LoadState.Wait:
                                    {
                                        kvp.Value.AsyncLoad();
                                    }
                                    break;
                                case AsyncAsset.LoadState.Loading:
                                    {
                                        if (kvp.Value.isDone)
                                        {
                                            kvp.Value.Complete();
                                            m_remove.Add(kvp.Value);
                                        }
                                    }
                                    break;
                            }
                            if (--m_currentMaxLoader == 0)
                            {
                                break;
                            }
                        }
                        foreach (var asyn in m_remove)
                        {
                            m_complete.Add(asyn.url, asyn);
                            m_loading.Remove(asyn.url);
                        }
                    }
                    // 等待加载中的处理                
                    if (m_queue.Count > 0 && m_queue[0].isDone)
                    {
                        UnityAsyncAsset asyncBundle = m_queue[0];
                        m_queue.Remove(asyncBundle);
                        asyncBundle.Complete();
                    }
                }
            }

            /// <summary>
            /// 卸载所有资源
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public void UnloadAssets(bool unloadAllLoadedObjects)
            {
                foreach (var asyn in m_complete.Values)
                {
                    asyn.Unload(unloadAllLoadedObjects);
                }
                foreach (var asyn in m_loading.Values)
                {
                    asyn.Unload(unloadAllLoadedObjects);
                }

                m_complete.Clear();
                m_loading.Clear();
                m_queue.Clear();
            }

            /// <summary>
            /// 卸载指定资源
            /// </summary>
            /// <param name="asset"></param>
            /// <param name="unloadAllLoadedObjects"></param>
            public void UnloadAsset(AsyncAsset asset, bool unloadAllLoadedObjects)
            {
                if (null == asset)
                {
                    return;
                }

                if (m_complete.ContainsKey(asset.url) && asset == m_complete[asset.url])
                {
                    asset.Unload(unloadAllLoadedObjects);
                    m_complete.Remove(asset.url);
                }
                else if (m_loading.ContainsKey(asset.url) && asset == m_loading[asset.url])
                {
                    asset.Unload(unloadAllLoadedObjects);
                    m_loading.Remove(asset.url);
                }
                for (int i = 0; i < m_queue.Count; ++i)
                {
                    if (m_queue[i].asyncAsset == asset)
                    {
                        m_queue.RemoveAt(i);
                        break;
                    }
                }
            }

            /// <summary>
            /// 得到依赖列表
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public List<string> GetDependencies(string path)
            {
                return App.manifest.GetDependencies(GetBundlePath(path));
            }

            /// <summary>
            /// 得到捆绑资源的短路径
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public string GetBundlePath(string path)
            {
                return App.manifestMapping.Get(path);
            }
            #endregion
        }
    }

}