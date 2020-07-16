using System;
using System.Collections.Generic;

namespace SLG
{
    namespace Event
    {
		public abstract class ScheduleEvent
        {
            /// <summary>
            /// 延迟时间
            /// </summary>
            protected float m_delayTime;

            /// <summary>
            /// 更新间隔时间
            /// </summary>
            protected float m_updateTime;

            /// <summary>
            /// 最大运行时间
            /// </summary>
            protected float m_maxTime;

            /// <summary>
            /// 当前运行时刻时间
            /// </summary>
            protected float m_runTime;

            /// <summary>
            /// 当前真实时刻时间
            /// </summary>
            protected float m_realTime;

            /// <summary>
            /// 结束事件
            /// </summary>
            protected Action m_overAction;

            /// <summary>
            /// 暂停
            /// </summary>
            protected bool m_pause;
            
            /// <summary>
            /// 停止
            /// </summary>
            protected bool m_stop;

            /// <summary>
            /// 更新间隔时间
            /// </summary>
            /// <value>The update time.</value>
            public float updateTime
            {
                get { return m_updateTime; }
                set { m_updateTime = value; }
            }

            /// <summary>
            /// 得到当前运行事件
            /// </summary>
            /// <value>The run time.</value>
            public float runTime
            {
                get { return m_runTime; }
            }

            /// <summary>
            /// 得到当前真实时间
            /// </summary>
            /// <value>The real time.</value>
            public float realTime
            {
                get { return m_realTime; }
            }


            /// <summary>
            /// 暂停
            /// </summary>
            public bool pause
            {
                get { return m_pause; }
            }

            /// <summary>
            /// 播放
            /// </summary>
            public bool play
            {
                get { return !m_pause; }
            }

            /// <summary>
            /// 停止
            /// </summary>
            public bool stop
            {
                get { return m_stop; }
            }

            /// <summary>
            /// 暂停
            /// </summary>
            public void Pause()
            {
                m_pause = true;
            }

            /// <summary>
            /// 继续执行
            /// </summary>
            public void Play()
            {
                m_pause = false;
            }

            /// <summary>
            /// 停止
            /// </summary>
            public void Stop()
            {
                m_pause = true;
                m_stop = true;
            }

            /// <summary>
            /// 更新
            /// </summary>
            /// <param name="delta">Delta.</param>
            public bool Update(float delta)
            {
                if (!m_pause)
                {
                    // 当前运行的真实时间
                    m_realTime += delta;
                    // 延迟时间执行一次
                    if (m_delayTime > 0 && m_runTime == 0 && m_realTime >= m_delayTime)
                    {
                        m_runTime = m_delayTime;
                        RunEvent();
                    }
                    // 是否达到更新时间
                    else if ((m_delayTime == 0 || m_runTime > 0) && m_realTime >= m_runTime + m_updateTime && (m_maxTime <= 0 || m_realTime <= m_maxTime))
                    {
                        m_runTime += m_updateTime;
                        RunEvent();
                    }
                    // 是否达到最大时间
                    if (m_maxTime > 0 && m_realTime >= m_maxTime)
                    {
                        m_pause = true;
                        m_stop = true;
                        if (m_overAction != null)
                        {
                            m_overAction();
                        }
                    }
                }
                return m_stop;
            }

            /// <summary>
            /// 运行事件
            /// </summary>
            protected virtual void RunEvent() {}
        }

        /// <summary>
        /// 无参数
        /// </summary>
        class EventAction : ScheduleEvent
        {
            /// <summary>
            /// 事件
            /// </summary>
            Action m_action;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action">Action.</param>
            /// <param name="delay">Delay.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            public EventAction(Action action, float delay, float updateTime, float maxTime, Action overAction)
            {
                m_action = action;
				m_delayTime = delay;
                m_updateTime = updateTime;
                m_maxTime = maxTime;
                m_overAction = overAction;
                m_pause = false;
                m_stop = false;
            }
            
            /// <summary>
            /// 运行事件
            /// </summary>
            protected override void RunEvent()
            {
                if (m_action != null)
                {
                    m_action();
                }
            }
        }

        /// <summary>
        /// 带有一个参数
        /// </summary>
        class EventActionT<T> : ScheduleEvent
        {
            /// <summary>
            /// 事件
            /// </summary>
            Action<T> m_action;
            
