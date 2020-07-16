using System.IO;
using UnityEngine;
using UnityEditor;

namespace SLG
{
    public class AssetPostprocessorEditor : AssetPostprocessor
    {
        /// <summary>
        /// 预处理
        /// </summary>
        private void OnPreprocessTexture()
        {
            if (assetPath.StartsWith("Assets/Res/"))
            {
                string directory = Path.GetDirectoryName(assetPath);
                string[] array = directory.Split('/');
                directory = array.Length > 1 ? array[array.Length - 1] : array[0];

                if (directory.EndsWith(".atlas"))
                {
                    TextureImporter textureImporter = (TextureImporter)assetImporter;
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spritePackingTag = directory;
                }

            }
        }

        /// <summary>
        /// 后处理
        /// </summary>
        /// <param name="texture"></param>
        private void OnPostprocessTexture(Texture2D texture)
        {
            if (assetPath.StartsWith("Assets/Res/"))
            {
                string directory = Path.GetDirectoryName(assetPath);
                string[] array = directory.Split('/');
                directory = array.Length > 1 ? array[array.Length - 1] : array[0];

                TextureImporter textureImporter = (TextureImporter)assetImporter;

            }
        }

        /// <summary>
        /// 所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromPath"></param>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            foreach (string move in movedAssets)
            {

            }
        }
    }
}