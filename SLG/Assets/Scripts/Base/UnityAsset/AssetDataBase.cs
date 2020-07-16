using UnityEngine;

namespace SLG
{
    namespace UnityAsset
    {
        public sealed class AssetDataBase : AsyncAsset
        {
            #region Variable
            /// <summary>
            /// 资源
            /// </summary>
            private Object m_mainAsset = null;
            #endregion

            #region Property
            /// <summary>
            /// 记载资源的类型
            /// </summary>
            public override LoadType loadType
            {
                get { return LoadType.AssetDataBase; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            public override bool isDone
            {
                get { return m_mainAsset != null; }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public override float progress
            {
                get
                {
                    m_progress = m_mainAsset != null ? 1F : 0f;
                    return m_progress;
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public override string error
            {
                get
                {
                    return m_mainAsset != null ? "" : "unknown error";
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public AssetDataBase(string url)
                : base(url)
            { }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public override void AsyncLoad()
            {
                base.AsyncLoad();
#if UNITY_EDITOR
                m_mainAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(m_url, typeof(Object));
#endif
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override Object LoadAsset(string name)
            {
                return m_mainAsset;
            }

            /// <summary>
            /// 卸载
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public override void Unload(bool unloadAllLoadedObjects = true)
            {
                base.Unload(unloadAllLoadedObjects);
                m_mainAsset = null;
            }
            #endregion
        }
    }
}