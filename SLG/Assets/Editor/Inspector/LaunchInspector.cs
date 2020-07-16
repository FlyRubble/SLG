using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace SLG
{
    using JsonFx;

    /// <summary>
    /// 启动监视器
    /// </summary>
    [CustomEditor(typeof(Launch))]
    public class LaunchInspector : Editor
    {
        enum ChangeType
        {
            ProductName,
            BundleIdentifier,
            Version,
            BundleVersionCode,
            ScriptingDefineSymbols
        }

        enum BuildType
        {
            None,
            RebuildForApp,
            BuildForApp,
            RebuildForUpdate,
            BuildForUpdate
        }

        protected class VersionInfo
        {
            /// <summary>
            /// 选中索引
            /// </summary>
            private int m_selectIndex = 0;

            /// <summary>
            /// 数据
            /// </summary>
            private List<Dictionary<string, object>> m_data = new List<Dictionary<string, object>>() {
                new Dictionary<string, object>(){
                    { Const.NAME, @"自定义" },
                    { Const.PRODUCT_NAME, "" },
                    { Const.BUNDLE_IDENTIFIER, "" },
                    { Const.VERSION, "1.0.0" },
                    { Const.INNERVERSION, "1.0.0" },
                    { Const.ASSETVERSION, "1.0.0" },
                    { Const.BUNDLE_VERSION_CODE, 1 },
                    { Const.SCRIPTING_DEFINE_SYMBOLS, "" },
                    { Const.LOGIN_URL, "" },
                    { Const.CDN, "" },
                    { Const.OPEN_GUIDE, true },
                    { Const.OPEN_UPDATE, true },
                    { Const.UNLOCK_ALL_FUNCTION, false },
                    { Const.LOG, false },
                    { Const.LOGLEVEL, 0 },
                    { Const.WEB_LOG, false },
                    { Const.WEB_LOG_IP, "" },
                    { Const.ANDROID_PLATFORM_NAME, "Android"},
                    { Const.IOS_PLATFORM_NAME, "iOS"},
                    { Const.DEFAULT_PLATFORM_NAME, "PC"},
                },
            };

            /// <summary>
            /// 索引
            /// </summary>
            public int selectIndex
            {
                get { return m_selectIndex; }
                set { m_selectIndex = value; }
            }

            /// <summary>
            /// 得到数据
            /// </summary>
            public List<Dictionary<string, object>> data
            {
                get { return m_data; }
                set { m_data = value; }
            }

            /// <summary>
            /// 得到选中的数据
            /// </summary>
            public Dictionary<string, object> select => m_data[m_selectIndex];
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        private static VersionInfo m_info = null;

        /// <summary>
        /// 名字表
        /// </summary>
        private List<string> m_nameList = new List<string>();

        /// <summary>
        /// 是否添加新的服务器选项
        /// </summary>
        private bool m_addNew = false;

        /// <summary>
        /// 新服务器名
        /// </summary>
        private string m_newServerName = string.Empty;

        /// <summary>
        /// 是否需要保存
        /// </summary>
        private bool m_isSave = false;

        /// <summary>
        /// 执行的方式
        /// </summary>
        private static BuildType m_buildType = BuildType.None;

        /// <summary>
        /// 更新名字列表
        /// </summary>
        public void UpdateNameList()
        {
            m_nameList.Clear();
            foreach (var data in m_info.data)
            {
                m_nameList.Add(data["name"].ToString());
            }
        }

        /// <summary>
        /// OnEnable
        /// </summary>
        private void OnEnable()
        {
            // 读取配置数据
            if (!File.Exists(PathUtil.versionConfigPath))
            {
                m_info = new VersionInfo();
                m_isSave = true;
            }
            else
            {
                m_info = JsonReader.Deserialize<VersionInfo>(File.ReadAllText(PathUtil.versionConfigPath));
            }

            // 更新名字列表
            UpdateNameList();
        }

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUI.BeginChangeCheck();
            {
                serializedObject.Update();
                // 模式
                string value = m_info.select[Const.SCRIPTING_DEFINE_SYMBOLS].ToString();
                var index = value.Contains("AB_MODE") ? 1 : 0;
                var selected = GUILayout.Toolbar(index, new string[] { "编辑器模式", "高级AB模式" });
                if (index != selected)
                {
                    m_isSave = true;
                    m_info.select[Const.SCRIPTING_DEFINE_SYMBOLS] = GetScriptingDefineSymbolsp(value, selected);
                    ChangeSettings(ChangeType.ScriptingDefineSymbols);
                }
                // 产品名字
                value = EditorGUILayout.TextField("Product Name", m_info.select[Const.PRODUCT_NAME].ToString());
                if (value != m_info.select[Const.PRODUCT_NAME].ToString())
                {
                    m_isSave = true;
                    m_info.select[Const.PRODUCT_NAME] = value;
                    ChangeSettings(ChangeType.ProductName);
                }
                // 包名
                value = EditorGUILayout.TextField("Bundle Identifier", m_info.select[Const.BUNDLE_IDENTIFIER].ToString());
                if (value != m_info.select[Const.BUNDLE_IDENTIFIER].ToString())
                {
                    m_isSave = true;
                    m_info.select[Const.BUNDLE_IDENTIFIER] = value;
                    ChangeSettings(ChangeType.BundleIdentifier);
                }
                // 版本
                value = EditorGUILayout.TextField("Version*", m_info.select[Const.VERSION].ToString());
                if (value != m_info.select[Const.VERSION].ToString())
                {
                    m_isSave = true;
                    m_info.select[Const.VERSION] = value;
                    ChangeSettings(ChangeType.Version);
                }

                // 资源版本
                if (selected != 0)
                {
                    value = EditorGUILayout.TextField("Inner Version*", m_info.select[Const.INNERVERSION].ToString());
                    if (value != m_info.select[Const.INNERVERSION].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.INNERVERSION] = value;
                    }
                }

                // 资源版本
                if (selected != 0)
                {
                    value = EditorGUILayout.TextField("Asset Version*", m_info.select[Const.ASSETVERSION].ToString());
                    if (value != m_info.select[Const.ASSETVERSION].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.ASSETVERSION] = value;
                    }
                }

                // 版本Code
#if UNITY_ANDROID
                int bundleVersionCode = EditorGUILayout.IntField("BundleVersionCode", (int)m_info.select[Const.BUNDLE_VERSION_CODE]);
                if (PlayerSettings.Android.bundleVersionCode != bundleVersionCode)
                {
                    m_isSave = true;
                    m_info.select[Const.BUNDLE_VERSION_CODE] = bundleVersionCode;
                    ChangeSettings(ChangeType.BundleVersionCode);
                }
#elif UNITY_IOS
                int buildNumber = EditorGUILayout.IntField("BuildNumber", (int)m_info.select[Const.BUNDLE_VERSION_CODE]);
                if (PlayerSettings.iOS.buildNumber != buildNumber.ToString())
                {
                    m_isSave = true;
                    m_info.select[Const.BUNDLE_VERSION_CODE] = buildNumber;
                    ChangeSettings(ChangeType.BundleVersionCode);
                }
#else
                EditorGUI.BeginDisabledGroup(true);
			    EditorGUILayout.BeginHorizontal();
			    EditorGUILayout.IntField("BundleVersionCode", (int)m_info.select[Const.BUNDLE_VERSION_CODE]);
			    EditorGUILayout.LabelField("*仅Android或iOS有效");
			    EditorGUILayout.EndHorizontal();
			    EditorGUI.EndDisabledGroup();
#endif

                // 平台、宏定义
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Scripting Define Symbol", m_info.select[Const.SCRIPTING_DEFINE_SYMBOLS].ToString());
                EditorGUI.EndDisabledGroup();
                if (0 == m_info.selectIndex && GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"), GUILayout.Width(EditorGUIUtility.singleLineHeight*1.26F)))
                {
                    m_isSave = true;
                    m_info.select[Const.SCRIPTING_DEFINE_SYMBOLS] = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                    ChangeSettings(ChangeType.ScriptingDefineSymbols);
                }
                GUILayout.EndHorizontal();

                // 设置配置选项组
                GUILayout.BeginHorizontal();
                selected = EditorGUILayout.Popup("服务器配置", m_nameList.IndexOf(m_info.select[Const.NAME].ToString()), m_nameList.ToArray());
                if (selected != m_nameList.IndexOf(m_info.select[Const.NAME].ToString()))
                {
                    m_isSave = true;
                    m_info.selectIndex = selected;

                    ChangeSettings(ChangeType.ProductName);
                    ChangeSettings(ChangeType.BundleIdentifier);
                    ChangeSettings(ChangeType.Version);
                    ChangeSettings(ChangeType.BundleVersionCode);
                    ChangeSettings(ChangeType.ScriptingDefineSymbols);
                }
                if (0 == selected)
                {
                    if (GUILayout.Button("+", GUILayout.Width(EditorGUIUtility.singleLineHeight)))
                    {
                        m_addNew = true;
                    }
                }
                else if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(EditorGUIUtility.singleLineHeight*1.36F)))
                {
                    m_info.data.RemoveAt(m_info.selectIndex);
                    m_info.selectIndex = 0;
                    UpdateNameList();
                    m_isSave = true;
                }
                GUILayout.EndHorizontal();

                // 新服务器选项名
                if (m_addNew)
                {
                    GUILayout.BeginHorizontal();
                    m_newServerName = EditorGUILayout.TextField("服务器配置名", m_newServerName);
                    if (GUILayout.Button("+", GUILayout.Width(EditorGUIUtility.singleLineHeight)))
                    {
                        if (!m_nameList.Contains(m_newServerName) && !string.IsNullOrWhiteSpace(m_newServerName))
                        {
                            Dictionary<string, object> dict = new Dictionary<string, object>(m_info.select.Count);
                            foreach (var kvp in m_info.select)
                            {
                                if (kvp.Key.Equals(Const.NAME))
                                {
                                    dict.Add(kvp.Key, m_newServerName);
                                    continue;
                                }
                                dict.Add(kvp.Key, kvp.Value);
                            }
                            m_info.selectIndex = m_info.data.Count;
                            m_info.data.Add(dict);
                            UpdateNameList();
                            m_isSave = true;
                        }
                        m_newServerName = string.Empty;
                        m_addNew = false;
                    }
                    GUILayout.EndHorizontal();
                    if (m_nameList.Contains(m_newServerName) || string.IsNullOrWhiteSpace(m_newServerName))
                    {
                        EditorGUILayout.HelpBox("名字已存在或名字不合法", MessageType.Warning);
                    }
                }

                // 脚本
                EditorGUI.BeginDisabledGroup(true);
                SerializedProperty property = serializedObject.GetIterator();
                if (property.NextVisible(true))
                {
                    EditorGUILayout.PropertyField(property, new GUIContent("Script"), true, new GUILayoutOption[0]);
                }
                EditorGUI.EndDisabledGroup();

                // 其它字段属性
                EditorGUI.BeginDisabledGroup(selected != 0);
                {
                    // 登陆地址
                    value = EditorGUILayout.TextField("登录地址", m_info.select[Const.LOGIN_URL].ToString());
                    if (value != m_info.select[Const.LOGIN_URL].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.LOGIN_URL] = value;
                    }

                    // CDN
                    value = EditorGUILayout.TextField("CDN资源地址", m_info.select[Const.CDN].ToString());
                    if (value != m_info.select[Const.CDN].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.CDN] = value;
                    }

                    // 是否开启引导
                    bool bValue = EditorGUILayout.Toggle("开启新手引导?", (bool)m_info.select[Const.OPEN_GUIDE]);
                    if (bValue != (bool)m_info.select[Const.OPEN_GUIDE])
                    {
                        m_isSave = true;
                        m_info.select[Const.OPEN_GUIDE] = bValue;
                    }

                    // 是否开启更新模式
                    bValue = EditorGUILayout.Toggle("开启资源更新?", (bool)m_info.select[Const.OPEN_UPDATE]);
                    if (bValue != (bool)m_info.select[Const.OPEN_UPDATE])
                    {
                        m_isSave = true;
                        m_info.select[Const.OPEN_UPDATE] = bValue;
                    }

                    // 是否完全解锁所有功能
                    bValue = EditorGUILayout.Toggle("开启所有功能?", (bool)m_info.select[Const.UNLOCK_ALL_FUNCTION]);
                    if (bValue != (bool)m_info.select[Const.UNLOCK_ALL_FUNCTION])
                    {
                        m_isSave = true;
                        m_info.select[Const.UNLOCK_ALL_FUNCTION] = bValue;
                    }

                    // 是否开启日志
                    bValue = EditorGUILayout.Toggle("开启日志&GM工具?", (bool)m_info.select[Const.LOG]);
                    if (bValue != (bool)m_info.select[Const.LOG])
                    {
                        m_isSave = true;
                        m_info.select[Const.LOG] = bValue;
                    }

                    // 日志等级
                    if (bValue)
                    {
                        Debugger.LogLevel logLevel = (Debugger.LogLevel)EditorGUILayout.EnumPopup("日志等级", (Debugger.LogLevel)m_info.select[Const.LOGLEVEL]);
                        if (logLevel != (Debugger.LogLevel)m_info.select[Const.LOGLEVEL])
                        {
                            m_isSave = true;
                            m_info.select[Const.LOGLEVEL] = (int)logLevel;
                        }
                    }

                    // 是否开启Web日志
                    bValue = EditorGUILayout.Toggle("开启远程日志?", (bool)m_info.select[Const.WEB_LOG]);
                    if (bValue != (bool)m_info.select[Const.WEB_LOG])
                    {
                        m_isSave = true;
                        m_info.select[Const.WEB_LOG] = bValue;
                    }

                    // 远程日志白名单
                    if ((bool)m_info.select[Const.WEB_LOG])
                    {
                        value = EditorGUILayout.TextField("远程日志白名单", m_info.select[Const.WEB_LOG_IP].ToString());
                        if (value != m_info.select[Const.WEB_LOG_IP].ToString())
                        {
                            m_isSave = true;
                            string[] array = value.Split(',', ';', '|');
                            value = string.Empty;
                            for (int i = 0; i < array.Length; ++i)
                            {
                                if (!string.IsNullOrEmpty(array[i]))
                                {
                                    value += ";" + array[i];
                                }
                            }
                            if (value.StartsWith(";"))
                            {
                                value = value.Substring(1, value.Length - 1);
                            }
                            m_info.select[Const.WEB_LOG_IP] = value;
                        }
                    }

                    // [安卓]CDN资源标签
                    value = EditorGUILayout.TextField("[安卓]CDN资源标签", m_info.select[Const.ANDROID_PLATFORM_NAME].ToString());
                    if (value != m_info.select[Const.ANDROID_PLATFORM_NAME].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.ANDROID_PLATFORM_NAME] = value;
                    }

                    // [苹果]CDN资源标签
                    value = EditorGUILayout.TextField("[苹果]CDN资源标签", m_info.select[Const.IOS_PLATFORM_NAME].ToString());
                    if (value != m_info.select[Const.IOS_PLATFORM_NAME].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.IOS_PLATFORM_NAME] = value;
                    }

                    // [桌面]CDN资源标签
                    value = EditorGUILayout.TextField("[桌面]CDN资源标签", m_info.select[Const.DEFAULT_PLATFORM_NAME].ToString());
                    if (value != m_info.select[Const.DEFAULT_PLATFORM_NAME].ToString())
                    {
                        m_isSave = true;
                        m_info.select[Const.DEFAULT_PLATFORM_NAME] = value;
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (selected != 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("复制到自定义..."))
                    {
                        m_isSave = true;
                        foreach (var kvp in m_info.select)
                        {
                            if (kvp.Key.Equals(Const.NAME)) continue;
                            if (m_info.data[0].ContainsKey(kvp.Key))
                            {
                                m_info.data[0][kvp.Key] = kvp.Value;
                            }
                            else
                            {
                                m_info.data[0].Add(kvp.Key, kvp.Value);
                            }
                        }
                        m_info.selectIndex = 0;

                        ChangeSettings(ChangeType.ProductName);
                        ChangeSettings(ChangeType.BundleIdentifier);
                        ChangeSettings(ChangeType.Version);
                        ChangeSettings(ChangeType.BundleVersionCode);
                        ChangeSettings(ChangeType.ScriptingDefineSymbols);
                    }
                    if (GUILayout.Button("编辑配置..."))
                    {
                        if (File.Exists(PathUtil.versionConfigPath))
                        {
                            System.Diagnostics.Process.Start(PathUtil.versionConfigPath);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                // 打开本地沙盒资源目录
                if (GUILayout.Button("打开本地沙盒资源目录"))
                {
                    AssetBundleTool.OpenPersistentData();
                }
                // 清除本地沙盒资源
                if (GUILayout.Button("清除本地沙盒资源"))
                {
                    AssetBundleTool.DeletePersistentData();
                }
                // 清除本地沙盒版本
                if (GUILayout.Button("清除本地沙盒版本"))
                {
                    PlayerPrefs.DeleteKey(Const.SANDBOX_VERSION);
                }

                value = m_info.select[Const.SCRIPTING_DEFINE_SYMBOLS].ToString();
                index = value.Contains("AB_MODE") ? 1 : 0;
                EditorGUI.BeginDisabledGroup(index == 0);
                {
                    // 打开更新资源目录
                    if (GUILayout.Button("打开更新资源目录", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        AssetBundleTool.OpenOutputVersionPath();
                    }
                    // 一下开始执行
                    if (GUILayout.Button("全量打包资源", GUILayout.Height(EditorGUIUtility.singleLineHeight*1.28F)))
                    {
                        m_buildType = BuildType.RebuildForApp;
                    }
                    if (GUILayout.Button("增量打包资源", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.28F)))
                    {
                        m_buildType = BuildType.BuildForApp;
                    }
                    if (GUILayout.Button("全量打包资源(为热更新)", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.28F)))
                    {
                        m_buildType = BuildType.RebuildForUpdate;
                    }
                    if (GUILayout.Button("增量打包资源(为热更新)", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.28F)))
                    {
                        m_buildType = BuildType.BuildForUpdate;
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (m_isSave)
                {
                    m_isSave = false;
                    File.WriteAllText(PathUtil.versionConfigPath, JsonWriter.Serialize(m_info), Encoding.UTF8);
                    SaveVersion();
                    AssetDatabase.Refresh();
                }
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndChangeCheck();
            EditorGUI.EndDisabledGroup();
        }

        public static void Update()
        {
            if (BuildType.None != m_buildType)
            {
                try
                {
                    DateTime lastTime = DateTime.Now;
                    if (m_buildType == BuildType.RebuildForApp || m_buildType == BuildType.BuildForApp)
                    {
                        AssetBundleTool.BuildAssetBundlesWithCopy(PathUtil.outputPath, m_buildType == BuildType.RebuildForApp,
                            m_info.select[Const.INNERVERSION].ToString(), m_info.select[Const.ASSETVERSION].ToString(), true);
                    }
                    if (m_buildType == BuildType.RebuildForUpdate || m_buildType == BuildType.BuildForUpdate)
                    {
                        string url = m_info.select[Const.CDN].ToString() + "/";
#if UNITY_ANDROID
                        url += m_info.select[Const.ANDROID_PLATFORM_NAME].ToString();
#elif UNITY_IOS
                        url += m_info.select[Const.IOS_PLATFORM_NAME].ToString();
#else
                        url += m_info.select[Const.DEFAULT_PLATFORM_NAME].ToString();
#endif
                        url += "/" + string.Format(Const.REMOTE_DIRECTORY, m_info.select[Const.INNERVERSION]);
                        AssetBundleTool.BuildUpdateAssetBundlesAndZip(PathUtil.outputPath, PathUtil.outputVersionPath, m_buildType == BuildType.RebuildForUpdate,
                            m_info.select[Const.INNERVERSION].ToString(), m_info.select[Const.ASSETVERSION].ToString(), url);
                    }
                    Debugger.Log(string.Format("[{0}] 打包完成，本次打包耗时: {1:F2}秒!", m_buildType.ToString(), DateTime.Now.Subtract(lastTime).TotalSeconds));
                }
                catch (Exception e)
                {
                    Debugger.LogError(e.ToString());
                }
                m_buildType = BuildType.None;
            }

        }

        /// <summary>
        /// 保存版本配置
        /// </summary>
        private void SaveVersion()
        {
            Launch instance = target as Launch;
            string[] list = new string[] {
                    Const.PRODUCT_NAME,
                    Const.VERSION,
                    Const.INNERVERSION,
                    Const.ASSETVERSION,
                    Const.LOGIN_URL,
                    Const.CDN,
                    Const.OPEN_GUIDE,
                    Const.OPEN_UPDATE,
                    Const.UNLOCK_ALL_FUNCTION,
                    Const.LOG,
                    Const.LOGLEVEL,
                    Const.WEB_LOG,
                    Const.WEB_LOG_IP,
                    Const.ANDROID_PLATFORM_NAME,
                    Const.IOS_PLATFORM_NAME,
                    Const.DEFAULT_PLATFORM_NAME
                };
            instance.data.Clear();
            for (int i = 0; i < list.Length; ++i)
            {
                if (list[i].Equals(Const.ANDROID_PLATFORM_NAME))
                {
                    instance.data.Add(new DictPair() { key = Const.ANDROID_PLATFORM_NAME, value = m_info.select[Const.ANDROID_PLATFORM_NAME].ToString() });
                    continue;
                }
                else if (list[i].Equals(Const.IOS_PLATFORM_NAME))
                {
                    instance.data.Add(new DictPair() { key = Const.IOS_PLATFORM_NAME, value = m_info.select[Const.IOS_PLATFORM_NAME].ToString() });
                    continue;
                }
                else if (list[i].Equals(Const.DEFAULT_PLATFORM_NAME))
                {
                    instance.data.Add(new DictPair() { key = Const.DEFAULT_PLATFORM_NAME, value = m_info.select[Const.DEFAULT_PLATFORM_NAME].ToString() });
                    continue;
                }
                if (m_info.select.ContainsKey(list[i]))

                {
                    instance.data.Add(new DictPair() { key = list[i], value = m_info.select[list[i]].ToString() });
                }
            }
        }

        /// <summary>
        /// 根据模式得到宏定义
        /// </summary>
        /// <param name="selectedMode"></param>
        /// <returns></returns>
        public string GetScriptingDefineSymbolsp(string defineSymbols, int selectedMode)
        {
            switch (selectedMode)
            {
                case 0:
                    {
                        var tokens = defineSymbols.Split(';');
                        defineSymbols = string.Empty;
                        foreach (var token in tokens)
                        {
                            if (token != "AB_MODE" && !string.IsNullOrEmpty(token))
                            {
                                defineSymbols += token + ";";
                            }
                        }
                        if (defineSymbols.EndsWith(";"))
                        {
                            defineSymbols = defineSymbols.Substring(0, defineSymbols.Length - 1);
                        }
                    } break;
                case 1:
                    {
                        var tokens = defineSymbols.Split(';');
                        bool abMode = false;
                        foreach (var token in tokens)
                        {
                            if (token == "AB_MODE")
                            {
                                abMode = true;
                                break;
                            }
                        }
                        if (!abMode)
                        {
                            if (defineSymbols.Length == 0)
                            {
                                defineSymbols = "AB_MODE";
                            }
                            else
                            {
                                defineSymbols = "AB_MODE;" + defineSymbols;
                            }
                        }
                    } break;
            }
            return defineSymbols;
        }

        /// <summary>
        /// 改变设置
        /// </summary>
        /// <param name="type">Type.</param>
        private void ChangeSettings(ChangeType type)
        {
            switch (type)
            {
                case ChangeType.ProductName:
                    {
                        PlayerSettings.productName = m_info.select[Const.PRODUCT_NAME].ToString();
                    }
                    break;
                case ChangeType.BundleIdentifier:
                    {
#if UNITY_ANDROID || UNITY_IOS
                        PlayerSettings.applicationIdentifier = m_info.select[Const.BUNDLE_IDENTIFIER].ToString();
#endif
                    }
                    break;
                case ChangeType.Version:
                    {
                        PlayerSettings.bundleVersion = m_info.select[Const.VERSION].ToString();
                    }
                    break;
                case ChangeType.BundleVersionCode:
                    {
#if UNITY_ANDROID
                        PlayerSettings.Android.bundleVersionCode = (int)m_info.select[Const.BUNDLE_VERSION_CODE];
#elif UNITY_IOS
                        PlayerSettings.iOS.buildNumber = m_info.select[Const.BUNDLE_VERSION_CODE].ToString();
#endif
                    }
                    break;
                case ChangeType.ScriptingDefineSymbols:
                    {
                        string value = m_info.select[Const.SCRIPTING_DEFINE_SYMBOLS].ToString();
                        if (!PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Equals(value))
                        {
                            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, value);
                        }
                    }
                    break;
            }
        }
    }
}