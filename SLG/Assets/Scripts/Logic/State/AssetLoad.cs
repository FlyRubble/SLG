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
    public class AssetLoad : State
    {
        enum AssetState
        {
            Ready,
            Loading,
            Complete,
        }

        #region Variable
        /// <summary>
        /// 资源部署状态
        /// </summary>
        private AssetState m_state = AssetState.Ready;

        /// <summary>
        /// 要加载的资源大小
        /// </summary>
        private float m_size = 0;

        /// <summary>
        /// 当前已加载大小
        /// </summary>
        private float m_currentSize = 0;

        /// <summary>
        /// 当前已加载大小(真实)
        /// </summary>
        private float m_currentRealSize = 0F;

        /// <summary>
        /// 上一秒已加载大小(真实)
        /// </summary>
        private float m_lastRealSize = 0F;

        /// <summary>
        /// 时间，协助计算加载速度
        /// </summary>
        private float m_time = 0F;

        /// <summary>
        /// 速度
        /// </summary>
        private float m_speed = 0F;

        /// <summary>
        /// 加载资源记录
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

            AssetManager.instance.UnloadAssets(true);
            // 获取清单数据
            UnityAsyncAsset asset = AssetManager.instance.AssetBundleLoad("MANIFESTFILE");
            if (string.IsNullOrEmpty(asset.error))
            {
                App.manifest = JsonReader.Deserialize<ManifestConfig>(asset.text);
                AssetManager.instance.UnloadAsset(asset.asyncAsset, true);
            }
            // 获取清单映射数据
            asset = AssetManager.instance.AssetBundleLoad("MANIFESTFILEMAPPING");
            if (string.IsNullOrEmpty(asset.error))
            {
                App.manifestMapping = JsonReader.Deserialize<ManifestMappingConfig>(asset.text);
                AssetManager.instance.UnloadAsset(asset.asyncAsset, true);
            }
            // 加载Loading界面
            UIManager.instance.Clear();
            Action open = () =>
            {
                //EventListener.instance.OnEvent<string>("SetTips", ConfigManager.GetLang("AssetLoading"));
            };
            //UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] { Const.OPENUI, open }), immediate: false);
            // 预加载
            Lua instance = Lua.instance;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            switch (m_state)
            {
                case AssetState.Ready:
                    {
                        //if (Lua.instance.init)
                        //{
                        //    StartAssetLoad();
                        //}
                    }
                    break;
                case AssetState.Loading:
                    {
                        m_currentRealSize = 0;
                        int count = Mathf.Min(Const.MAX_LOADER, m_async.Count);
                        for (int i = count - 1; i >= 0; --i)
                        {
                            m_currentRealSize += (float)m_async[i].asyncAsset.userData * m_async[i].asyncAsset.progress;
                        }
                        m_currentRealSize += m_currentSize;

                        if ((m_currentSize == m_size) || Time.realtimeSinceStartup >= m_time + 1F)
                        {
                            m_speed = m_currentRealSize - m_lastRealSize;
                            m_lastRealSize = m_currentRealSize;
                            m_time = Time.realtimeSinceStartup;
                        }

                        EventListener.instance.OnEvent<float, float, float>("UpdateProgress", m_currentSize, m_size, m_speed);
                        if (m_currentSize == m_size)
                        {
                            m_state = AssetState.Complete;
                            AssetLoadComplete();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 开始资源加载
        /// </summary>
        private void StartAssetLoad()
        {
            // 加载配置
            //ConfigManager.instance.Init();
            //float USERDATA = 0.5F;
            //foreach (var data in ConfigManager.instance.loadList)
            //{
            //    bool needAdd = true;
            //    UnityAsyncAsset async = AssetManager.instance.LoadConf(data.Key, (bResult, asset) =>
            //    {
            //        if (bResult)
            //        {
            //            data.Value(asset.text);
            //        }
            //        else
            //        {
            //            Debugger.LogError(asset.error);
            //        }
            //        m_currentSize += USERDATA;
            //        m_async.Remove(asset);
            //        needAdd = false;
            //    });
            //    async.asyncAsset.userData = USERDATA;
            //    m_size += (float)async.asyncAsset.userData;
            //    if (needAdd)
            //    {
            //        m_async.Add(async);
            //    }
            //}

            if (m_size > 0)
            {
                // 记录是否资源加载中
                m_state = AssetState.Loading;
                m_time = Time.realtimeSinceStartup;
            }
            else
            {
                m_state = AssetState.Complete;
                AssetLoadComplete();
            }
        }

        /// <summary>
        /// 资源加载完成
        /// </summary>
        private void AssetLoadComplete()
        {
            // 开始登陆
            StateMachine.instance.OnEnter(new StartLogin());
        }
        #endregion
    }
}