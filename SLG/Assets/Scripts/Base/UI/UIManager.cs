using UnityEngine;
using System.Collections.Generic;

namespace SLG
{
    using Singleton;
    using Event;
    using UnityAsset;
    namespace UI
    {
        /// <summary>
        /// UI管理器
        /// </summary>
        public sealed class UIManager : Singleton<UIManager>
        {
            #region Variable
            /// <summary>
            /// 界面集合
            /// </summary>
            private Dictionary<string, UIBase> m_data = null;

            /// <summary>
            /// 参数表
            /// </summary>
            private Dictionary<string, Action<UIBase>> m_param = null;

            /// <summary>
            /// root
            /// </summary>
            private Transform m_root = null;
            #endregion

            #region Function
            /// <summary>
            /// 构造
            /// </summary>
            public UIManager()
            {
                m_data = new Dictionary<string, UIBase>();
                m_param = new Dictionary<string, Action<UIBase>>();

                AssetManager.instance.LoadUI(Const.UI_ROOT, (bResult, target) =>
                {
                    if (bResult && target != null)
                    {
                        GameObject go = GameObject.Instantiate(target) as GameObject;
                        go.name = go.name.Replace("(Clone)", "");
                        go.transform.parent = m_root;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = Vector3.one;
                        go.transform.localRotation = Quaternion.identity;

                        GameObject.DontDestroyOnLoad(go);
                        m_root = go.transform.Find("Center");
                    }
                }, async: false);
            }

            /// <summary>
            /// 是否包含界面
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            //public bool Contain(string name)
            //{
            //    return m_data.ContainsKey(name);
            //}

            ///// <summary>
            ///// 得到UI
            ///// </summary>
            ///// <param name="name"></param>
            ///// <returns></returns>
            //public UIBase GetUI(string name)
            //{
            //    UIBase t = null;
            //    if (string.IsNullOrEmpty(name))
            //    {
            //        Debugger.LogErrorFormat("UI: name '{0}' is null or empty!", name);
            //    }
            //    else
            //    {
            //        m_data.TryGetValue(name, out t);
            //    }
            //    return t;
            //}


            /// <summary>
            /// 打开UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void OpenUI(string name, Action<UIBase> loadFinish, bool immediate = false)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debugger.LogError("UI: name is null or empty!");
                    return;
                }

                if (m_data.ContainsKey(name))
                {
                    UIBase t = m_data[name];
                    if (null != t && !t.show)
                    {
                        t.Open();
                    }
                }
                else
                {
                    if (m_param.ContainsKey(name))
                    {
                        m_param[name] = loadFinish;
                    }
                    else
                    {
                        m_param.Add(name, loadFinish);

                        AssetManager.instance.LoadUI(name, (bResult, target) =>
                        {
                            if (bResult && target != null)
                            {
                                GameObject go = GameObject.Instantiate(target) as GameObject;
                                go.name = go.name.Replace("(Clone)", "");
                                go.transform.parent = m_root;
                                go.transform.localPosition = Vector3.zero;
                                go.transform.localScale = Vector3.one;
                                go.transform.localRotation = Quaternion.identity;
                                RectTransform rectTransform = go.transform as RectTransform;
                                if (rectTransform != null)
                                {
                                    rectTransform.offsetMin = Vector3.zero;
                                    rectTransform.offsetMax = Vector3.zero;
                                }
                                UIBase t = go.GetComponent<UIBase>();
                                if (null != t)
                                {
                                    m_param[name](t);
                                    m_data.Add(name, t);
                                    t.Open();
                                }
                                else
                                {
                                    Debugger.LogErrorFormat("UI: '{0}' is not find!", name);
                                }
                                m_param.Remove(name);
                            }
                        }, async: !immediate);
                    }
                }
            }

            /// <summary>
            /// 关闭UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void CloseUI(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debugger.LogError("UI: name is null or empty!");
                    return;
                }

                if (m_data.ContainsKey(name))
                {
                    UIBase t = m_data[name];
                    if (null != t)
                    {
                        if (t.show)
                        {
                            t.Close();
                            if (!t.cache)
                            {
                                GameObject.Destroy(t.gameObject);
                                m_data.Remove(name);
                            }
                        }
                    }
                    else
                    {
                        Debugger.LogErrorFormat("UI: '{0}' is not exist!", name);
                    }
                }
            }

            ///// <summary>
            ///// 关闭所有显示的UI
            ///// </summary>
            ///// <param name="param"></param>
            //public void CloseAllUI(Param param = null)
            //{
            //    Action closeAll = param["closeall"] as Action;
            //    param.Remove("closeall");
            //    List<string> filter = param["filter"] as List<string>;
            //    param.Remove("filter");

            //    int showNum = 0;
            //    Param.Destroy(param);
            //    param = Param.Create();
            //    Action close = () =>
            //    {
            //        --showNum;
            //        if (showNum == 0)
            //        {
            //            if (null != closeAll)
            //            {
            //                closeAll();
            //            }
            //        }
            //    };
            //    param.Add("close", close);

            //    List<string> nameList = new List<string>(m_data.Keys);
            //    foreach (var name in nameList)
            //    {
            //        if (m_data[name].show && filter.Contains(name))
            //        {
            //            ++showNum;
            //            m_data[name].Close();
            //            if (!m_data[name].cache)
            //            {
            //                GameObject.Destroy(m_data[name].gameObject);
            //                m_data.Remove(name);
            //            }
            //        }
            //    }
            //}

            ///// <summary>
            ///// 设置同对象中的顺序
            ///// </summary>
            ///// <param name="name"></param>
            ///// <param name="index"></param>
            //public void SetSiblingIndex(string name, int index)
            //{
            //    if (string.IsNullOrEmpty(name))
            //    {
            //        Debugger.LogErrorFormat("UI: name '{0}' is null or empty!", name);
            //        return;
            //    }

            //    if (m_data.ContainsKey(name))
            //    {
            //        UIBase t = m_data[name];
            //        if (null != t)
            //        {
            //            t.SetSiblingIndex(index);
            //        }
            //        else
            //        {
            //            Debugger.LogErrorFormat("UI: '{0}' is not exist!", name);
            //        }
            //    }
            //}

            ///// <summary>
            ///// 设置UI在同父对象下顺序
            ///// </summary>
            ///// <param name="name"></param>
            ///// <param name="names">排序在姐妹对象前面</param>
            //public void SetSiblingIndex(string name, params string[] names)
            //{
            //    if (string.IsNullOrEmpty(name))
            //    {
            //        Debugger.LogErrorFormat("UI: name '{0}' is null or empty!", name);
            //        return;
            //    }

            //    int index = 0;
            //    UIBase t = GetUI(name);
            //    if (null != t)
            //    {
            //        index = t.siblingIndex;
            //    }
            //    else
            //    {
            //        Debugger.LogErrorFormat("UI: '{0}' is not exist!", name);
            //        return;
            //    }


            //    for (int i = 0; i < names.Length; ++i)
            //    {
            //        t = GetUI(names[i]);
            //        if (null != t && index > t.siblingIndex)
            //        {
            //            index = t.siblingIndex;
            //        }
            //    }

            //    m_data.TryGetValue(name, out t);
            //    t.SetSiblingIndex(--index);
            //}

            /// <summary>
            /// 清理数据
            /// </summary>
            public override void Clear()
            {
                base.Clear();

                foreach (var t in m_data.Values)
                {
                    if (null != t)
                    {
                        t.ClearEvent();
                        t.Close();
                        GameObject.Destroy(t.gameObject);
                    }
                }
                m_data.Clear();
                m_param.Clear();
            }
            #endregion
        }
    }
}
