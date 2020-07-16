using UnityEngine;

namespace SLG
{
    using Event;

    namespace UnityAsset
    {
        public static class AssetManagerExtensions
        {
            /// <summary>
            /// 加载UI
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="action"></param>
            /// <param name="async"></param>
            /// <returns></returns>
            public static void LoadUI(this AssetManager self, string path, Action<bool, Object> complete, Action<bool, UnityAsyncAsset> action = null, bool async = true)
            {
                path = Const.LOADUI + path + ".prefab";
#if UNITY_EDITOR && !AB_MODE
                path = "Assets/" + path;
                complete?.Invoke(true, UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
#else
                path = path.ToLower();
                if (async)
                {
                    self.AssetBundleAsyncLoad(path, (bResult, asset) =>
                    {
                        complete?.Invoke(bResult, asset.mainAsset);
                    }, action);
                }
                else
                {
                    UnityAsyncAsset unityAsyncAsset = self.AssetBundleLoad(path);
                    complete?.Invoke(string.IsNullOrEmpty(unityAsyncAsset.error), unityAsyncAsset.mainAsset);
                }
#endif
            }

            /// <summary>
            /// 加载UI
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="action"></param>
            /// <param name="async"></param>
            /// <returns></returns>
            public static void LoadLua(this AssetManager self, string path, Action<bool, byte[]> complete, Action<bool, UnityAsyncAsset> action = null, bool async = true)
            {
#if UNITY_EDITOR && !AB_MODE
                path = PathUtil.assetPath + "/Lua/" + path + ".lua";
                complete?.Invoke(true, System.IO.File.ReadAllBytes(path));
#else
                path = Const.LOADLUA + path + ".bytes";
                path = path.ToLower();
                if (async)
                {
                    self.AssetBundleAsyncLoad(path, (bResult, asset) =>
                    {
                        complete?.Invoke(bResult, asset.asyncAsset.bytes);
                    }, action);
                }
                else
                {
                    UnityAsyncAsset unityAsyncAsset = self.AssetBundleLoad(path);
                    complete?.Invoke(string.IsNullOrEmpty(unityAsyncAsset.error), System.Text.Encoding.Default.GetBytes(unityAsyncAsset.text));
                }
#endif
            }

            /// <summary>
            /// 加载配置文件
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="async"></param>
            public static UnityAsyncAsset LoadConf(this AssetManager self, string path, Action<bool, UnityAsyncAsset> complete, bool async = true)
            {
                UnityAsyncAsset unityAsyncAsset;
                path = Const.LOADCONF + path + ".json";
#if UNITY_EDITOR && !AB_MODE
                path = "Assets/" + path;
                AssetDataBase assetDataBase = new AssetDataBase(path);
                assetDataBase.AsyncLoad();
                unityAsyncAsset = new UnityAsyncAsset(path, assetDataBase, complete);
                complete?.Invoke(true, unityAsyncAsset);

#else
                path = path.ToLower();
                if (async)
                {
                    unityAsyncAsset = self.AssetBundleAsyncLoad(path, (bResult, asset) =>
                    {
                        complete?.Invoke(bResult, asset);
                    }, null);
                }
                else
                {
                    unityAsyncAsset = self.AssetBundleLoad(path);
                    complete?.Invoke(string.IsNullOrEmpty(unityAsyncAsset.error), unityAsyncAsset);
                }
#endif
                return unityAsyncAsset;
            }
        }
    }
}