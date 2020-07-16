using System;
using System.Collections.Generic;

namespace SLG
{
    namespace Pool
    {
        public class Pool<T> : IPool where T : new()
        {
            /// <summary>
            /// 池队列
            /// </summary>
            private Queue<T> m_pool = null;
            
            /// <summary>
            /// 得到池名
            /// </summary>
            public string name => typeof(T).Name;

            /// <summary>
            /// 类型
            /// </summary>
            public Type poolType => typeof(T);

            /// <summary>
            /// 得到池的大小
            /// </summary>
            public int Count => m_pool.Count;

            /// <summary>
            /// 创建当前池
            /// </summary>
            /// <param name="capacity"></param>
            public void Create(int capacity = 1<<6)
            {
                m_pool = new Queue<T>(capacity);
            }

            /// <summary>
            /// 从池中获取
            /// </summary>
            /// <returns></returns>
            public T Get()
            {
                T t;
                if (Count > 0)
                {
                    t = m_pool.Dequeue();
                }
                else
                {
                    t = new T();
                }
                return t;
            }

            public void Release(T t)
            {
                if (null != t)
                {
                    if (!m_pool.Contains(t))
                    {
                        m_pool.Enqueue(t);
                    }
                }
            }

            public T[] ToArray()
            {
                return m_pool.ToArray();
            }

            /// <summary>
            /// 清楚当前池
            /// </summary>
            public void Clear()
            {
                m_pool.Clear();
            }
        }
    }
}