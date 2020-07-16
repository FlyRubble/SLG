using System.Collections.Generic;
using UnityEngine;
using SLG.IO;
using SLG;

/// <summary>
/// App
/// </summary>
public sealed class App
{
    #region Variable
    /// <summary>
    /// 产品名
    /// </summary>
    private static string m_productName = string.Empty;

    /// <summary>
    /// 游戏App版本[App版本、显示版本、资源版本一致]
    /// </summary>
    private static string m_version = "1.0.0";

    /// <summary>
    /// 内部版本
    /// </summary>
    private static string m_innerVersion = "1.0.0";

    /// <summary>
    /// 资源版本
    /// </summary>
    private static string m_assetVersion = "1.0.0";

    /// <summary>
    /// 登陆地址
    /// </summary>
    private static string m_loginUrl = string.Empty;

    /// <summary>
    /// Cdn
    /// </summary>
    private static string m_cdn = string.Empty;

    /// <summary>
    /// 是否开启引导
    /// </summary>
    private static bool m_openGuide = true;

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    private static bool m_openUpdate = true;

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    private static bool m_unlockAllFunction = false;

    /// <summary>
    /// 是否开启日志
    /// </summary>
    private static bool m_log = false;

    /// <summary>
    /// 日志的等级
    /// </summary>
    private static Debugger.LogLevel m_logLevel = Debugger.LogLevel.None;

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    private static bool m_webLog = false;

    /// <summary>
    /// WebLog白名单
    /// </summary>
    private static List<string> m_webLogIp = new List<string>();

    /// <summary>
    /// [安卓]平台标签
    /// </summary>
    private static string m_androidPlatformName = string.Empty;

    /// <summary>
    /// [苹果]平台标签
    /// </summary>
    private static string m_iOSPlatformName = string.Empty;

    /// <summary>
    /// [桌面]平台标签
    /// </summary>
    private static string m_defaultPlatformName = string.Empty;

    /// <summary>
    /// 资源清单
    /// </summary>
    private static ManifestConfig m_manifest = new ManifestConfig();

    /// <summary>
    /// 资源清单映射表
    /// </summary>
    private static ManifestMappingConfig m_manifestMapping = new ManifestMappingConfig(new Dictionary<string, string>() { { "res/lua/main.bytes", "res/lua/main.unity3d" } });

    /// <summary>
    /// 新版本下载地址
    /// </summary>
    private static string m_newVersionDownloadUrl = string.Empty;
    #endregion

    #region Property
    /// <summary>
    /// 产品名
    /// </summary>
    public static string productName
    {
        get { return m_productName; }
    }

    /// <summary>
    /// 游戏App版本
    /// </summary>
    public static string version
    {
        get { return m_version; }
    }

    /// <summary>
    /// 内部版本
    /// </summary>
    public static string innerVersion
    {
        get { return m_innerVersion; }
    }

    /// <summary>
    /// 资源版本
    /// </summary>
    public static string assetVersion
    {
        get { return m_assetVersion; }
    }

    /// <summary>
    /// 登陆地址
    /// </summary>
    /// <value>The login URL.</value>
    public static string loginUrl
    {
        get { return m_loginUrl; }
    }

    /// <summary>
    /// CDN
    /// </summary>
    public static string cdn
    {
        get { return m_cdn; }
    }

