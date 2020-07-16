using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace SLG
{
    using UI;
    using IO;
    using JsonFx;
    using Event;
    using UnityAsset;

    /// <summary>
    /// 状态
    /// </summary>
    public class AssetUpdate : State
    {
        #region Variable
        /// <summary>
        /// WWW资源请求
        /// </summary>
        private WWW m_www = null;

        /// <summary>
        /// 远程清单文件
        /// </summary>
        private ManifestConfig m_remoteManifest = null;

        /// <summary>
        /// 远程清单文件字节
        /// </summary>
        private byte[] m_bytes = null;

        /// <summary>
        /// 是否资源更新中
        /// </summary>
        private bool m_assetUpdating = false;

        /// <summary>
        /// 要更新的资源大小
        /// </summary>
        private float m_size = 0;

        /// <summary>
        /// 当前已更新大小
        /// </summary>
        private float m_currentSize = 0;

        /// <summary>
        /// 当前已更新大小(真实)
        /// </summary>
        private float m_currentRealSize = 0F;

        /// <summary>
        /// 上一秒已更新大小(真实)
        /// </summary>
        private float m_lastRealSize = 0F;

        /// <summary>
        /// 时间，协助计算更新速度
        /// </summary>
        private float m_time = 0F;

        /// <summary>
        /// 速度
        /// </summary>
        private float m_speed = 0F;

        /// <summary>
        /// 更新资源记录
        /// </summary>
        private List<UnityAsyncAsset> m_async = new List<UnityAsyncAsset>();
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

            // 获取远程更新清单文件
            m_www = new WWW(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.innerVersion)), "MANIFESTFILE"));
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            // 远程更新清单文件加载
            if (m_www != null && m_www.isDone)
            {
                if (string.IsNullOrEmpty(m_www.error))
                {
                    m_remoteManifest = JsonReader.Deserialize<ManifestConfig>(m_www.assetBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(m_www.url)).text);
                    if (m_www.assetBundle != null)
                    {
                        m_www.assetBundle.Unload(true);
                    }
                }
                else
                {
                    m_remoteManifest = new ManifestConfig();
                }
                StartAssetUpdate();

                m_www.Dispose();
                m_www = null;
            }

            // 更新中...
            if (m_assetUpdating)
            {
                m_currentRealSize = 0;
                int count = Mathf.Min(Const.MAX_LOADER, m_async.Count);
                for (int i = count - 1; i >= 0; --i)
                {
                    m_currentRealSize += (float)m_async[i].asyncAsset.userData * m_async[i].asyncAsset.progress;
                }
                m_currentRealSize += m_currentSize;

                if (m_currentSize == m_size || Time.realtimeSinceStartup >= m_time + 1F)
                {
                    m_speed = m_currentRealSize - m_lastRealSize;
                    m_lastRealSize = m_currentRealSize;
                    m_time = Time.realtimeSinceStartup;
                }

                EventListener.instance.OnEvent<float, float, float>("UpdateProgress", m_currentSize, m_size, m_speed);
                if (m_currentSize == m_size)
                {
                    Util.WriteAllBytes(Path.Combine(PathUtil.persistentDataPath, "MANIFESTFILE"), m_bytes);
                    m_assetUpdating = false;
                    AssetUpdateComplete();
                }
            }
        }

        /// <summary>
        /// 开始资源更新
        /// </summary>
        private void StartAssetUpdate()
        {
            bool hasAssetUpdate = (!string.IsNullOrEmpty(m_remoteManifest.assetVersion) && 
                !App.manifest.assetVersion.Equals(m_remoteManifest.assetVersion));
            // 更新
            if (hasAssetUpdate)
            {
                m_size = m_currentSize = 0f;
                List<Manifest> updateFile = new List<Manifest>();
                foreach (var data in m_remoteManifest.data.Values)
                {
                    if (App.manifest.Contains(data.name))
                    {
                        if (App.manifest.Get(data.name).MD5.Equals(data.MD5))
                        {
                            continue;
                        }
                        else if (data.MD5.Equals(Util.GetMD5(Path.Combine(PathUtil.persistentDataPath, data.name))))
                        {
                            continue;
                        }
                    }
                    m_size += data.size / 1024F;
                    updateFile.Add(data);
                }
                // 写入清单文件
                m_bytes = m_www.bytes;
                if (0 == m_size)
                {
                    Util.WriteAllBytes(Path.Combine(PathUtil.persistentDataPath, "MANIFESTFILE"), m_bytes);
                    AssetUpdateComplete();
                }
                else
                {
                    EventListener.instance.OnEvent<string>("SetTips", null);
                    UnityEngine.Events.UnityAction cancel = () =>
                    {
                        UIManager.instance.CloseUI(Const.UI_TIPSBOX);
                        Application.Quit();
                    };
                    UnityEngine.Events.UnityAction sure = () =>
                    {
                        UIManager.instance.CloseUI(Const.UI_TIPSBOX);
                        AssetManager.instance.UnloadAssets(false);
                        m_async.Clear();
                        // 更新其它文件
                        foreach (var data in updateFile)
                        {
                            UnityAsyncAsset async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.innerVersion)), data.name), (bResult, asset) =>
                            {
                                if (bResult)
                                {
                                    Util.WriteAllBytes(Path.Combine(PathUtil.persistentDataPath, data.name), asset.asyncAsset.bytes);
                                }
                                else
                                {
                                    Debugger.LogError(asset.error);
                                }
                                m_currentSize += (float)asset.asyncAsset.userData;
                                m_async.Remove(asset);
                            });
                            async.asyncAsset.userData = data.size / 1024F;
                            m_async.Add(async);
                        }

                        // 记录是否资源更新中
                        m_assetUpdating = true;
                        m_time = Time.realtimeSinceStartup;
                    };
                    Action open = () =>
                    {
                        //EventListener.instance.OnEvent<Param>("ShowTipsBox", Param.Create(new object[]
                        //{
                        //    "context", ConfigManager.GetLangFormat("NewAssetNeedUpdate", "", m_size),
                        //    "cancelText", ConfigManager.GetLang("Quit"), "cancel", cancel,
                        //    "sureText", ConfigManager.GetLang("Sure"), "sure", sure,
                        //}));
                    };
                    //UIManager.instance.OpenUI(Const.UI_TIPSBOX, Param.Create(new object[] { Const.OPENUI, open }), immediate: false);
                }
            }
            // 如果没有资源更新，就直接认为更新完成
            else
            {
                AssetUpdateComplete();
            }
        }

        /// <summary>
        /// 资源更新完成
        /// </summary>
        private void AssetUpdateComplete()
        {
            // 资源加载
            StateMachine.instance.OnEnter(new AssetLoad());
        }
        #endregion
    }
}