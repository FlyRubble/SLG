using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityScript.Scripting.Pipeline;
using System.IO;
using System.Linq;
using System;

namespace SLG
{
    public class AssetBundleBuildWindow : EditorWindow
    {
        static AssetBundleBuildWindow g_instance;
        private Vector2 m_scrollPosition = Vector2.zero;

        public static void Open(Dictionary<string, object> dict)
        {
            g_instance = EditorWindow.GetWindowWithRect<AssetBundleBuildWindow>(new Rect(0, 0, 480, 320), false, "AssetBundleBuildWindow", true);
            g_instance.Show();
        }

        void OnGUI()
        {

        }
    }
}