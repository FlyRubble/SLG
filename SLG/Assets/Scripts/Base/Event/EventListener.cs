using System;
using System.Collections.Generic;

namespace SLG
{
    using Singleton;
    namespace Event
    {
        public class EventListener : EventListener<string> { }

        public class EventListener<Type> : Singleton<EventListener<Type>>
        {
            /// <summary>
            /// 事件字典表
            /// </summary>
            private Dictionary<Type, List<Delegate>> m_dic;

            /// <summary>
            /// 得到事件表大小
            /// </summary>
            public int count
            {
                get { return m_dic.Count; }
            }

            /// <summary>
            /// 无参构造
            /// </summary>
            public EventListener()
            {
                m_dic = new Dictionary<Type, List<Delegate>>();
            }

            /// <summary>
            /// 得到事件表
            /// </summary>
            /// <param name="Type">Event type.</param>
            private List<Delegate> GetEventList(Type Type)
            {
                List<Delegate> list;
                if (m_dic.ContainsKey(Type))
                {
                    list = m_dic[Type];
                }
                else
                {
                    list = new List<Delegate>();
                    m_dic.Add(Type, list);
                }
                return list;
            }
            
            /// <summary>
            /// 添加事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            public void AddEvent(Type Type, Action action)
            {
                List<Delegate> list = GetEventList(Type);
                if (!list.Contains(action))
                {
                    list.Add(action);
                }
            }

            /// <summary>
            /// 添加事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public void AddEvent<T>(Type Type, Action<T> action)
            {
                List<Delegate> list = GetEventList(Type);
                if (!list.Contains(action))
                {
                    list.Add(action);
                }
            }

            /// <summary>
            /// 添加事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            public void AddEvent<T, U>(Type Type, Action<T, U> action)
            {
                List<Delegate> list = GetEventList(Type);
                if (!list.Contains(action))
                {
                    list.Add(action);
                }
            }

            /// <summary>
            /// 添加事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            public void AddEvent<T, U, V>(Type Type, Action<T, U, V> action)
            {
                List<Delegate> list = GetEventList(Type);
                if (!list.Contains(action))
                {
                    list.Add(action);
                }
            }

            /// <summary>
            /// 添加事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            /// <typeparam name="W">The 4th type parameter.</typeparam>
            public void AddEvent<T, U, V, W>(Type Type, Action<T, U, V, W> action)
            {
                List<Delegate> list = GetEventList(Type);
                if (!list.Contains(action))
                {
                    list.Add(action);
                }
            }

            /// <summary>
            /// 移除事件
            /// </summary>
            /// <returns><c>true</c>, if event was removed, <c>false</c> otherwise.</returns>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            public bool RemoveEvent(Type Type, Action action)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Remove(action);
            }

            /// <summary>
            /// 移除事件
            /// </summary>
            /// <returns><c>true</c>, if event was removed, <c>false</c> otherwise.</returns>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public bool RemoveEvent<T>(Type Type, Action<T> action)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Remove(action);
            }

            /// <summary>
            /// 移除事件
            /// </summary>
            /// <returns><c>true</c>, if event was removed, <c>false</c> otherwise.</returns>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            public bool RemoveEvent<T, U>(Type Type, Action<T, U> action)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Remove(action);
            }

            /// <summary>
            /// 移除事件
            /// </summary>
            /// <returns><c>true</c>, if event was removed, <c>false</c> otherwise.</returns>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            public bool RemoveEvent<T, U, V>(Type Type, Action<T, U, V> action)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Remove(action);
            }

            /// <summary>
            /// 移除事件
            /// </summary>
            /// <returns><c>true</c>, if event was removed, <c>false</c> otherwise.</returns>
            /// <param name="Type">Event type.</param>
            /// <param name="action">Action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            /// <typeparam name="W">The 4th type parameter.</typeparam>
            public bool RemoveEvent<T, U, V, W>(Type Type, Action<T, U, V, W> action)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Remove(action);
            }

            /// <summary>
            /// 执行事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            public void OnEvent(Type Type)
            {
                List<Delegate> list = GetEventList(Type);
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i] == null)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                    ((Action)list[i])();
                }
            }

            /// <summary>
            /// 执行事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="t">T.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public void OnEvent<T>(Type Type, T t)
            {
                List<Delegate> list = GetEventList(Type);
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i] == null)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                    ((Action<T>)list[i])(t);
                }
            }

            /// <summary>
            /// 执行事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            public void OnEvent<T, U>(Type Type, T t, U u)
            {
                List<Delegate> list = GetEventList(Type);
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i] == null)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                    ((Action<T, U>)list[i])(t, u);
                }
            }

            /// <summary>
            /// 执行事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            public void OnEvent<T, U, V>(Type Type, T t, U u, V v)
            {
                List<Delegate> list = GetEventList(Type);
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i] == null)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                    ((Action<T, U, V>)list[i])(t, u, v);
                }
            }

            /// <summary>
            /// 执行事件
            /// </summary>
            /// <param name="Type">Event type.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="w">The width.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            /// <typeparam name="W">The 4th type parameter.</typeparam>
            public void OnEvent<T, U, V, W>(Type Type, T t, U u, V v, W w)
            {
                List<Delegate> list = GetEventList(Type);
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i] == null)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                    ((Action<T, U, V, W>)list[i])(t, u, v, w);
                }
            }

            /// <summary>
            /// 是否包含某个事件
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="action"></param>
            /// <returns></returns>
            public bool Contains(Type Type, Delegate action)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Contains(action);
            }

            /// <summary>
            /// 得到整表大小
            /// </summary>
            /// <returns>表大小</returns>
            public int Count()
            {
                return m_dic.Count;
            }

            /// <summary>
            /// 得到指定类型事件表大小
            /// </summary>
            /// <param name="Type"></param>
            /// <returns></returns>
            public int Count(Type Type)
            {
                List<Delegate> list = GetEventList(Type);
                return list.Count;
            }

            /// <summary>
            /// 清理事件表
            /// </summary>
            /// <param name="Type"></param>
            public void Clear(Type Type)
            {
                List<Delegate> list = GetEventList(Type);
                list.Clear();
            }

            /// <summary>
            /// 清理
            /// </summary>
            public void Clear()
            {
                m_dic.Clear();
            }
        }
    }
}