            /// <summary>
            /// 参数t
            /// </summary>
            T m_t;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action">Action.</param>
            /// <param name="delay">Delay.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            /// <param name="t">T.</param>
            public EventActionT(Action<T> action, float delay, float updateTime, float maxTime, Action overAction, T t)
            {
                m_action = action;
				m_delayTime = delay;
                m_updateTime = updateTime;
                m_maxTime = maxTime;
                m_overAction = overAction;
                m_pause = false;
                m_stop = false;
                m_t = t;
            }

            /// <summary>
            /// 运行事件
            /// </summary>
            protected override void RunEvent()
            {
                if (m_action != null)
                {
                    m_action(m_t);
                }
            }
        }

        /// <summary>
        /// 带有两个参数
        /// </summary>
        class EventActionTU<T, U> : ScheduleEvent
        {
            /// <summary>
            /// 事件
            /// </summary>
            Action<T, U> m_action;
            
            /// <summary>
            /// 参数t
            /// </summary>
            T m_t;

            /// <summary>
            /// 参数u
            /// </summary>
            U m_u;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action">Action.</param>
            /// <param name="delay">Delay.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            public EventActionTU(Action<T, U> action, float delay, float updateTime, float maxTime, Action overAction, T t, U u)
            {
                m_action = action;
				m_delayTime = delay;
                m_updateTime = updateTime;
                m_maxTime = maxTime;
                m_overAction = overAction;
                m_pause = false;
                m_stop = false;
                m_t = t;
                m_u = u;
            }
            
            /// <summary>
            /// 运行事件
            /// </summary>
            protected override void RunEvent()
            {
                if (m_action != null)
                {
                    m_action(m_t, m_u);
                }
            }
        }

        /// <summary>
        /// 带有三个参数
        /// </summary>
        class EventActionTUV<T, U, V> : ScheduleEvent
        {
            /// <summary>
            /// 事件
            /// </summary>
            Action<T, U, V> m_action;
            
            /// <summary>
            /// 参数t
            /// </summary>
            T m_t;
            
            /// <summary>
            /// 参数u
            /// </summary>
            U m_u;

            /// <summary>
            /// 参数v
            /// </summary>
            V m_v;
            
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action">Action.</param>
            /// <param name="delay">Delay.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            public EventActionTUV(Action<T, U, V> action, float delay, float updateTime, float maxTime, Action overAction, T t, U u, V v)
            {
                m_action = action;
				m_delayTime = delay;
                m_updateTime = updateTime;
                m_maxTime = maxTime;
                m_overAction = overAction;
                m_pause = false;
                m_stop = false;
                m_t = t;
                m_u = u;
                m_v = v;
            }
            
            /// <summary>
            /// 运行事件
            /// </summary>
            protected override void RunEvent()
            {
                if (m_action != null)
                {
                    m_action(m_t, m_u, m_v);
                }
            }
        }

        /// <summary>
        /// 带有四个参数
        /// </summary>
        class EventActionTUVW<T, U, V, W> : ScheduleEvent
        {
            /// <summary>
            /// 事件
            /// </summary>
            Action<T, U, V, W> m_action;
            
            /// <summary>
            /// 参数t
            /// </summary>
            T m_t;
            
            /// <summary>
            /// 参数u
            /// </summary>
            U m_u;
            
            /// <summary>
            /// 参数v
            /// </summary>
            V m_v;

            /// <summary>
            /// 参数w
            /// </summary>
            W m_w;
            
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action">Action.</param>
            /// <param name="delay">Delay.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="w">The width.</param>
            public EventActionTUVW(Action<T, U, V, W> action, float delay, float updateTime, float maxTime, Action overAction, T t, U u, V v, W w)
            {
                m_action = action;
				m_delayTime = delay;
                m_updateTime = updateTime;
                m_maxTime = maxTime;
                m_overAction = overAction;
                m_pause = false;
                m_stop = false;
                m_t = t;
                m_u = u;
                m_v = v;
                m_w = w;
            }
            
            /// <summary>
            /// 运行事件
            /// </summary>
            protected override void RunEvent()
            {
                if (m_action != null)
                {
                    m_action(m_t, m_u, m_v, m_w);
                }
            }
        }

		public class Schedule : Singleton.Singleton<Schedule>
        {
            /// <summary>
            /// 时刻表
            /// </summary>
            private List<ScheduleEvent> m_schedule;

