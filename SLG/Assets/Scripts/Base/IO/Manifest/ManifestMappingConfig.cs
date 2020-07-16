using System.Collections.Generic;

namespace SLG
{
    namespace IO
    {
        /// <summary>
        /// 清单映射配置
        /// </summary>
        public class ManifestMappingConfig
        {
            #region Variable
            /// <summary>
            /// 数据
            /// </summary>
            private Dictionary<string, string> m_data = null;
            #endregion

            #region Property
            /// <summary>
            /// 得到数据
            /// </summary>
            /// <value>The data.</value>
            public Dictionary<string, string> data
            {
                get { return m_data; }
                set { m_data = value; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public ManifestMappingConfig()
            {
                m_data = new Dictionary<string, string>(1<<11);
            }

            /// <summary>
            /// 带参数构造
            /// </summary>
            /// <param name="data"></param>
            public ManifestMappingConfig(Dictionary<string, string> data)
            {
                m_data = new Dictionary<string, string>(1 << 11);
                foreach (var kvp in data)
                {
                    m_data.Add(kvp.Key, kvp.Value);
                }
            }

            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="assetName"></param>
            /// <param name="assetBundleName"></param>
            public void Add(string assetName, string assetBundleName)
            {
                if (string.IsNullOrEmpty(assetName) || string.IsNullOrEmpty(assetBundleName))
                {
                    return;
                }
                if (m_data.ContainsKey(assetName))
                {
                    Debugger.LogError(string.Format("资源名: {0} 重复", assetName));
                }
                else
                {
                    m_data.Add(assetName, assetBundleName);
                }
            }

            /// <summary>
            /// 根据资源名得到资源包名
            /// </summary>
            /// <param name="assetName"></param>
            /// <returns></returns>
            public string Get(string assetName)
            {
                return m_data.ContainsKey(assetName) ? m_data[assetName] : assetName;
            }

            /// <summary>
            /// 是否包含值
            /// </summary>
            /// <param name="assetName"></param>
            /// <returns></returns>
            public bool Contains(string assetName)
            {
                return m_data.ContainsKey(assetName);
            }
            #endregion
        }
    }
}