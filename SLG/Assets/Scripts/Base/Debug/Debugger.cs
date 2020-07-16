using System;
using System.Diagnostics;

namespace SLG
{
	/// <summary>
	/// 调试器
	/// </summary>
	public class Debugger
	{
        public enum LogLevel
        {
            Log = 0,
            Warning = 1,
            Exception = 2,
            Error = 3,
            None = 4,
        }

		#region Variable
		/// <summary>
		/// 是否显示日志
		/// </summary>
		static bool m_logEnabled = true;

		/// <summary>
		/// 是否开启Web日志
		/// </summary>
		static bool m_webLogEnabled = false;

        /// <summary>
        /// 日志的等级
        /// </summary>
        static LogLevel m_logLevel = LogLevel.None;
        #endregion

        #region Property
        /// <summary>
        /// 是否需要日志
        /// </summary>
        public static bool logEnabled
        {
			get
            {
                return m_logEnabled && UnityEngine.Debug.unityLogger.logEnabled;
            }
			set
            {
                m_logEnabled = value;
                UnityEngine.Debug.unityLogger.logEnabled = m_logEnabled;
            }
		}

        /// <summary>
        /// 是否需要Web日志
        /// </summary>
        public static bool webLogEnabled
        {
			get { return m_webLogEnabled; }
			set { m_webLogEnabled = value; }
		}

        /// <summary>
        /// 日志的等级
        /// </summary>
        public static LogLevel logLevel
        {
            get { return m_logLevel; }
            set { m_logLevel = value; }
        }
        #endregion

        #region Function
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message">Message.</param>
        public static void Log(object message)
        {
            if (m_logLevel <= LogLevel.Log)
            {
                UnityEngine.Debug.Log(message);
                WebDebug(LogLevel.Log, message);
            }
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogFormat(string format, params object[] args)
        {
            if (m_logLevel <= LogLevel.Log)
            {
                UnityEngine.Debug.LogFormat(format, args);
                WebDebugFormat(LogLevel.Log, format, args);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">Message.</param>
        public static void LogError(object message)
        {
            if (m_logLevel <= LogLevel.Error)
            {
                UnityEngine.Debug.LogError(message);
                WebDebug(LogLevel.Error, message);
            }
		}

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (m_logLevel <= LogLevel.Error)
            {
                UnityEngine.Debug.LogErrorFormat(format, args);
                WebDebugFormat(LogLevel.Error, format, args);
            }
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="exception">Exception.</param>
        public static void LogException(Exception exception)
        {
            if (m_logLevel <= LogLevel.Exception)
            {
                UnityEngine.Debug.LogException(exception);
                WebDebug(LogLevel.Exception, exception.Message);
            }
		}

		/// <summary>
		/// 警告日志
		/// </summary>
		/// <param name="message">Message.</param>
		public static void LogWarning(object message)
        {
            if (m_logLevel <= LogLevel.Warning)
            {
                UnityEngine.Debug.LogWarning(message);
                WebDebug(LogLevel.Warning, message);
            }
		}

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogWarningFormat(string format, params object[] args)
        {
            if (m_logLevel <= LogLevel.Warning)
            {
                UnityEngine.Debug.LogWarningFormat(format, args);
                WebDebugFormat(LogLevel.Exception, format, args);
            }
        }

        /// <summary>
        /// Web日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        static void WebDebugFormat(LogLevel logLevel, string format, params object[] args)
        {
            WebDebug(logLevel, string.Format(format, args));
        }

        /// <summary>
        /// Web日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        static void WebDebug(LogLevel logLevel, object message)
        {
            if (m_webLogEnabled)
            {
                StackTrace st = new StackTrace(1, true);
                string methodName = st.GetFrame(0).GetMethod().Name;
                string value = methodName + message + st.ToString();
            }
        }
        #endregion
    }
}