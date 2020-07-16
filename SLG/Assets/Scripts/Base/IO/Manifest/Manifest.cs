using UnityEngine;
using System.Collections.Generic;
using System.Xml;

namespace SLG
{
    namespace IO
    {
        /// <summary>
        /// 资源单子
        /// </summary>
        public class Manifest
        {
            #region Variable
            /// <summary>
            /// 名字
            /// </summary>
            protected string m_name = string.Empty;
            /// <summary>
            /// MD5
            /// </summary>
            string m_MD5 = string.Empty;

            /// <summary>
            /// Size
            /// </summary>
            long m_size = 0;

            /// <summary>
            /// 依赖文件名
            /// </summary>
            List<string> m_dependencies = null;
            #endregion

            #region Property
            /// <summary>
            /// 得到或设置名字
            /// </summary>
            /// <value>The name.</value>
            public string name
            {
                get { return m_name; }
                set { m_name = value; }
            }

            /// <summary>
            /// 得到或设置MD5
            /// </summary>
            /// <value>The M d5.</value>
            public string MD5
            {
                get { return m_MD5; }
                set { m_MD5 = value; }
            }

            /// <summary>
            /// 文件大小KB
            /// </summary>
            public long size
            {
                get { return m_size; }
                set { m_size = value; }
            }

            /// <summary>
            /// 得到依赖文件名
            /// </summary>
            /// <value>The file names.</value>
            public List<string> dependencies
            {
                get { return m_dependencies; }
                set { m_dependencies = value; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public Manifest()
            {
                m_dependencies = new List<string>();
            }
            #endregion
        }
    }
}
