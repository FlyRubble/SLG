using System;
using System.Collections.Generic;

namespace SLG
{
    using Singleton;
    namespace Pool
    {
        public class PoolManager : Singleton<PoolManager>
        {
            /// <summary>
            /// 总池
            /// </summary>
            private Dictionary<string, IPool> m_pools = null;

            /// <summary>
            /// 得到总池的大小
            /// </summary>
            public int Count => m_pools.Count;

            /// <summary>
            /// 总池数据
            /// </summary>
            public Dictionary<string, IPool> pools => m_pools;

            /// <summary>
            /// 创建当前总池
            /// </summary>
            /// <param name="capacity"></param>
            public void Create(int capacity = 1<<6)
            {
                m_pools = new Dictionary<string, IPool>(capacity);
            }

            /// <summary>
            /// 从池中获取
            /// </summary>
            /// <returns></returns>
            public T Get<T>() where T : IPool, new()
            {
                T t;
                string typeName = typeof(T).ToString();
                if (m_pools.ContainsKey(typeName))
                {
                    t = (T)m_pools[typeName];
                }
                else
                {
                    t = new T();
                    m_pools.Add(typeName, t);
                    t.Create();
                }
                return t;
            }

            /// <summary>
            /// 从池中获取
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="typeName"></param>
            /// <returns></returns>
            public T Get<T>(string typeName) where T : IPool, new()
            {
                T t;
                typeName = string.IsNullOrEmpty(typeName) ? typeof(T).ToString() : typeName;
                if (m_pools.ContainsKey(typeName))
                {
                    t = (T)m_pools[typeName];
                }
                else
                {
                    t = new T();
                    m_pools.Add(typeName, t);
                    t.Create();
                }
                return t;
            }

            /// <summary>
            /// 从池中移除
            /// </summary>
            /// <param name="t"></param>
            public void Remove(IPool t)
            {
                if (null != t)
                {
                    string typeName = t.GetType().ToString();
                    if (m_pools.ContainsKey(typeName))
                    {
                        m_pools.Remove(typeName);
                    }
                }
            }

            /// <summary>
            /// 从池中移除
            /// </summary>
            /// <typeparam name="T"></typeparam>
            public void Remove<T>() where T : IPool, new()
            {
                string typeName = typeof(T).ToString();
                if (m_pools.ContainsKey(typeName))
                {
                    m_pools.Remove(typeName);
                }
            }

            /// <summary>
            /// 移除
            /// </summary>
            /// <param name="typeName"></param>
            public void Remove(string typeName)
            {
                if (!string.IsNullOrEmpty(typeName) && m_pools.ContainsKey(typeName))
                {
                    m_pools.Remove(typeName);
                }
            }

            /// <summary>
            /// 从池里移除
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="typeName"></param>
            public void Remove<T>(string typeName) where T : IPool, new()
            {
                typeName = string.IsNullOrEmpty(typeName) ? typeof(T).ToString() : typeName;
                if (m_pools.ContainsKey(typeName))
                {
                    m_pools.Remove(typeName);
                }
            }

            /// <summary>
            /// 清楚当前池
            /// </summary>
            public void Clear()
            {
                foreach (var pool in m_pools.Values)
                {
                    pool.Clear();
                }
                m_pools.Clear();
            }
        }
    }
}