            /// <summary>
            /// 将要添加的时刻事件表
            /// </summary>
            private List<ScheduleEvent> m_willAdd;

            /// <summary>
            /// 将要移除的时刻事件表
            /// </summary>
            private List<ScheduleEvent> m_willRemove;

            /// <summary>
            /// 暂停
            /// </summary>
            protected bool m_pause;
            
            /// <summary>
            /// 停止
            /// </summary>
            protected bool m_stop;

            /// <summary>
            /// 播放
            /// </summary>
            public bool play
            {
                get { return !m_pause; }
            }
            
            /// <summary>
            /// 暂停
            /// </summary>
            public bool pause
            {
                get { return m_pause; }
            }
            
            /// <summary>
            /// 停止
            /// </summary>
            public bool stop
            {
                get { return m_stop; }
            }

            /// <summary>
            /// 无参构造
            /// </summary>
            public Schedule()
            {
                m_schedule = new List<ScheduleEvent>();
                m_willAdd = new List<ScheduleEvent>();
                m_willRemove = new List<ScheduleEvent>();
                m_pause = false;
                m_stop = false;
            }

            /// <summary>
            /// 启动
            /// </summary>
            public void Start()
            {
                m_pause = false;
                m_stop = false;
            }

            /// <summary>
            /// 更新
            /// </summary>
            public void Update(float delta)
            {
                if (!m_pause)
                {
                    // 添加
                    if (m_willAdd.Count > 0)
                    {
                        foreach (ScheduleEvent scheduleEvent in m_willAdd)
                        {
                            m_schedule.Add(scheduleEvent);
                        }
                        m_willAdd.Clear();
                    }
                    // 移除
                    if (m_willRemove.Count > 0)
                    {
                        //移除无用的
                        foreach (ScheduleEvent scheduleEvent in m_willRemove)
                        {
                            m_schedule.Remove(scheduleEvent);
                        }
                        m_willRemove.Clear();
                    }
                    // 更新
                    foreach (ScheduleEvent scheduleEvent in m_schedule)
                    {
                        if (scheduleEvent.Update(delta))
                        {
                            m_willRemove.Add(scheduleEvent);
                        }
                    }
                }
            }

