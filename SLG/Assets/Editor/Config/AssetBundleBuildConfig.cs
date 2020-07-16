using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace SLG
{
    [System.Serializable]
    public enum AssetBundleType
    {
        OneAssetOneBundle = 0,
        OneFolderOneBundle = 1 << 1,
        OneChildFolderOneBundle = 1 << 2,
        Ignore = 1 << 6,
    }

    [System.Serializable]
    public enum CompressType
    {
        Default = 0,
        //NoCompress = 1 << 1,
        //LZ4 = 1 << 2,
        //LZMA = 1 << 3,
    }

    [System.Serializable]
    public class AssetBundleBuildConfigUnit
    {
        [SerializeField]
        public Object m_asset;

        [SerializeField]
        public AssetBundleType m_bundleType = AssetBundleType.OneAssetOneBundle;

        [SerializeField]
        public CompressType m_compressType = CompressType.Default;
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "Tools/Creat'AB'BuildConfigFile", fileName = "BuildConfig")]
    public class AssetBundleBuildConfig : ScriptableObject
    {
        public AssetBundleBuildConfigUnit[] m_list = new AssetBundleBuildConfigUnit[0];

        /// <summary>
        /// 检测配置
        /// </summary>
        /// <param name="config"></param>
		public static void Check(AssetBundleBuildConfig config)
		{
			if (null != config)
			{
				Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
				string key = string.Empty;
				string[] value = null;
				for (int i = 0; i < config.m_list.Length; ++i)
				{
					var data = config.m_list[i];
					if (null == data.m_asset)
					{
						Debugger.Log(string.Format("Index {0} 字段[Asset]为空!!!", i));
						continue;
					}
					key = AssetDatabase.GetAssetPath(data.m_asset);
					value = new string[] { i.ToString(), string.Format("{0}:{1}", data.m_bundleType, data.m_compressType) };
					if (!dict.ContainsKey(key))
					{
						dict.Add(key, value);
					}
					else
					{
						if (dict[key][1].Equals(value[1]))
						{
							Debugger.Log(string.Format("Index {0} 与 Index {1} 重复!!!", value[0], dict[key][0]));
						}
						else
						{
							Debugger.Log(string.Format("Index {0} 与 Index {1} 冲突!!!", value[0], dict[key][0]));
						}
					}
				}
			}
		}

        /// <summary>
        /// 检测配置
        /// </summary>
		public static void CheckAll()
		{
            List<string> pathList = new List<string>();
            pathList.AddRange(AssetDatabase.GetAllAssetPaths());
            pathList = pathList.FindAll(path => typeof(AssetBundleBuildConfig) == AssetDatabase.GetMainAssetTypeAtPath(path));

            foreach (var path in pathList)
            {
                Check(AssetDatabase.LoadAssetAtPath<AssetBundleBuildConfig>(path));
            }
        }
    }
}