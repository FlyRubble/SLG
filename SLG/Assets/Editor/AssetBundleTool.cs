using UnityEngine;
using UnityEditor;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System.Collections.Generic;
using System.Linq;

namespace SLG
{
    using IO;
    using JsonFx;

    /// <summary>
    /// 资源打包
    /// </summary>
    public class AssetBundleTool
    {
        /// <summary>
        /// AssetBundle File Info
        /// </summary>
        struct ABFI
        {
            public string md5;
            public long size;
        }

        /// <summary>
        /// 当前Build的目标平台
        /// </summary>
        public static BuildTarget currentBuildTarget => EditorUserBuildSettings.activeBuildTarget;

        /// <summary>
        /// 打包选项
        /// </summary>
        public static BuildAssetBundleOptions assetBundleOptions => BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

        /// <summary>
        /// 得到资源版本
        /// </summary>
        /// <returns></returns>
        public static long GetAssetVersion()
        {
            long version = 100000000;
            version *= DateTime.Now.Year;
            version += DateTime.Now.Month * 1000000 + DateTime.Now.Day * 10000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            return version;
        }

        /// <summary>
        /// 移除AssetBundleName
        /// </summary>
        public static void RemoveAssetBundleNames()
        {
            string[] names = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < names.Length; ++i)
            {
                AssetDatabase.RemoveAssetBundleName(names[i], true);
            }
        }

        /// <summary>
        /// 删除持久化数据
        /// </summary>
        public static void DeletePersistentData()
        {
            if (Directory.Exists(PathUtil.persistentDataPath))
            {
                Directory.Delete(PathUtil.persistentDataPath, true);
            }
        }

        /// <summary>
        /// 打开持久化路径
        /// </summary>
        public static void OpenPersistentData()
        {
            string persistentDataPath = PathUtil.persistentDataPath.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", persistentDataPath);
        }

        /// <summary>
        /// 打开输出版本资源路径
        /// </summary>
        public static void OpenOutputVersionPath()
        {
            string outputVersionPath = PathUtil.outputVersionPath.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", outputVersionPath);
        }

        /// <summary>
        /// 删除导出的资源目录
        /// </summary>
        public static void DeleteExportDirectory(string exportPath)
        {
            if (Directory.Exists(exportPath))
            {
                Directory.Delete(exportPath, true);
            }
        }

        /// <summary>
        /// 得到AB包名
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="compressType"></param>
        /// <returns></returns>
        private static string GetAssetBundleName(string assetPath, CompressType compressType)
        {
            string assetBundleName = assetPath;
            if (CompressType.Default == compressType)
            {
                assetBundleName = assetPath.Replace("Assets/", "");
            }
            else
            {
                assetBundleName = assetPath.Replace("Assets", compressType.ToString());
            }
            return assetBundleName;
        }

        /// <summary>
        /// 设置AB包名
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="compressType"></param>
        /// <param name="assetBundleName"></param>
        public static void SetAssetBundleName(string assetPath, CompressType compressType, string assetBundleName = "")
        {
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            assetBundleName = string.IsNullOrEmpty(assetBundleName) ? assetPath : assetBundleName;
            string extension = Path.GetExtension(assetBundleName);
            if (!string.IsNullOrEmpty(extension))
            {
                assetBundleName = assetBundleName.Replace(extension, "");
            }
            extension = Const.ASSETBUNDLEVARIANT;
            assetBundleName = GetAssetBundleName(assetBundleName, compressType);
            importer.SetAssetBundleNameAndVariant(assetBundleName, extension);
        }

        /// <summary>
        /// 设置图集
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="relativePath"></param>
        private static void SetSpriteAtlas(string relativePath)
        {
            string spriteAtlasPath = relativePath + ".spriteatlas";
            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(spriteAtlasPath);
            if (spriteAtlas == null)
            {
                spriteAtlas = new SpriteAtlas();
                AssetDatabase.CreateAsset(spriteAtlas, spriteAtlasPath);

                SpriteAtlasPackingSettings spriteAtlasPackingSettings = SpriteAtlasExtensions.GetPackingSettings(spriteAtlas);
                spriteAtlasPackingSettings.enableTightPacking = false;
                spriteAtlasPackingSettings.padding = 1;
                SpriteAtlasTextureSettings spriteAtlasTextureSettings = SpriteAtlasExtensions.GetTextureSettings(spriteAtlas);
                spriteAtlasTextureSettings.sRGB = true;

                var obj = AssetDatabase.LoadMainAssetAtPath(relativePath);
                UnityEngine.Object[] objects = new UnityEngine.Object[] { obj };
                SpriteAtlasExtensions.Add(spriteAtlas, objects);
            }
        }

