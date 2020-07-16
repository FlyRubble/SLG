using System.Collections.Generic;
using UnityEngine.Events;

namespace SLG
{
    using Event;
    using Pool;
    using SLG.UI;

    /// <summary>
    /// 参数
    /// </summary>
    public sealed class Param
    {
        #region Variable
        /// <summary>
        /// 参数表
        /// </summary>
        private Dictionary<object, object> m_param;
        #endregion

        #region Property
        /// <summary>
        /// 参数大小
        /// </summary>
        public int Count
        {
            get
            {
                return m_param.Count;
            }
        }

        /// <summary>
        /// 得到参数Key
        /// </summary>
        public List<object> keys
        {
            get
            {
                return new List<object>(m_param.Keys);
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <returns></returns>
        public static Param Create()
        {
            Pool<Param> pool = PoolManager.instance.Get<Pool<Param>>(Const.PARAMPOOL);
            Param param = pool.Get();
            param.Clear();
            return param;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Param Create(object[] param)
        {
            Param newParam = Param.Create();
            newParam.Add(param);
            return newParam;
        }

        /// <summary>
        /// 销毁参数
        /// </summary>
        /// <param name="param"></param>
        public static void Destroy(Param param)
        {
            Pool<Param> pool = PoolManager.instance.Get<Pool<Param>>(Const.PARAMPOOL);
            pool.Release(param);
        }

        /// <summary>
        /// 无参构造
        /// </summary>
        public Param()
        {
            m_param = new Dictionary<object, object>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(object name, object value)
        {
            if (m_param.ContainsKey(name))
            {
                m_param[name] = value;
            }
            else
            {
                m_param.Add(name, value);
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddEvent(object name, Action value)
        {
            if (m_param.ContainsKey(name))
            {
                m_param[name] = value;
            }
            else
            {
                m_param.Add(name, value);
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="name"></param>
        public void RemoveEvent(object name)
        {
            if (m_param.ContainsKey(name))
            {
                m_param[name] = null;
                m_param.Remove(name);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        public void Add(object[] param)
        {
            for (int i = 1; i < param.Length; ++i, ++i)
            {
                Add(param[i - 1], param[i]);
            }
        }

        /// <summary>
        /// 尝试添加(没有才添加)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void TryAdd(object name, object value)
        {
            if (!m_param.ContainsKey(name))
            {
                m_param.Add(name, value);
            }
        }

        /// <summary>
        /// 尝试添加
        /// </summary>
        /// <param name="param"></param>
        public void TryAdd(object[] param)
        {
            for (int i = 1; i < param.Length; ++i, ++i)
            {
                TryAdd(param[i - 1], param[i]);
            }
        }

        /// <summary>
        /// 移除参数
        /// </summary>
        /// <param name="name"></param>
        public void Remove(object name)
        {
            if (m_param.ContainsKey(name))
            {
                m_param.Remove(name);
            }
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contain(object name)
        {
            return m_param.ContainsKey(name);
        }

        /// <summary>
        /// 得到参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[object name]
        {
            get
            {
                return m_param.ContainsKey(name) ? m_param[name] : null;
            }
        }

        /// <summary>
        /// 得到Bool
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBool(object name)
        {
            return m_param.ContainsKey(name) ? (bool)m_param[name] : false;
        }

        /// <summary>
        /// 得到Int
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt(object name)
        {
            return m_param.ContainsKey(name) ? (int)m_param[name] : 0;
        }

        /// <summary>
        /// 得到Float
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetFloat(object name)
        {
            return m_param.ContainsKey(name) ? (float)m_param[name] : 0F;
        }

        /// <summary>
        /// 得到String
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(object name)
        {
            return m_param.ContainsKey(name) ? m_param[name].ToString() : null;
        }

        /// <summary>
        /// 得到事件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Action GetEvent(object name)
        {
            return m_param.ContainsKey(name) ? (Action)m_param[name] : null;
        }

        /// <summary>
        /// 得到事件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UnityAction GetUnityAction(object name)
        {
            return m_param.ContainsKey(name) ? (UnityAction)m_param[name] : null;
        }

        /// <summary>
        /// 清理参数
        /// </summary>
        public void Clear()
        {
            m_param.Clear();
        }
        #endregion
    }

}