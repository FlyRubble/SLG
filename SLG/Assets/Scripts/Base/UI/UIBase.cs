using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SLG
{
    using Event;

    namespace UI
    {
        public class ShowInspector : System.Attribute { }

        /// <summary>
        /// UIBase
        /// </summary>
        public class UIBase : MonoBehaviour
        {
            #region Variable
            /// <summary>
            /// 缓存
            /// </summary>
            [Tooltip("是否需要缓存")]
            [SerializeField] protected bool m_cache = false;

            /// <summary>
            /// 层选项
            /// </summary>
            [Tooltip("层选项")]
            [SerializeField] protected Sibling m_sibling = Sibling.First;

            /// <summary>
            /// 层级顺序
            /// </summary>
            [Tooltip("显示层级顺序")]
            [SerializeField] protected int m_sortOrder = 0;

            /// <summary>
            /// 需要操作的GameObject列表
            /// </summary>
            [Header("需要操作的GameObject列表")]
            [SerializeField] public List<GameObject> m_list = new List<GameObject>();
            private Dictionary<string, GameObject> m_dict = new Dictionary<string, GameObject>();

            /// <summary>
            /// Canvas设置层级后跟显示相关
            /// </summary>
            protected Canvas m_canvas = null;

            /// <summary>
            /// GraphicRaycaster图像射线碰撞相关
            /// </summary>
            protected GraphicRaycaster m_graphicRaycaster = null;

            /// <summary>
            /// 是否显示可见
            /// </summary>
            protected bool m_show = false;

            /// <summary>
            /// 组件字典
            /// </summary>
            private Dictionary<string, Component> m_componentDict = null;
            #endregion

            #region Property
            /// <summary>
            /// 当前对象名字
            /// </summary>
            public virtual string getName
            {
                get { return this.name; }
            }

            /// <summary>
            /// 是否需要缓存
            /// </summary>
            public bool cache
            {
                get { return m_cache; }
            }

            /// <summary>
            /// 对象顺序索引
            /// </summary>
            public int siblingIndex
            {
                get { return transform.GetSiblingIndex(); }
            }

            /// <summary>
            /// 当前是否显示可见
            /// </summary>
            public bool show
            {
                get { return m_show; }
            }


            #region Event
            public Action openUI
            {
                get; set;
            }

            public Action closeUI
            {
                get; set;
            }

            public Action onDestroy
            {
                get; set;
            }
            #endregion
            #endregion

            #region Function
            #region Component
            /// <summary>
            /// 启动
            /// </summary>
            protected virtual void Awake()
            {
                gameObject.SetActive(true);
                m_canvas = AddComponent<Canvas>();
                m_graphicRaycaster = AddComponent<GraphicRaycaster>();
                m_componentDict = new Dictionary<string, Component>(1<<3);
                for (int i = 0; i < m_list.Count; ++i)
                {
                    m_dict.Add(m_list[i].name.Remove(0, 1), m_list[i]);
                }

                AddListener();
            }

            /// <summary>
            /// 销毁
            /// </summary>
            protected virtual void OnDestroy()
            {
                RemoveListener();
                onDestroy?.Invoke();
            }
            #endregion

            /// <summary>
            /// 添加监听
            /// </summary>
            protected virtual void AddListener() { }

            /// <summary>
            /// 移除监听
            /// </summary>
            protected virtual void RemoveListener() { }

            /// <summary>
            /// 得到组件
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="componentName"></param>
            /// <returns></returns>
            public T GetComponent<T>(string componentName) where T : Component
            {
                string key = componentName + "@" + typeof(T).Name;
                if (m_componentDict.ContainsKey(key))
                {
                    return (T)m_componentDict[key];
                }
                else
                {
                    T t = null;
                    if (m_dict.ContainsKey(componentName))
                    {
                        t = m_dict[componentName].GetComponent<T>();
                        m_componentDict.Add(key, t);
                    }
                    return t;
                }
            }

            /// <summary>
            /// 得到组件
            /// </summary>
            /// <param name="name"></param>
            /// <param name="componentName"></param>
            /// <returns></returns>
            public Component GetComponent(string name, string componentName)
            {
                Component component = null;
                string key = name + "@" + componentName;
                if (m_componentDict.ContainsKey(key))
                {
                    component = m_componentDict[key];
                }
                else
                {
                    if (m_dict.ContainsKey(name))
                    {
                        component = m_dict[name].GetComponent(componentName);
                        m_componentDict.Add(key, component);
                    }
                }
                return component;
            }

            /// <summary>
            /// 添加组件(不存在则添加)
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            private T AddComponent<T>() where T : Component
            {
                T t = gameObject.GetComponent<T>();
                t = t ?? gameObject.AddComponent<T>();
                return t;
            }

            /// <summary>
            /// 显示
            /// </summary>
            private void Show()
            {
                m_show = true;
                SetLayer(transform, (int)Layers.UI);
                m_graphicRaycaster.enabled = true;
                if (Sibling.First == m_sibling)
                {
                    transform.SetAsFirstSibling();
                }
                else if (Sibling.Last == m_sibling)
                {
                    transform.SetAsLastSibling();
                }
                else
                {
                    transform.SetSiblingIndex(m_sortOrder);
                }
            }

            /// <summary>
            /// 隐藏
            /// </summary>
            private void Hide()
            {
                m_show = false;
                SetLayer(transform, (int)Layers.HIDE);
                m_graphicRaycaster.enabled = false;
            }

            /// <summary>
            /// 设置层
            /// </summary>
            /// <param name="tf"></param>
            /// <param name="layer"></param>
            private void SetLayer(Transform tf, int layer)
            {
                tf.gameObject.layer = layer;
            }

            /// <summary>
            /// 设置同对象中的顺序
            /// </summary>
            /// <param name="index"></param>
            public void SetSiblingIndex(int index)
            {
                transform.SetSiblingIndex(m_sortOrder);
            }

            /// <summary>
            /// 清理事件
            /// </summary>
            public void ClearEvent()
            {
                openUI = null;
                closeUI = null;
                onDestroy = null;
            }

            /// <summary>
            /// 打开
            /// </summary>
            /// <param name="param"></param>
            public void Open()
            {
                this.Show();
                openUI?.Invoke();
            }

            /// <summary>
            /// 关闭
            /// </summary>
            /// <param name="param"></param>
            public void Close()
            {
                closeUI?.Invoke();
                this.Hide();
            }
            #endregion
        }
    }
}