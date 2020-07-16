using System.Collections.Generic;
using UnityEngine;
using SLG;
using SLG.Event;
using SLG.UI;
using SLG.UnityAsset;
using SLG.JsonFx;
using SLG.IO;

public class LuaCallCS
{
    #region UnityAsset
    public static UnityAsyncAsset AssetBundleLoad(string path)
    {
        return AssetManager.instance.AssetBundleLoad(path);
    }

    public static UnityAsyncAsset LoadConf(string name, Action<bool, UnityAsyncAsset> complete, bool async = true)
    {
        return AssetManager.instance.LoadConf(name, complete, async);
    }

    public static void UnloadAssets(bool unloadAllLoadedObjects)
    {
        AssetManager.instance.UnloadAssets(unloadAllLoadedObjects);
    }

    public static void UnloadAsset(AsyncAsset asset, bool unloadAllLoadedObjects)
    {
        AssetManager.instance.UnloadAsset(asset, unloadAllLoadedObjects);
    }
    #endregion

    #region UIManager
    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loadFinish"></param>
    public static void OpenUI(string name, Action<UIBase> loadFinish)
    {
        UIManager.instance.OpenUI(name, loadFinish);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="name"></param>
    public static void CloseUI(string name)
    {
        UIManager.instance.CloseUI(name);
    }

    /// <summary>
    /// 清理UI
    /// </summary>
    public static void ClearUI()
    {
        UIManager.instance.Clear();
    }
    #endregion

    #region Manifest
    public static void UpdateManifestConfig(string text)
    {
        App.manifest = JsonReader.Deserialize<ManifestConfig>(text);
    }

    public static void UpdateManifestMappingConfig(string text)
    {
        App.manifestMapping = JsonReader.Deserialize<ManifestMappingConfig>(text);
    }
    #endregion
}
