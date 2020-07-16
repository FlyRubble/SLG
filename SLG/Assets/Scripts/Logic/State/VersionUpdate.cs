using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace SLG
{
    using UI;
    using Event;
    using JsonFx;
    using UnityAsset;

    /// <summary>
    /// 状态
    /// </summary>
    public class VersionUpdate : State
    {
        #region Variable
        /// <summary>
        /// WWW资源请求
        /// </summary>
        WWW m_www = null;
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

            // 获取远程版本文件
            string remoteUrl = Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTEVERSION, App.innerVersion));
            m_www = new WWW(remoteUrl);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (m_www != null && m_www.isDone)
            {
                if (string.IsNullOrEmpty(m_www.error))
                {
                    bool forceUpdate = false;
                    Dictionary<string, object> remoteVersion = JsonReader.Deserialize<Dictionary<string, object>>(m_www.text);
                    if (remoteVersion != null)
                    {
                        List<DictPair> data = new List<DictPair>();
                        foreach (var kvp in remoteVersion)
                        {
                            if (kvp.Key.Equals(Const.INNERVERSION))
                            {
                                continue;
                            }
                            data.Add(new DictPair() { key = kvp.Key, value = kvp.Value.ToString() });
                        }
                        App.Init(data);
                        Debugger.logEnabled = App.log;
                        Debugger.webLogEnabled = App.webLog;
                        string[] local = App.innerVersion.Split('.');
                        string[] remote = remoteVersion.ContainsKey(Const.INNERVERSION) ? remoteVersion[Const.INNERVERSION].ToString().Split('.') : new string[0];
                        if (local.Length == remote.Length)
                        {
                            forceUpdate = !(int.Parse(local[0]) >= int.Parse(remote[0]) && int.Parse(local[1]) >= int.Parse(remote[1]));
                        }



                        if (forceUpdate)
                        {
                            // 需要更新版本
                            UnityEngine.Events.UnityAction cancel = () =>
                            {
                                UIManager.instance.CloseUI(Const.UI_TIPSBOX);
                                Application.Quit();
                            };
                            UnityEngine.Events.UnityAction sure = () =>
                            {
                                UIManager.instance.CloseUI(Const.UI_TIPSBOX);
                                Application.OpenURL(App.newVersionDownloadUrl);
                            };
                            Action open = () =>
                            {
                            //    EventListener.instance.OnEvent<Param>("ShowTipsBox", Param.Create(new object[]
                            //    {
                            //"context", ConfigManager.GetLang("InstallNewVersion"),
                            //"cancelText", ConfigManager.GetLang("Quit"), "cancel", cancel,
                            //"sureText", ConfigManager.GetLang("Sure"), "sure", sure,
                            //    }));
                            };
                            //UIManager.instance.OpenUI(Const.UI_TIPSBOX, Param.Create(new object[] { Const.OPENUI, open }), immediate: false);
                        }
                        else
                        {
                            // 资源更新
                            StateMachine.instance.OnEnter(new AssetUpdate());
                        }
                    }
                }
                else
                {
                    // 获取版本信息失败
                    UnityEngine.Events.UnityAction sure = () =>
                    {
                        UIManager.instance.CloseUI(Const.UI_TIPSBOX);
                        Application.Quit();
                    };
                    Action open = () =>
                    {
                        //EventListener.instance.OnEvent<Param>("ShowTipsBox", Param.Create(new object[]
                        //{
                        //    "context", ConfigManager.GetLang("GetVersionFail"),
                        //    "sureText", ConfigManager.GetLang("Quit"), "sure", sure,
                        //}));
                    };
                    //UIManager.instance.OpenUI(Const.UI_TIPSBOX, Param.Create(new object[] { Const.OPENUI, open }), immediate: false);
                }
            }
        }
        #endregion
    }
}