        /// <summary>
        /// 设置AB包名
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="rootPath"></param>
        /// <param name="path"></param>
        /// <param name="compressType"></param>
        private static void SetAssetBundleNameByOneAssetOneBundle(Dictionary<string, AssetBundleBuildConfigUnit> dict, string rootPath, string path, CompressType compressType)
        {
            string[] files = new string[] { path };
            if (Directory.Exists(path))
            {
                files = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    string assetPath = Util.GetUniformityPath(file);
                    if (dict.ContainsKey(assetPath))
                    {
                        continue;
                    }
                    SetAssetBundleNameByOneAssetOneBundle(dict, rootPath, file, compressType);
                }

                files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            }

            files = files.Where(file => !file.Contains(".meta")).ToArray();
            foreach (var file in files)
            {
                string assetPath = Util.GetUniformityPath(file);
                if (!rootPath.Equals(file) && dict.ContainsKey(assetPath))
                {
                    continue;
                }

                SetAssetBundleName(assetPath, compressType);
            }
        }

        /// <summary>
        /// 设置AB包名
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="rootPath"></param>
        /// <param name="path"></param>
        /// <param name="compressType"></param>
        private static void SetAssetBundleNameByOneFolderOneBundle(Dictionary<string, AssetBundleBuildConfigUnit> dict, string rootPath, string path, CompressType compressType)
        {
            string[] files = new string[] { path };
            if (Directory.Exists(path))
            {
                files = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    string assetPath = Util.GetUniformityPath(file);
                    if (dict.ContainsKey(assetPath))
                    {
                        continue;
                    }
                    SetAssetBundleNameByOneFolderOneBundle(dict, rootPath, file, compressType);
                }

                files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            }

            files = files.Where(file => !file.Contains(".meta")).ToArray();
            foreach (var file in files)
            {
                string assetPath = Util.GetUniformityPath(file);
                if (!rootPath.Equals(file) && dict.ContainsKey(assetPath))
                {
                    continue;
                }

                SetAssetBundleName(assetPath, compressType, rootPath);
            }
        }

        /// <summary>
        /// 设置AB包名
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="rootPath"></param>
        /// <param name="path"></param>
        /// <param name="compressType"></param>
        private static void SetAssetBundleNameByOneChildFolderOneBundle(Dictionary<string, AssetBundleBuildConfigUnit> dict, string rootPath, string path, CompressType compressType)
        {
            string[] files = new string[] { path };
            if (Directory.Exists(path))
            {
                files = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    string assetPath = Util.GetUniformityPath(file);
                    if (dict.ContainsKey(assetPath))
                    {
                        continue;
                    }
                    SetAssetBundleNameByOneFolderOneBundle(dict, assetPath, file, compressType);
                }

                files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            }

            files = files.Where(file => !file.Contains(".meta")).ToArray();
            foreach (var file in files)
            {
                string assetPath = Util.GetUniformityPath(file);
                if (!rootPath.Equals(file) && dict.ContainsKey(assetPath))
                {
                    continue;
                }

                SetAssetBundleName(assetPath, compressType);
            }
        }

        public static void SetAssetBundleNames(string configPath = null)
        {
            // 找出AB配置文件
            List<string> pathList = new List<string>();
            if (string.IsNullOrEmpty(configPath))
            {
                pathList.AddRange(AssetDatabase.GetAllAssetPaths());
                pathList = pathList.FindAll(path => typeof(AssetBundleBuildConfig) == AssetDatabase.GetMainAssetTypeAtPath(path));
            }
            else
            {
                string[] array = configPath.Split('|', ',', ';');
                for (int i = 0; i < array.Length; ++i)
                {
                    if (typeof(AssetBundleBuildConfig) == AssetDatabase.GetMainAssetTypeAtPath(array[i]))
                    {
                        pathList.Add(array[i]);
                    }
                }
            }
            // 读取需要的AB配置文件
            Dictionary<string, AssetBundleBuildConfigUnit> dict = new Dictionary<string, AssetBundleBuildConfigUnit>();
            foreach (var path in pathList)
            {
                AssetBundleBuildConfig config = AssetDatabase.LoadAssetAtPath<AssetBundleBuildConfig>(path);
                AssetBundleBuildConfig.Check(config);
                foreach (var configUnit in config.m_list)
                {
                    string key = string.Empty;
                    if (null == configUnit.m_asset)
                    {
                        continue;
                    }

                    key = Util.GetUniformityPath(AssetDatabase.GetAssetPath(configUnit.m_asset));
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, configUnit);
                    }
                }
            }

            foreach (var kvp in dict)
            {
                if (AssetBundleType.OneAssetOneBundle == kvp.Value.m_bundleType)
                {
                    SetAssetBundleNameByOneAssetOneBundle(dict, kvp.Key, kvp.Key, kvp.Value.m_compressType);
                }
                else if (AssetBundleType.OneFolderOneBundle == kvp.Value.m_bundleType)
                {
                    SetAssetBundleNameByOneFolderOneBundle(dict, kvp.Key, kvp.Key, kvp.Value.m_compressType);
                }
                else if (AssetBundleType.OneChildFolderOneBundle == kvp.Value.m_bundleType)
                {
                    SetAssetBundleNameByOneChildFolderOneBundle(dict, kvp.Key, kvp.Key, kvp.Value.m_compressType);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 资源打包
        /// </summary>
        public static void BuildAssetBundles(string output, bool rebuild)
        {
            // 更新Lua
            string newDirectory = PathUtil.assetPath + "/Res/Lua";
            FileUtil.DeleteFileOrDirectory(newDirectory);
            string luaDirectory = Util.GetUniformityPath(PathUtil.assetPath + "/Lua");
            string[] paths = Directory.GetFiles(luaDirectory, "*.lua", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; ++i)
            {
                string newPath = Path.ChangeExtension(Util.GetUniformityPath(paths[i]).Replace(luaDirectory, newDirectory), ".bytes");
                string directory = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                FileUtil.CopyFileOrDirectory(paths[i], newPath);
            }
            // 设置图集
            string[] directoryPaths = Directory.GetDirectories(PathUtil.assetPath, "*.atlas", SearchOption.AllDirectories);
            foreach (var directoryPath in directoryPaths)
            {
                string relativePath = directoryPath.Replace(PathUtil.assetPath, "Assets");
                SetSpriteAtlas(relativePath);
            }
            // 设置AssetBundle名
            AssetDatabase.Refresh();
            SetAssetBundleNames();
            // Pack图集
            SpriteAtlasUtility.PackAllAtlases(currentBuildTarget);

            // 打包
            if (rebuild)
            {
                DeleteExportDirectory(output);
            }
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            BuildPipeline.BuildAssetBundles(output, assetBundleOptions, currentBuildTarget);
            // 移除所有assetBundleName
            RemoveAssetBundleNames();
            // 刷新
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 清单文件
        /// </summary>
        /// <param name="output"></param>
        public static void BuildManifestFile(string output, string innerVersion, string assetVersion)
        {
            ManifestConfig manifestConfig = GetManifest(output);
            // 写入Manifest
            if (manifestConfig != null)
            {
                // *************************************************************************
                // 生成映射表
                ManifestMappingConfig config = new ManifestMappingConfig();
                bool START = false;
                foreach (var path in manifestConfig.data.Keys)
                {
                    string[] lines = File.ReadAllLines(output + "/" + path + ".manifest");
                    START = false;

                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Assets:"))
                        {
                            START = true;
                            continue;
                        }
                        else if (line.StartsWith("Dependencies:"))
                        {
                            break;
                        }
                        if (START)
                        {
                            config.Add(Util.GetUniformityPath(line).Replace("- Assets/", "").ToLower(), path);
                        }
                    }
                }
                File.WriteAllText(PathUtil.manifestMappingConfigPath, JsonWriter.Serialize(config));
                // 刷新
                AssetDatabase.Refresh();
                // Build清单映射文件
                AssetBundleBuild[] builds = new AssetBundleBuild[1];
                builds[0].assetBundleName = "manifestmapping";
                builds[0].assetBundleVariant = null;
                builds[0].assetNames = new string[1] { PathUtil.manifestMappingConfigPath.Replace(PathUtil.assetPath, "Assets") };
                BuildPipeline.BuildAssetBundles(output, builds, assetBundleOptions, currentBuildTarget);
                // *************************************************************************
                // 写入到文件
                Manifest manifest = new Manifest();
                manifest.name = "manifestmapping";
                ABFI ab = GetABFI(Path.Combine(output, manifest.name));
                manifest.MD5 = ab.md5;
                manifest.size = ab.size;
                manifestConfig.Add(manifest);
                manifestConfig.innerVersion = string.IsNullOrEmpty(innerVersion) ? GetAssetVersion().ToString() : innerVersion;
                manifestConfig.assetVersion = string.IsNullOrEmpty(assetVersion) ? GetAssetVersion().ToString() : assetVersion;
                File.WriteAllText(PathUtil.manifestConfigPath, JsonWriter.Serialize(manifestConfig));
                // 刷新
                AssetDatabase.Refresh();
                // Build清单文件
                builds = new AssetBundleBuild[2];
                builds[0].assetBundleName = "manifest";
                builds[0].assetBundleVariant = null;
                builds[0].assetNames = new string[1] { PathUtil.manifestConfigPath.Replace(PathUtil.assetPath, "Assets") };
                BuildPipeline.BuildAssetBundles(output, builds, assetBundleOptions, currentBuildTarget);
            }
        }

        /// <summary>
        /// 删除清单文件
        /// </summary>
        private static void DeleteManifestFile()
        {
            // 删除文件
            FileUtil.DeleteFileOrDirectory(PathUtil.manifestConfigPath);
            FileUtil.DeleteFileOrDirectory(PathUtil.manifestMappingConfigPath);
        }

        /// <summary>
        /// 资源打包并拷贝
        /// </summary>
        /// <param name="output"></param>
        public static void BuildAssetBundlesWithCopy(string output, bool rebuild, string innerVersion, string assetVersion = "", bool copy = false)
        {
            BuildAssetBundles(output, rebuild);
            BuildManifestFile(output, innerVersion, assetVersion);
            if (copy)
            {
                CopyAssetBundles(output);
            }
            DeleteManifestFile();
        }

        /// <summary>
        /// 资源打包并压缩更新资源包
        /// </summary>
        /// <param name="output"></param>
        /// <param name="dest"></param>
        /// <param name="version"></param>
        /// <param name="platform"></param>
        /// <param name="cdn"></param>
        public static string BuildUpdateAssetBundlesAndZip(string output, string dest, bool rebuild, string innerVersion, string assetVersion = "", string cdn = null)
        {
            BuildAssetBundles(output, rebuild);
            BuildManifestFile(output, innerVersion, assetVersion);
            string zipPath = CopyUpdateAssetBundles(output, dest, innerVersion, assetVersion, cdn);
            DeleteManifestFile();
            return zipPath;
        }

        /// <summary>
        /// 拷贝资源包
        /// </summary>
        public static void CopyAssetBundles(string output)
        {
            // 拷贝资源
            if (Directory.Exists(PathUtil.streamingAssets))
            {
                FileUtil.DeleteFileOrDirectory(PathUtil.streamingAssets);
            }

            AssetDatabase.Refresh();
            FileUtil.CopyFileOrDirectory(output, PathUtil.streamingAssets);
            string[] filePaths = Directory.GetFiles(PathUtil.streamingAssets, "*.manifest", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                FileUtil.DeleteFileOrDirectory(filePath);
            }
            string destPath = PathUtil.streamingAssets + "/" + Util.GetPlatformSign();
            if (File.Exists(destPath))
            {
                FileUtil.DeleteFileOrDirectory(destPath);
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 拷贝更新资源包
        /// </summary>
        public static string CopyUpdateAssetBundles(string output, string dest, string innerVersion, string assetVersion = "", string cdn = null)
        {
            dest += "/v" + innerVersion;
            string zipPath = string.Empty;
            // 获取远程清单
            ManifestConfig remote = new ManifestConfig();
            if (!string.IsNullOrEmpty(cdn))
            {
                string url = cdn + "/manifest";
                WWW www = new WWW(url);
                while (!www.isDone) ;
                if (string.IsNullOrEmpty(www.error) && www.progress == 1F)
                {
                    TextAsset text = www.assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(url)) as TextAsset;
                    remote = JsonReader.Deserialize<ManifestConfig>(text.text);
                    www.assetBundle.Unload(true);
                }
                www.Dispose();
            }

            ManifestConfig local = JsonReader.Deserialize<ManifestConfig>(File.ReadAllText(PathUtil.manifestConfigPath));
            if (local != null)
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                AssetDatabase.Refresh();

                using (MemoryStream stream = new MemoryStream())
                {
                    using (ZipOutputStream zip = new ZipOutputStream(stream))
                    {
                        assetVersion = string.IsNullOrEmpty(assetVersion) ? GetAssetVersion().ToString() : assetVersion;
                        zip.SetComment(string.Format("innerVersion:{0}\nassetVersion:{1}", innerVersion, assetVersion));
                        Action<string> action = (name) =>
                        {
                            ZipEntry entry = new ZipEntry("v" + innerVersion + "/" + name);
                            entry.DateTime = new DateTime();
                            entry.DosTime = 0;
                            zip.PutNextEntry(entry);

                            string filepPath = output + "/" + name;
                            var bytes = File.ReadAllBytes(filepPath);
                            zip.Write(bytes, 0, bytes.Length);
                        };
                        action("manifest");
                        foreach (var data in local.data.Values)
                        {
                            if (remote.Contains(data.name) && remote.Get(data.name).MD5 == data.MD5)
                            {
                                continue;
                            }
                            action(data.name);
                        }
                        zip.Finish();
                        zip.Flush();

                        var fileBytes = new byte[stream.Length];
                        Array.Copy(stream.GetBuffer(), fileBytes, fileBytes.Length);

                        string md5 = Util.GetMD5(fileBytes);
                        zipPath = string.Format("{0}/{1}_{2}.zip", dest, assetVersion, md5);
                        File.WriteAllBytes(zipPath, fileBytes);
                    }
                }
            }
            return zipPath;
        }

        /// <summary>
        /// 得到MD5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static ABFI GetABFI(string path)
        {
            ABFI ab = new ABFI();
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                ab.size = Mathf.CeilToInt((fs.Length / 1024f));
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                ab.md5 = sb.ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ab;
        }

        /// <summary>
        /// 得到清单文件
        /// </summary>
        /// <returns></returns>
        private static ManifestConfig GetManifest(string output)
        {
            string filePath = Util.GetUniformityPath(output);
            filePath += "/" + filePath.Split('/')[filePath.Split('/').Length - 1];
            ManifestConfig manifestConfig = null;
            if (File.Exists(filePath))
            {
                manifestConfig = new ManifestConfig();
                var bundle = AssetBundle.LoadFromFile(filePath);
                AssetBundleManifest abManifest = bundle.LoadAsset("assetbundlemanifest") as AssetBundleManifest;
                string[] bundleNames = abManifest.GetAllAssetBundles();
                for (int i = 0; i < bundleNames.Length; ++i)
                {
                    Manifest manifest = new Manifest();
                    manifest.name = bundleNames[i];
                    ABFI ab = GetABFI(Path.Combine(output, bundleNames[i]));
                    manifest.MD5 = ab.md5;
                    manifest.size = ab.size;
                    foreach (var dependenciesName in abManifest.GetDirectDependencies(bundleNames[i]))
                    {
                        manifest.dependencies.Add(dependenciesName);
                    }
                    manifestConfig.Add(manifest);
                }
                bundle.Unload(true);
            }
            return manifestConfig;
        }
    }
}