            /// <summary>
            /// 延迟事件
            /// </summary>
            /// <returns>The once.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            public ScheduleEvent ScheduleOnce(float delayTime, Action action)
            {
                delayTime = Math.Max(delayTime, 0.0001f);
                ScheduleEvent scheduleEvent = new EventAction(action, delayTime, 0, delayTime, null);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

            /// <summary>
            /// 延迟事件
            /// </summary>
            /// <returns>The once.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="t">T.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public ScheduleEvent ScheduleOnce<T>(float delayTime, Action<T> action, T t)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
                ScheduleEvent scheduleEvent = new EventActionT<T>(action, delayTime, 0, delayTime, null, t);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

            /// <summary>
            /// 延迟事件
            /// </summary>
            /// <returns>The once.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            public ScheduleEvent ScheduleOnce<T, U>(float delayTime, Action<T, U> action, T t, U u)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
                ScheduleEvent scheduleEvent = new EventActionTU<T, U>(action, delayTime, 0, delayTime, null, t, u);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

            /// <summary>
            /// 延迟事件
            /// </summary>
            /// <returns>The once.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            public ScheduleEvent ScheduleOnce<T, U, V>(float delayTime, Action<T, U, V> action, T t, U u, V v)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
                ScheduleEvent scheduleEvent = new EventActionTUV<T, U, V>(action, delayTime, 0, delayTime, null, t, u, v);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

            /// <summary>
            /// 延迟事件
            /// </summary>
            /// <returns>The once.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="w">The width.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            /// <typeparam name="W">The 4th type parameter.</typeparam>
            public ScheduleEvent ScheduleOnce<T, U, V, W>(float delayTime, Action<T, U, V, W> action, T t, U u, V v, W w)
            {
                delayTime = Math.Max(delayTime, 0.0001f);
                ScheduleEvent scheduleEvent = new EventActionTUVW<T, U, V, W>(action, delayTime, 0, delayTime, null, t, u, v, w);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 延迟更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="delayTime">Delay time.</param>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			public ScheduleEvent ScheduleUpdate(float delayTime, Action action, float updateTime, float maxTime)
			{
				return ScheduleUpdate(delayTime, action, updateTime, maxTime, null);
			}

            /// <summary>
            /// 延迟更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            public ScheduleEvent ScheduleUpdate(float delayTime, Action action, float updateTime, float maxTime, Action overAction)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
				updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventAction(action, delayTime, updateTime, maxTime, overAction);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 延迟更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="delayTime">Delay time.</param>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			public ScheduleEvent ScheduleUpdate<T>(float delayTime, Action<T> action, float updateTime, float maxTime, T t)
			{
				return ScheduleUpdate<T>(delayTime, action, updateTime, maxTime, t, null);
			}

            /// <summary>
            /// 延迟更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T>(float delayTime, Action<T> action, float updateTime, float maxTime, T t, Action overAction)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
				updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionT<T>(action, delayTime, updateTime, maxTime, overAction, t);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 延迟更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="delayTime">Delay time.</param>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <param name="u">U.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			/// <typeparam name="U">The 2nd type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T, U>(float delayTime, Action<T, U> action, float updateTime, float maxTime, T t, U u)
			{
				return ScheduleUpdate<T, U>(delayTime, action, updateTime, maxTime, t, u, null);
			}

            /// <summary>
            /// 延迟更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T, U>(float delayTime, Action<T, U> action, float updateTime, float maxTime, T t, U u, Action overAction)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
                updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionTU<T, U>(action, delayTime, updateTime, maxTime, overAction, t, u);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 延迟更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="delayTime">Delay time.</param>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <param name="u">U.</param>
			/// <param name="v">V.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			/// <typeparam name="U">The 2nd type parameter.</typeparam>
			/// <typeparam name="V">The 3rd type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T, U, V>(float delayTime, Action<T, U, V> action, float updateTime, float maxTime, T t, U u, V v)
			{
				return ScheduleUpdate<T, U, V>(delayTime, action, updateTime, maxTime, t, u, v, null);
			}

            /// <summary>
            /// 延迟更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T, U, V>(float delayTime, Action<T, U, V> action, float updateTime, float maxTime, T t, U u, V v, Action overAction)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
                updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionTUV<T, U, V>(action, delayTime, updateTime, maxTime, overAction, t, u, v);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 延迟更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="delayTime">Delay time.</param>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <param name="u">U.</param>
			/// <param name="v">V.</param>
			/// <param name="w">The width.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			/// <typeparam name="U">The 2nd type parameter.</typeparam>
			/// <typeparam name="V">The 3rd type parameter.</typeparam>
			/// <typeparam name="W">The 4th type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T, U, V, W>(float delayTime, Action<T, U, V, W> action, float updateTime, float maxTime, T t, U u, V v, W w)
			{
				return ScheduleUpdate<T, U, V, W>(delayTime, action, updateTime, maxTime, t, u, v, w, null);
			}

            /// <summary>
            /// 延迟更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="delayTime">Delay time.</param>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="w">The width.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            /// <typeparam name="W">The 4th type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T, U, V, W>(float delayTime, Action<T, U, V, W> action, float updateTime, float maxTime, T t, U u, V v, W w, Action overAction)
            {
				delayTime = Math.Max(delayTime, 0.0001f);
				updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionTUVW<T, U, V, W>(action, delayTime, updateTime, maxTime, overAction, t, u, v, w);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			public ScheduleEvent ScheduleUpdate(Action action, float updateTime, float maxTime)
			{
				return ScheduleUpdate(action, updateTime, maxTime, null);
			}

            /// <summary>
            /// 更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="overAction">Over action.</param>
            public ScheduleEvent ScheduleUpdate(Action action, float updateTime, float maxTime, Action overAction)
            {
                updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventAction(action, 0, updateTime, maxTime, overAction);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T>(Action<T> action, float updateTime, float maxTime, T t)
			{
				return ScheduleUpdate<T>(action, updateTime, maxTime, t, null);
			}

            /// <summary>
            /// 更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T>(Action<T> action, float updateTime, float maxTime, T t, Action overAction)
            {
                updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionT<T>(action, 0, updateTime, maxTime, overAction, t);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <param name="u">U.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			/// <typeparam name="U">The 2nd type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T, U>(Action<T, U> action, float updateTime, float maxTime, T t, U u)
			{
				return ScheduleUpdate<T, U>(action, updateTime, maxTime, t, u, null);
			}

            /// <summary>
            /// 更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T, U>(Action<T, U> action, float updateTime, float maxTime, T t, U u, Action overAction)
            {
                updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionTU<T, U>(action, 0, updateTime, maxTime, overAction, t, u);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <param name="u">U.</param>
			/// <param name="v">V.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			/// <typeparam name="U">The 2nd type parameter.</typeparam>
			/// <typeparam name="V">The 3rd type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T, U, V>(Action<T, U, V> action, float updateTime, float maxTime, T t, U u, V v)
			{
				return ScheduleUpdate<T, U, V>(action, updateTime, maxTime, t, u, v, null);
			}

            /// <summary>
            /// 更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T, U, V>(Action<T, U, V> action, float updateTime, float maxTime, T t, U u, V v, Action overAction)
            {
                updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionTUV<T, U, V>(action, 0, updateTime, maxTime, overAction, t, u, v);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

			/// <summary>
			/// 更新事件
			/// </summary>
			/// <returns>The update.</returns>
			/// <param name="action">Action.</param>
			/// <param name="updateTime">Update time.</param>
			/// <param name="maxTime">Max time.</param>
			/// <param name="t">T.</param>
			/// <param name="u">U.</param>
			/// <param name="v">V.</param>
			/// <param name="w">The width.</param>
			/// <typeparam name="T">The 1st type parameter.</typeparam>
			/// <typeparam name="U">The 2nd type parameter.</typeparam>
			/// <typeparam name="V">The 3rd type parameter.</typeparam>
			/// <typeparam name="W">The 4th type parameter.</typeparam>
			public ScheduleEvent ScheduleUpdate<T, U, V, W>(Action<T, U, V, W> action, float updateTime, float maxTime, T t, U u, V v, W w)
			{
				return ScheduleUpdate<T, U, V, W>(action, updateTime, maxTime, t, u, v, w, null);
			}

            /// <summary>
            /// 更新事件
            /// </summary>
            /// <returns>The update.</returns>
            /// <param name="action">Action.</param>
            /// <param name="updateTime">Update time.</param>
            /// <param name="maxTime">Max time.</param>
            /// <param name="t">T.</param>
            /// <param name="u">U.</param>
            /// <param name="v">V.</param>
            /// <param name="w">The width.</param>
            /// <param name="overAction">Over action.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            /// <typeparam name="U">The 2nd type parameter.</typeparam>
            /// <typeparam name="V">The 3rd type parameter.</typeparam>
            /// <typeparam name="W">The 4th type parameter.</typeparam>
            public ScheduleEvent ScheduleUpdate<T, U, V, W>(Action<T, U, V, W> action, float updateTime, float maxTime, T t, U u, V v, W w, Action overAction)
            {
				updateTime = Math.Max(updateTime, 0);
                maxTime = Math.Max(maxTime, 0);
                ScheduleEvent scheduleEvent = new EventActionTUVW<T, U, V, W>(action, 0, updateTime, maxTime, overAction, t, u, v, w);
                m_willAdd.Add(scheduleEvent);
                return scheduleEvent;
            }

            /// <summary>
            /// 停止某个事件
            /// </summary>
            /// <param name="scheduleEvent">Schedule event.</param>
            public void UnSchedule(ScheduleEvent scheduleEvent)
            {
                if (scheduleEvent != null)
                {
                    scheduleEvent.Stop();
                    m_willRemove.Add(scheduleEvent);
                }
            }

            /// <summary>
            /// 停止所有
            /// </summary>
            public void UnSchedule()
            {
                m_schedule.Clear();
                m_willAdd.Clear();
                m_willRemove.Clear();
            }

            /// <summary>
            /// 暂停指定定时器
            /// </summary>
            /// <param name="scheduleEvent">Schedule event.</param>
            public void Pause(ScheduleEvent scheduleEvent)
            {
                if (scheduleEvent != null)
                {
                    scheduleEvent.Pause();
                }
            }

            /// <summary>
            /// 暂停
            /// </summary>
            public void Pause()
            {
                m_pause = true;
            }

            /// <summary>
            /// 播放指定定时器
            /// </summary>
            /// <param name="scheduleEvent">Schedule event.</param>
            public void Play(ScheduleEvent scheduleEvent)
            {
                if (scheduleEvent != null)
                {
                    scheduleEvent.Play();
                }
            }
            
            /// <summary>
            /// 继续执行
            /// </summary>
            public void Play()
            {
                m_pause = false;
            }
        }
    }
}