    /// <summary>
    /// 是否开启引导
    /// </summary>
    public static bool openGuide
    {
        get { return m_openGuide; }
    }

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    public static bool openUpdate
    {
        get { return m_openUpdate; }
    }

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    public static bool unlockAllFunction
    {
        get { return m_unlockAllFunction; }
    }

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public static bool log
    {
        get { return m_log; }
    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public static Debugger.LogLevel logLevel
    {
        get { return m_logLevel; }
    }

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    public static bool webLog
    {
        get { return m_webLog; }
    }

    /// <summary>
    /// WebLog白名单
    /// </summary>
    public static List<string> webLogIp
    {
        get { return m_webLogIp; }
    }

    /// <summary>
    /// 平台标签
    /// </summary>
    public static string platform
    {
        get
        {
#if UNITY_ANDROID
            return m_androidPlatformName;
#elif UNITY_IOS
            return m_iOSPlatformName;
#else
            return m_defaultPlatformName;
#endif
        }
    }

    /// <summary>
    /// 资源清单文件
    /// </summary>
    public static ManifestConfig manifest
    {
        get { return m_manifest; }
        set { m_manifest = value; }
    }

    /// <summary>
    /// 资源清单映射表
    /// </summary>
    public static ManifestMappingConfig manifestMapping
    {
        get { return m_manifestMapping; }
        set { m_manifestMapping = value; }
    }

    /// <summary>
    /// 新版本下载地址
    /// </summary>
    public static string newVersionDownloadUrl
    {
        get { return m_newVersionDownloadUrl; }
    }

    /// <summary>
    /// 得到下一步状态
    /// </summary>
    public static string nextState
    {
        get; set;
    }

    /// <summary>
    /// 是否有网络
    /// </summary>
    public static bool internetReachability => Application.internetReachability != NetworkReachability.NotReachable;

    /// <summary>
    /// 是否是流量网络
    /// </summary>
    public static bool dataNetwork => Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;

    /// <summary>
    /// 是否是Wifi网络
    /// </summary>
    public static bool wifi => Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
    #endregion

    #region Function
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init(List<DictPair> data)
    {
        for (int i = 0; i < data.Count; ++i)
        {
            // 产品名
            if (data[i].key.Equals(Const.PRODUCT_NAME))
            {
                m_productName = data[i].value;
            }
            // 游戏App版本
            else if (data[i].key.Equals(Const.VERSION))
            {
                m_version = data[i].value;
            }
            // 内部版本
            else if (data[i].key.Equals(Const.INNERVERSION))
            {
                m_innerVersion = data[i].value;
            }
            // 资源版本
            else if (data[i].key.Equals(Const.ASSETVERSION))
            {
                m_assetVersion = data[i].value;
            }
            // 登陆地址
            else if (data[i].key.Equals(Const.LOGIN_URL))
            {
                m_loginUrl = data[i].value;
            }
            // Cdn
            else if (data[i].key.Equals(Const.CDN))
            {
                m_cdn = data[i].value;
            }
            // 是否开启引导
            else if (data[i].key.Equals(Const.OPEN_GUIDE))
            {
                m_openGuide = bool.Parse(data[i].value);
            }
            // 是否开启更新功能
            else if (data[i].key.Equals(Const.OPEN_UPDATE))
            {
                m_openUpdate = bool.Parse(data[i].value);
            }
            // 是否功能全解锁
            else if (data[i].key.Equals(Const.UNLOCK_ALL_FUNCTION))
            {
                m_unlockAllFunction = bool.Parse(data[i].value);
            }
            // 是否开启日志
            else if (data[i].key.Equals(Const.LOG))
            {
                m_log = bool.Parse(data[i].value);
            }
            // 日志等级
            else if (data[i].key.Equals(Const.LOGLEVEL))
            {
                m_logLevel = (Debugger.LogLevel)int.Parse(data[i].value);
            }
            // 是否开启Web日志
            else if (data[i].key.Equals(Const.WEB_LOG))
            {
                m_webLog = bool.Parse(data[i].value);
            }
            // WebLog白名单
            else if (data[i].key.Equals(Const.WEB_LOG))
            {
                m_webLogIp.Clear();
                string[] array = data[i].value.Split(',', ';', '|');
                foreach (var ip in array)
                {
                    if (!string.IsNullOrEmpty(ip))
                    {
                        m_webLogIp.Add(ip);
                    }
                }
            }
            // [安卓]平台标签
            else if (data[i].key.Equals(Const.ANDROID_PLATFORM_NAME))
            {
                m_androidPlatformName = data[i].value;
            }
            // [苹果]平台标签
            else if (data[i].key.Equals(Const.IOS_PLATFORM_NAME))
            {
                m_iOSPlatformName = data[i].value;
            }
            // [桌面]平台标签
            else if (data[i].key.Equals(Const.DEFAULT_PLATFORM_NAME))
            {
                m_defaultPlatformName = data[i].value;
            }
            // 新版本下载地址
            else if (data[i].key.Equals(Const.NEWVERSIONDOWNLOADURL))
            {
                m_newVersionDownloadUrl = data[i].value;
            }
        }
    }

    /// <summary>
    /// 更新使用
    /// </summary>
    /// <param name="text"></param>
    public static void Update(string text)
    {
        Dictionary<string, object> remoteVersion = SLG.JsonFx.JsonReader.Deserialize<Dictionary<string, object>>(text);
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
            Debugger.logLevel = App.logLevel;
        }
    }
    #endregion
}
