using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SLG.Event;

namespace SLG
{
    public class PathUtil
    {
        /// <summary>
        /// 得到资源文件夹路径(绝对路径,以Assets目录结尾)
        /// </summary>
        public static string assetPath => Application.dataPath;

        /// <summary>
        /// StreamingAssets文件夹路径(绝对路径,以StreamingAssets目录结尾)
        /// </summary>
        public static string streamingAssets => Application.streamingAssetsPath;

        /// <summary>
        /// 得到持久化路径
        /// </summary>
        public static string persistentDataPath => Application.persistentDataPath;

        /// <summary>
        /// 打包资源直接输出路径
        /// </summary>
        public static string outputPath => assetPath + "/../AssetBundles/" + Util.GetPlatformSign();

        /// <summary>
        /// 热更资源输出路径
        /// </summary>
        public static string outputVersionPath => assetPath + "/../AssetBundles/Version/" + Util.GetPlatformSign();

        /// <summary>
        /// 清单文件配置路径
        /// </summary>
        public static string manifestConfigPath => assetPath + "/Res/Manifest.json";

        /// <summary>
        /// 清单映射文件配置路径
        /// </summary>
        public static string manifestMappingConfigPath => assetPath + "/Res/ManifestMapping.json";

        /// <summary>
        /// 获取版本信息配置路径
        /// </summary>
        public static string versionConfigPath => assetPath + "/Res/Version.json";


        /// <summary>
        /// 持久化数据URL
        /// </summary>
        public static string persistentDataUrl
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return "jar:file://" + Application.persistentDataPath + "/";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return "file://" + Application.persistentDataPath + "/";
                }
                else
                {
                    return "file:///" + Application.persistentDataPath + "/";
                }
            }
        }

        /// <summary>
        /// 流式数据URL
        /// </summary>
        public static string streamingAssetsPath
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return Application.streamingAssetsPath + "/";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return "file://" + Application.streamingAssetsPath + "/";
                }
                else
                {
                    return "file://" + Application.streamingAssetsPath + "/";
                }
            }
        }

        /// <summary>
        /// 得到真实的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRealUrl(string path)
        {
            if (path.StartsWith("http"))
            {
                return path;
            }
            // 选择从沙盒路径还是流式路径加载清单
            bool sandbox = File.Exists(Path.Combine(PathUtil.persistentDataPath, path));
            return (sandbox ? persistentDataUrl : streamingAssetsPath) + path;
        }
    }
}
